using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class LineOfSightRenderer : MonoBehaviour {

    public static LineOfSightRenderer Instance { get; private set; }
    public static Texture2D Texture { get; private set; }

    private Color[] _pixels;

    /// <summary>
    /// How many times a second should we update the texture
    /// </summary>
    private float REFRESH_RATE = 10;

    private float _timeSinceLastUpdate;
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

        _pixels = new Color[Mathf.RoundToInt(GameSettings.TileMap.x * GameSettings.TileMap.y)];
        
        for (int i = 0; i < _pixels.Length; i++)
        {
            _pixels[i] = Color.black;
        }
        
        CreateTexture();
        UpdateTexture();
    }
    private void Update()
    {
        _timeSinceLastUpdate += Time.unscaledDeltaTime;

        if(_timeSinceLastUpdate > 1 / REFRESH_RATE)
        {
            _timeSinceLastUpdate = 0;

            UpdateTexture();
        }
    }
    private void UpdateTexture()
    {
        Texture.SetPixels(_pixels);
        Texture.Apply();
    }
    private void CreateTexture()
    {
        Texture = new Texture2D(Mathf.RoundToInt(GameSettings.TileMap.x), Mathf.RoundToInt(GameSettings.TileMap.y), TextureFormat.ARGB32, false, false);
        _material.mainTexture = Texture;
    }
}
