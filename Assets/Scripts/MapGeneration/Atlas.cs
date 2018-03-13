using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Atlas.asset", menuName = "Game/Atlas", order = 69)]
public class Atlas : ScriptableObject {
    
    public Texture2D Texture
    {
        get
        {
            if (_texture == null)
                CreateTexture();

            return _texture;
        }
    }
    private Texture2D _texture;

    private Dictionary<TileType, Rect> _tileRectangles;

    [SerializeField]
    private List<TileType> _tiles;
    [SerializeField]
    private int _padding = 1;
    [SerializeField]
    private int _maxSize = 4096;

    public Rect GetRect(TileType tileType)
    {
        if (_tileRectangles == null)
            CreateTexture();

        return _tileRectangles[tileType];
    }
    private void CreateTexture()
    {
        _tileRectangles = new Dictionary<TileType, Rect>();

        _texture = new Texture2D(_maxSize, _maxSize);
        Rect[] uvs = _texture.PackTextures(_tiles.Select(x => x.Texture).ToArray(), _padding, _maxSize);

        for (int i = 0; i < uvs.Length; i++)
        {
            _tileRectangles.Add(_tiles[i], uvs[i]);
        }
    }
}
