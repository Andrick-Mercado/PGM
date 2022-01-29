using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGC_Mesh : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Mesh _mesh;
    private Vector3[] _vertices;
    public int xSize;
    public int ySize;

    private void Awake()
    {
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshRenderer = gameObject.AddComponent<MeshRenderer>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        MeshBuilder();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MeshBuilder()
    {
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();

        //_mesh.name = "Procedural Grid";

        _vertices = new Vector3[( xSize + 1 ) * ( ySize + 1 )];

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                _vertices[i] = new Vector3(x, 0, y); // _vertices[i] = new Vector3(x, y);
            }
        }

        int[] triangles = new int[xSize * ySize * 6];

        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }

        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();

    }
}
