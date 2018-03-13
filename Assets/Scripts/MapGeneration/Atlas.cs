using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Atlas.asset", menuName = "Game/Atlas", order = 69)]
public class Atlas : ScriptableObject {
    
    public static Atlas Instance
    {
        get
        {
            return StaticObjects.GetObject<Atlas>();
        }
    }

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

    public Material Material
    {
        get
        {
            if (_material == null)
                CreateMaterial();

            return _material;
        }
    }
    private Material _material;

    private Dictionary<TileType.Names, Rect> _tileRectangles;

    [SerializeField]
    private List<TileType> _tiles;
    [SerializeField]
    private int _padding = 1;
    [SerializeField]
    private int _maxSize = 4096;

    public Vector2[] GetUVs(Chunk chunk)
    {
        Vector2[] toReturn = new Vector2[Chunk.CHUNK_SIZE * Chunk.CHUNK_SIZE * 4];

        for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
        {
            for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
            {
                Vector2[] tileUvs = GetUVs(chunk.Tiles[x, y]);

                for (int i = 0; i < 4; i++)
                {
                    toReturn[y * Chunk.CHUNK_SIZE + x + i] = tileUvs[i];
                }
            }
        }

        return toReturn;
    }
    public Vector2[] GetUVs(TileType.Names name)
    {
        Rect rect = GetRect(name);

        return new Vector2[4]
        {
            new Vector2(rect.x, rect.y),
            new Vector2(rect.x + rect.width, rect.y),
            new Vector2(rect.x + rect.width, rect.y + rect.height),
            new Vector2(rect.x, rect.y + rect.height),
        };
    }
    public TileType GetTile(TileType.Names name)
    {
        return _tiles.Find(x => x.Name == name);
    }
    public Rect GetRect(TileType.Names name)
    {
        if (_tileRectangles == null)
            CreateTexture();

        return _tileRectangles[name];
    }
    private void CreateTexture()
    {
        _tileRectangles = new Dictionary<TileType.Names, Rect>();

        _texture = new Texture2D(_maxSize, _maxSize, TextureFormat.ARGB32, false, true);
        Rect[] uvs = _texture.PackTextures(_tiles.Select(x => x.Texture).ToArray(), _padding, _maxSize);

        for (int i = 0; i < uvs.Length; i++)
        {
            if (_tiles[i].Name == TileType.Names.None || _tileRectangles.ContainsKey(_tiles[i].Name))
                throw new System.Exception();
            
            _tileRectangles.Add(_tiles[i].Name, uvs[i]);
        }

        _texture.wrapMode = TextureWrapMode.Clamp;
        _texture.filterMode = FilterMode.Point;
        _texture.Apply();
    }
    private void CreateMaterial()
    {
        _material = new Material(StaticObjects.GetObject<Material>("DefaultMaterial"));
        _material.mainTexture = Instance.Texture;
    }
}
