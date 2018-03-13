using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile.asset", menuName = "Game/Tile", order = 69)]
public class TileType : ScriptableObject {

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
            return (int)_sprite.textureRect.x;
        }
    }
    public int Height
    {
        get
        {
            return (int)_sprite.textureRect.y;
        }
    }
    public Vector2 TextureOffset
    {
        get
        {
            return _sprite.textureRectOffset;
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
    private Sprite _sprite;

    public void CreateTexture()
    {
        _texture = new Texture2D(Width, Height);

        Color[] pixels = _sprite.texture.GetPixels((int)TextureOffset.x, (int)TextureOffset.y, Width, Height);

        _texture.SetPixels(pixels);
        _texture.Apply();
    }
}
