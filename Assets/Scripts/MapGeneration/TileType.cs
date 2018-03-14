using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile.asset", menuName = "Game/Tile", order = 69)]
public class TileType : ScriptableObject {

    public static Dictionary<byte, TileType> AllTiles
    {
        get
        {
            if(_allTiles == null)
            {
                _allTiles = new Dictionary<byte, TileType>();

                foreach (TileType tile in Resources.LoadAll<TileType>("Tiles"))
                {
                    if(_allTiles.ContainsKey(tile.ID))
                    {
                        throw new System.ArgumentException(string.Format("A tile with the ID {0} already exists", tile.ID));
                    }

                    if(tile.ID == 0)
                    {
                        throw new System.NullReferenceException(tile.name);
                    }

                    _allTiles.Add(tile.ID, tile);
                }
            }

            return _allTiles;
        }
    }
    private static Dictionary<byte, TileType> _allTiles;
    
    public byte ID { get { return (byte)_name; } }
    public bool Impassable { get { return _collision; } }
    public bool Natural { get { return !_isBuildingBlock; } }
    public Sprite Sprite
    {
        get
        {
            return _sprite;
        }
    }
    public int Width
    {
        get
        {
            return (int)_sprite.textureRect.width;
        }
    }
    public int Height
    {
        get
        {
            return (int)_sprite.textureRect.height;
        }
    }
    public Vector2 TextureOffset
    {
        get
        {
            return _sprite.textureRect.position;
        }
    }
    public Texture2D Texture
    {
        get
        {
            if (_texture == null)
                Initialize();

            return _texture;
        }
    }
    public Material Material
    {
        get
        {
            if (_material == null)
                Initialize();

            return _material;
        }
    }
    private Material _material;

    private Texture2D _texture;

    [SerializeField]
    private Name _name;
    [SerializeField]
    private Sprite _sprite;
    [SerializeField]
    private bool _collision;
    [SerializeField]
    private bool _isBuildingBlock;

    public void Initialize()
    {
        CreateTexture();
        CreateMaterial();
    }
    public void CreateTexture()
    {
        _texture = new Texture2D(Width, Height, TextureFormat.ARGB32, false, true);
        
        Color[] pixels = _sprite.texture.GetPixels((int)TextureOffset.x, (int)TextureOffset.y, Width, Height);

        _texture.SetPixels(pixels);
        _texture.Apply();
    }
    public void CreateMaterial()
    {
        _material = new Material(StaticObjects.GetObject<Material>("DefaultMaterial"));
        _material.mainTexture = Texture;
    }
    [System.Serializable]
    public enum Name : byte
    {
        None = 0,

        Grass = 1,
        WoodWall = 2,
        WoodFloor = 3,
    }
}
