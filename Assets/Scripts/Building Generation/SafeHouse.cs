using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeHouse : Building {

	public SafeHouse() : base()
    {
        StaticPosition = true;
        
        _width = Random.Range(MIN, MAX);
        _height = Random.Range(MIN, MAX);

        Position = new Vector2(-_width / 2, -_height / 2);

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector2 position = new Vector2(x, y);
                
                if(IsEdge(position))
                {
                    Add(position, TileType.Names.WoodWall);
                }
                else
                {
                    Add(position, TileType.Names.WoodFloor);
                }
            }
        }
    }

    private const int MIN = 10;
    private const int MAX = 30;

    private float _width;
    private float _height;    

    private bool IsEdge(Vector2 pos)
    {
        return pos.x == 0 || pos.y == 0 || pos.x == _width - 1 || pos.y == _height - 1;
    }
}
