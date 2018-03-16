using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class LineOfSightRenderer : MonoBehaviour {

    public static LineOfSightRenderer Instance { get; private set; }
    public static Texture2D Texture { get; private set; }

    private static readonly Color COLOR_ACTIVE = new Color(0, 0, 0, 0);
    private static readonly Color COLOR_PASSIVE = new Color(7f / 255f, 9f / 255f, 12f / 255, 0.8f);
    private static readonly Color COLOR_DISABLED = new Color(7f / 255f, 9f / 255f, 12f / 255, 1);

    private static Vector2Int _offset;
    private static int _width;
    private static int _height;

    private Material _material;

    private void Awake()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.enabled = true;

        _material = renderer.material;
    }
    private void Start()
    {
        Instance = this;

        _width = Mathf.RoundToInt(MapGenerationSettings.TileMap.x);
        _height = Mathf.RoundToInt(MapGenerationSettings.TileMap.y);
        _offset = new Vector2Int(Mathf.FloorToInt((float)_width / 2f), Mathf.FloorToInt((float)_height / 2f));

        gameObject.transform.localScale = MapGenerationSettings.TileMap;

        CreateTexture();
    }
    public static void Render(IEnumerable<Vector2Int> toUpdate)
    {
        foreach (Vector2Int pos in toUpdate)
        {
            Texture.SetPixel(pos.x - _offset.x, pos.y - _offset.y, GetColor(pos));
        }
        
        Texture.Apply();
    }
    private static Color GetColor(Vector2Int position)
    {
        switch (LineOfSightManager.GetState(position))
        {
            case LineOfSightState.Active:
                return COLOR_ACTIVE;
            case LineOfSightState.Passive:
                return COLOR_PASSIVE;
            case LineOfSightState.Disabled:
                return COLOR_DISABLED;
            default:
                throw new System.NotImplementedException();
        }
    }
    private void CreateTexture()
    {
        Texture = new Texture2D(Mathf.RoundToInt(MapGenerationSettings.TileMap.x), Mathf.RoundToInt(MapGenerationSettings.TileMap.y), TextureFormat.ARGB32, false, false);

        Color[] pixels = new Color[_width * _height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = COLOR_DISABLED;
        }

        Texture.SetPixels(pixels);
        Texture.Apply();

        _material.mainTexture = Texture;
    }
}
