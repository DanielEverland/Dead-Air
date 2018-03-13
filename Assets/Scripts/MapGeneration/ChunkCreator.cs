using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChunkCreator {
    
    private static Default DefaultData
    {
        get
        {
            if (_default == null)
                _default = new Default();

            return _default;
        }
    }
    private static Default _default;

    private static Vector3[] DefaultVertices { get { return DefaultData.vertices.Copy(); } }
    private static int[] DefaultTriangles { get { return DefaultData.triangles.Copy(); } }
    private static Vector3[] DefaultNormals { get { return DefaultData.normals.Copy(); } }
        
	public static void Create(Chunk chunk)
    {
        GameObject gameObject = GetTemplate(chunk.Position);
        chunk.GameObject = gameObject;

        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();

        filter.mesh = CreateMesh(chunk);
        renderer.material = Atlas.Instance.Material;

        Update(chunk);
    }
    public static void Update(Chunk chunk)
    {
        chunk.GameObject.GetComponent<MeshFilter>().mesh.uv = Atlas.Instance.GetUVs(chunk);
    }
    private static Mesh CreateMesh(Chunk chunk)
    {
        Mesh mesh = new Mesh();
        
        mesh.vertices = DefaultVertices;
        mesh.triangles = DefaultTriangles;
        mesh.normals = DefaultNormals;
        
        return mesh;
    }
    public static GameObject GetTemplate(Vector2 chunkPos)
    {
        GameObject gameObject = GameObject.Instantiate(StaticObjects.GetObject<GameObject>("ChunkTemplate"));

        gameObject.name = string.Format("ChunkPos: {0}", chunkPos);
        gameObject.transform.position = Utility.ChunkPosToWorldPos(chunkPos);

        return gameObject;
    }

    private class Default
    {
        public Default()
        {
            vertices = new Vector3[Chunk.CHUNK_SIZE * Chunk.CHUNK_SIZE * 4];
            triangles = new int[Chunk.CHUNK_SIZE * Chunk.CHUNK_SIZE * 6];
            normals = new Vector3[vertices.Length];
            
            CreateVertices();
            CreateTriangles();
            CreateNormals();
        }
        private void CreateNormals()
        {
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = Vector3.back;
            }
        }
        private void CreateTriangles()
        {
            for (int i = 0; i < Chunk.CHUNK_SIZE * Chunk.CHUNK_SIZE; i++)
            {
                int index = i * 6;
                int verticeCount = i * 4;

                triangles[index + 0] = verticeCount + 0;
                triangles[index + 1] = verticeCount + 1;
                triangles[index + 2] = verticeCount + 2;

                triangles[index + 3] = verticeCount + 2;
                triangles[index + 4] = verticeCount + 3;
                triangles[index + 5] = verticeCount + 0;
            }
        }
        private void CreateVertices()
        {
            List<Vector3> toReturn = new List<Vector3>();

            for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
            {
                for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
                {
                    toReturn.AddRange(
                        new Vector3[4]
                        {
                            new Vector3(x, y),
                            new Vector3(x, y + 1),
                            new Vector3(x + 1, y + 1),
                            new Vector3(x + 1, y),
                        });
                }
            }

            vertices = toReturn.ToArray();
        }

        public Vector3[] vertices;
        public Vector3[] normals;
        public int[] triangles;
    }
}
