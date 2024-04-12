using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public float meshWidth = 10f;
    public float meshLength = 10f;
    public float scale = 0.5f;
    public float waveSpeed;
    public float waveHeight;
    private MeshFilter _mF;
    private Mesh _mesh, _mesh2;
    
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        _mF = GetComponent<MeshFilter>();
        _mesh = new Mesh();
        _mF.mesh = _mesh;
        
          Vector3[] vertices = new Vector3[4]
                {
                    new Vector3(0, 0, 0),
                    new Vector3(meshWidth, 0, 0),
                    new Vector3(0, 0, meshLength),
                    new Vector3(meshWidth,0, meshLength)
                } ;
          
         int[] triangles = new int[6];
                triangles[0] = 1;
                triangles[1] = 0;
                triangles[2] = 2;
                
                triangles[3] = 2;
                triangles[4] = 3;
                triangles[5] = 1;

        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh2 = _mesh;
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateNoise();
    }

    void CalculateNoise()
    {
        Vector3[] verts = _mesh2.vertices; //Array of mesh's vertices
        
        for (int i = 0; i < verts.Length; i++) //Iterates through each vertex, multiplying by scale value and adding wave speed
        {
            float pX = verts[i].x * scale + (Time.time * waveSpeed);
            float pZ = verts[i].z * scale + (Time.time * waveSpeed);
            verts[i].y = Mathf.PerlinNoise(pX, pZ) * waveHeight; //Updates the y-value using Perlin noise
        }

        _mesh2.vertices = verts;
        _mesh2.RecalculateNormals();
        _mesh2.RecalculateBounds();
    }
}
