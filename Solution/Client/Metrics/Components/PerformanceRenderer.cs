using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Networking;
using TMPro;
using System.Text;

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
#pragma warning restore

        private const int HISTORY_BUFFER_LENGTH = 20;
        private const int BACKGROUND_LINES = 3;
        
        private List<DataEntry> _entries;
        private float _timeSinceLastUpdate = float.MaxValue;
        private Rect _rect;
        
        private readonly List<DataHeader> _headers = new List<DataHeader>()
        {
            new DataHeader("Ping", new Color32(255, 111, 255, 255), x => x.Ping, 200, 500, 1000),
        };

        private void Awake()
        {
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
                _timeSinceLastUpdate = 0;

                AddEntry(DataEntry.Get());
                SetText();
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
                builder.Append($"{header.Name}: {header.GetValue(entry)}");

                builder.Append("    ");

                builder.Append("<color=#");
                builder.Append(ColorUtility.ToHtmlStringRGB(GetColor(header, entry)));
                builder.Append(">");
                builder.Append(GetMiscText(header));
                builder.Append("</color>");


                builder.AppendLine();
            }

            _textElement.text = builder.ToString();
        }
        private string GetMiscText(DataHeader header)
        {
            return $"{header.Min}-{header.Max} [{header.MediumSeverity}|{header.HighSeverity}]";
        }
        private Color GetColor(DataHeader header, DataEntry entry)
        {
            float value = header.GetValue(entry);

            if(value >= header.HighSeverity)
            {
                return _highSeverityTextColor;
            }
            else if(value >= header.MediumSeverity)
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
            public DataHeader(string name, Color color, System.Func<DataEntry, float> getValueCallback, float mediumSeverity, float highSeverity, float max, float min = 0)
            {
                Name = name;
                Color = color;
                GetValue = getValueCallback;
                Min = min;
                Max = max;
                MediumSeverity = mediumSeverity;
                HighSeverity = highSeverity;
            }

            public string Name { get; private set; }
            public Color Color { get; private set; }
            public System.Func<DataEntry, float> GetValue { get; private set; }
            public float Min { get; private set; }
            public float Max { get; private set; }
            public float MediumSeverity { get; private set; }
            public float HighSeverity { get; private set; }
        }
        private struct DataEntry
        {
            public float Ping;

            public static DataEntry Get()
            {
                return new DataEntry()
                {
                    Ping = Client.Peer.Ping,
                };
            }
        }
    }
}
