using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChunkCreator {

    private static int _submeshID;
    private static int _verticeCount;
    private static MeshFilter _currentMeshFilter;
    private static Mesh _currentMesh;
    private static Vector3[] _vertices = new Vector3[(Chunk.CHUNK_SIZE * Chunk.CHUNK_SIZE) * 4];
    private static List<List<int>> _triangles = new List<List<int>>();
    private static List<Material> _materials = new List<Material>();
    private static Vector2[] _uvs = new Vector2[(Chunk.CHUNK_SIZE * Chunk.CHUNK_SIZE) * 4];
    private static Vector3[] Normals
    {
        get
        {
            if (_normals == null)
                CreateNormals();

            return _normals;
        }
    }
    private static Vector3[] _normals;

    public static bool Initialize()
    {
        CreateNormals();

        return true;
    }
    public static void RenderChunk(Chunk chunk)
    {
        _currentMeshFilter = chunk.GameObject.GetComponent<MeshFilter>();
        _currentMesh = new Mesh();
        _verticeCount = 0;
        _materials.Clear();
        _triangles.Clear();

        for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
        {
            for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
            {
                AddTile(x, y, chunk.Tiles[x, y]);
            }
        }

        _currentMesh.vertices = _vertices;
        _currentMesh.subMeshCount = _triangles.Count;

        for (int i = 0; i < _triangles.Count; i++)
        {
            _currentMesh.SetTriangles(_triangles[i], i);
        }

        _currentMesh.normals = Normals;
        _currentMesh.uv = _uvs;

        _currentMeshFilter.mesh = _currentMesh;
        _currentMeshFilter.GetComponent<MeshRenderer>().materials = _materials.ToArray();
        _currentMeshFilter.gameObject.transform.position = new Vector3(Chunk.CHUNK_SIZE * chunk.Position.x, Chunk.CHUNK_SIZE * chunk.Position.y);
    }
    private static void AddTile(int x, int y, byte tileID)
    {
        if (_materials.Contains(TileType.AllTiles[tileID].Material))
        {
            _submeshID = _materials.IndexOf(TileType.AllTiles[tileID].Material);
        }
        else
        {
            _submeshID = _materials.Count;
            _materials.Add(TileType.AllTiles[tileID].Material);
            _triangles.Add(new List<int>());
        }

        _vertices[_verticeCount + 0] = new Vector3(1 + x, 1 + y);
        _vertices[_verticeCount + 1] = new Vector3(1 + x, 0 + y);
        _vertices[_verticeCount + 2] = new Vector3(0 + x, 1 + y);
        _vertices[_verticeCount + 3] = new Vector3(0 + x, 0 + y);

        _uvs[_verticeCount + 0] = new Vector3(1, 1);
        _uvs[_verticeCount + 1] = new Vector3(1, 0);
        _uvs[_verticeCount + 2] = new Vector3(0, 1);
        _uvs[_verticeCount + 3] = new Vector3(0, 0);

        _triangles[_submeshID].AddRange(new List<int>(6)
        {
            3 + _verticeCount, 0 + _verticeCount, 1 + _verticeCount,
            2 + _verticeCount, 0 + _verticeCount, 3 + _verticeCount,
        });

        _verticeCount += 4;
    }
    private static void CreateNormals()
    {
        _normals = new Vector3[(Chunk.CHUNK_SIZE * Chunk.CHUNK_SIZE) * 4];

        for (int i = 0; i < _normals.Length; i++)
        {
            _normals[i] = Vector3.back;
        }
    }
}
