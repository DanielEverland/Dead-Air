using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile.asset", menuName = "Game/Tile", order = 69)]
public class TileType : ScriptableObject {

    public Names Name { get { return _name; } }
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
                CreateTexture();

            return _texture;
        }
    }

    private Texture2D _texture;

    [SerializeField]
    private Names _name;
    [SerializeField]
    private Sprite _sprite;    

    public void CreateTexture()
    {
        _texture = new Texture2D(Width, Height, TextureFormat.ARGB32, false, true);
        
        Color[] pixels = _sprite.texture.GetPixels((int)TextureOffset.x, (int)TextureOffset.y, Width, Height);

        _texture.SetPixels(pixels);
        _texture.Apply();
    }
    [System.Serializable]
    public enum Names
    {
        None = 0,

        Grass,
        WoodWall,
        WoodFloor,
    }
}
