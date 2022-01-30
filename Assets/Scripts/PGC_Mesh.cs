using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGC_Mesh : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Mesh _mesh;
    private MeshCollider _meshCollider;
    private Vector3[] _vertices;

    public Material materialMesh;

    //grid size
    public int xSize;
    public int zSize;

    //wave variables
    public float WaveHeight;
    public float WaveSpeed;
    private Vector3[] baseHeight;

    private void Awake()
    {
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshRenderer = gameObject.AddComponent<MeshRenderer>(); 

    }

    // Start is called before the first frame update
    void Start()
    {
        MeshBuilder();
        CreateSpherePrimitive();
    }

    // Update is called once per frame
    void Update()
    {
        //create wave effects
        MeshWaves();
    }

    void MeshBuilder()
    {
        //GetComponent<MeshFilter>().mesh = _mesh = new Mesh();  //uncomment if doesnt work
        _meshFilter.mesh = _mesh = new Mesh();


        _vertices = new Vector3[( xSize + 1 ) * (zSize + 1 )];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                _vertices[i] = new Vector3(x, 0, z); // _vertices[i] = new Vector3(x, y);
            }
        }

        int[] triangles = new int[xSize * zSize * 6];

        for (int ti = 0, vi = 0, z = 0; z < zSize; z++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }

        // draw triangles
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();

        // add renderer material
        _meshRenderer.material = materialMesh;

        // add collider to mesh
        _meshCollider = gameObject.AddComponent<MeshCollider>();
    }

    void CreateSpherePrimitive()
    {
        //create a sphere and add it to the middle of the mesh
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = new Vector3(xSize/2, WaveHeight*5, zSize/2);
        sphere.AddComponent<Rigidbody>();
    }

    void MeshWaves()
    {
        if (baseHeight == null)
            baseHeight = _mesh.vertices;

        Vector3[] vertices = new Vector3[baseHeight.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseHeight[i];
            vertex.y += Mathf.Sin(Time.time * WaveSpeed + baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * WaveHeight;
            vertices[i] = vertex;
        }
        _mesh.vertices = vertices;
        _mesh.RecalculateNormals();

        //update mesh collider
        _meshCollider.sharedMesh = _mesh;
    }
}
