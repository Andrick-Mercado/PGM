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

    //material for mesh
    public Material materialMesh;
    //material for sphere
    public Material materialSphere;

    //grid size
    public int xSize;
    public int zSize;

    //wave variables
    public float WaveHeight;
    public float WaveSpeed;
    private Vector3[] BaseHeights;//holds original heights

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
        _meshFilter.mesh = _mesh = new Mesh();

        _vertices = new Vector3[( xSize + 1 ) * (zSize + 1 )];

        //makes vertices based on xSize and zSize
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                _vertices[i] = new Vector3(x, 0, z); 
            }
        }

        int[] triangles = new int[xSize * zSize * 6];

        //creates triangles based on vertices above
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
        sphere.transform.position = new Vector3(xSize/2, WaveHeight*2, zSize/2);
        sphere.AddComponent<Rigidbody>();
        sphere.GetComponent<MeshRenderer>().material = materialSphere;
    }

    void MeshWaves()
    {
        if (BaseHeights == null)//base case
            BaseHeights = _mesh.vertices;

        Vector3[] vertices = new Vector3[BaseHeights.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = BaseHeights[i];
            vertex.y += Mathf.Sin(Time.time * WaveSpeed + BaseHeights[i].x + BaseHeights[i].y + BaseHeights[i].z) * WaveHeight;
            vertices[i] = vertex;
        }

        //update vertices
        _mesh.vertices = vertices;
        _mesh.RecalculateNormals();

        //update mesh collider
        _meshCollider.sharedMesh = _mesh;
    }
}
