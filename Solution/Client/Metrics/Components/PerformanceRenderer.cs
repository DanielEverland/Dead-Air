using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Networking;
using TMPro;
using System.Text;
using Components;

namespace Metrics.Components
{
    public class PerformanceRenderer : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        private float _interval = 1;
        [SerializeField]
        private Material _lineMaterial;
        [SerializeField]
        private RectTransform _gridRectTransform;
        [SerializeField]
        private TMP_Text _textElement;
        [SerializeField]
        private Color _backgroundLineColor;
        [SerializeField]
        private Color _backgroundColor;
        [SerializeField]
        private Color _mediumSeverityTextColor;
        [SerializeField]
        private Color _highSeverityTextColor;
        [SerializeField]
        private Color _normalTextColor;

        [Space()]

        [SerializeField]
        private Color _pingColor;
        [SerializeField]
        private Color _clientFPSColor;
        [SerializeField]
        private Color _serverFPSColor;
        [SerializeField]
        private Color _packetLossColor;
#pragma warning restore

        private const int HISTORY_BUFFER_LENGTH = 20;
        private const int BACKGROUND_LINES = 3;
        
        private List<DataEntry> _entries;
        private float _timeSinceLastUpdate = float.MaxValue;
        private Rect _rect;

        private List<DataHeader> _headers;

        private void Awake()
        {
            _headers = new List<DataHeader>()
            {
                new DataHeader("Ping", _pingColor, x => x.Ping, x => x.Ping > 200, x => x.Ping > 500, 1000),
                new DataHeader("Client FPS", _clientFPSColor, x => x.ClientFramerate, x => x.ClientFramerate < 120, x => x.ClientFramerate < 60, 144, 0),
                new DataHeader("Server FPS", _serverFPSColor, x => x.ServerFramerate, x => x.ServerFramerate < Client.ServerInformation.ServerSendRate, x => x.ServerFramerate < (float)Client.ServerInformation.ServerSendRate / 2, 144, 0),
                new DataHeader("Packet Loss", _packetLossColor, x => x.PacketLoss, x => x.PacketLoss > 20, x => x.PacketLoss > 30, 100, 0),
            };

            _entries = new List<DataEntry>(HISTORY_BUFFER_LENGTH);
            for (int i = 0; i < HISTORY_BUFFER_LENGTH; i++)
            {
                _entries.Add(new DataEntry());
            }

            SetText();
        }
        private void Update()
        {
            if (!Client.IsConnected)
                return;

            if(_timeSinceLastUpdate > _interval)
            {
                AddEntry(DataEntry.Get());
                SetText();

                _timeSinceLastUpdate = 0;
            }
            else
            {
                _timeSinceLastUpdate += Time.deltaTime;
            }
        }
        private void SetText()
        {
            StringBuilder builder = new StringBuilder();

            DataEntry entry = _entries[0];
            foreach (DataHeader header in _headers)
            {
                builder.Append("<color=#");
                builder.Append(ColorUtility.ToHtmlStringRGB(header.Color));
                builder.Append(">");
                builder.Append($"{header.Name}: ");
                
                builder.Append("<color=#");
                builder.Append(ColorUtility.ToHtmlStringRGB(GetColor(header, entry)));
                builder.Append(">");
                builder.Append(header.GetValue(entry));
                builder.Append("</color>");


                builder.AppendLine();
            }

            _textElement.text = builder.ToString();
        }
        private Color GetColor(DataHeader header, DataEntry entry)
        {
            float value = header.GetValue(entry);

            if(header.IsHighSeverity(entry))
            {
                return _highSeverityTextColor;
            }
            else if(header.IsMediumSeverity(entry))
            {
                return _mediumSeverityTextColor;
            }
            else
            {
                return _normalTextColor;
            }
        }
        private void OnGUI()
        {
            if(Event.current.type == EventType.Repaint)
            {
                DrawGL();
            }
        }
        private void AddEntry(DataEntry entry)
        {
            _entries.Insert(0, entry);
            _entries.RemoveAt(_entries.Count - 1);
        }
        private void DrawGL()
        {
            _rect = _gridRectTransform.GetRectInScreenSpace();
            _rect.y = Screen.height - _rect.y - _rect.height;

            GL.PushMatrix();
            _lineMaterial.SetPass(0);

            DrawBackground();

            GL.Begin(GL.LINES);

            DrawBackgroundLines();
            DrawEntries();           
            
            GL.End();
            GL.PopMatrix();
        }
        private void DrawEntries()
        {
            float elementWidth = _rect.width / HISTORY_BUFFER_LENGTH;

            for (int i = 0; i < HISTORY_BUFFER_LENGTH; i++)
            {
                DataEntry entry = _entries[i];

                float min = i * elementWidth + _rect.xMin;
                float max = (i + 1) * elementWidth + _rect.xMin;

                foreach (DataHeader header in _headers)
                {
                    float height = GetHeight(entry, header);

                    GL.Color(header.Color);
                    DrawLine(min, height, max, height);

                    if(i < HISTORY_BUFFER_LENGTH - 1)
                    {
                        DrawConnectingLine(i, header);
                    }
                }
            }
        }
        private float GetHeight(DataEntry entry, DataHeader header)
        {
            return GetHeight(Mathf.InverseLerp(header.Min, header.Max, header.GetValue(entry)));
        }
        private float GetHeight(float delta)
        {
            return (_rect.y + _rect.height) - _rect.height * delta;
        }
        private void DrawConnectingLine(int i, DataHeader header)
        {
            DataEntry a = _entries[i];
            DataEntry b = _entries[i + 1];
            float x = (i + 1) * _rect.width / HISTORY_BUFFER_LENGTH + _rect.xMin;

            float ay = GetHeight(a, header);
            float by = GetHeight(b, header);

            DrawLine(x, ay, x, by);
        }
        private void DrawBackground()
        {
            GL.Begin(GL.TRIANGLES);

            GL.Color(_backgroundColor);

            GL.Vertex(new Vector2(_rect.xMin, _rect.yMin));
            GL.Vertex(new Vector2(_rect.xMax, _rect.yMin));
            GL.Vertex(new Vector2(_rect.xMax, _rect.yMax));

            GL.Vertex(new Vector2(_rect.xMax, _rect.yMax));
            GL.Vertex(new Vector2(_rect.xMin, _rect.yMax));
            GL.Vertex(new Vector2(_rect.xMin, _rect.yMin));

            GL.End();
        }
        private void DrawBackgroundLines()
        {
            GL.Color(_backgroundLineColor);

            //Outline
            DrawLine(_rect.xMin, _rect.yMin, _rect.xMax, _rect.yMin);
            DrawLine(_rect.xMax, _rect.yMin, _rect.xMax, _rect.yMax);
            DrawLine(_rect.xMin, _rect.yMax, _rect.xMax, _rect.yMax);
            DrawLine(_rect.xMin, _rect.yMin, _rect.xMin, _rect.yMax);

            float height = 0;

            for (int i = 1; i <= BACKGROUND_LINES; i++)
            {
                height = _rect.yMax - (_rect.height / 4) * i;
                DrawLine(_rect.xMin, height, _rect.xMax, height);
            }
        }
        private void DrawLine(float ax, float ay, float bx, float by)
        {
            DrawLine(new Vector2(ax, ay), new Vector2(bx, by));
        }
        private void DrawLine(Vector2 a, Vector2 b)
        {
            GL.Vertex(a);
            GL.Vertex(b);
        }
        private class DataHeader
        {
            public DataHeader(string name, Color color, System.Func<DataEntry, float> getValueCallback, System.Func<DataEntry, bool> mediumSeverityCallback, System.Func<DataEntry, bool> highSeverityCallback, float max, float min = 0)
            {
                Name = name;
                Color = color;
                GetValue = getValueCallback;
                Min = min;
                Max = max;
                IsMediumSeverity = mediumSeverityCallback;
                IsHighSeverity = highSeverityCallback;
            }

            public string Name { get; private set; }
            public Color Color { get; private set; }
            public System.Func<DataEntry, float> GetValue { get; private set; }
            public System.Func<DataEntry, bool> IsMediumSeverity { get; private set; }
            public System.Func<DataEntry, bool> IsHighSeverity { get; private set; }
            public float Min { get; private set; }
            public float Max { get; private set; }            
        }
        private struct DataEntry
        {
            public float Ping;
            public float ClientFramerate;
            public float ServerFramerate;
            public float PacketLoss;

            public static DataEntry Get()
            {
                return new DataEntry()
                {
                    Ping = Client.Peer.Ping,
                    ClientFramerate = Mathf.RoundToInt(PerformanceCapture.FrameRate),
                    ServerFramerate = Client.ServerPerformance.FrameRate,
                    PacketLoss = Mathf.RoundToInt(Client.ServerPerformance.PacketLoss * 100),
                };
            }
        }
    }
}
