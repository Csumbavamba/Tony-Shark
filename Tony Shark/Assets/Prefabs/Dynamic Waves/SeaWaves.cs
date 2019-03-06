using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// TODO remove if not needed
public class SeaWaves : MonoBehaviour
{
    [SerializeField] int dimensions = 10;
    [SerializeField] Octave[] octaves;
    [SerializeField] float uvScale;

    protected Mesh mesh;

    // Start is called before the first frame update

    void Start()
    {
        // Create mesh at runtime
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.name = gameObject.name;

        // Generate the mesh vertices and triangles
        mesh.vertices = GenerateVerticies();
        mesh.triangles = GenerateTriangles();
        mesh.uv = GenerateUV();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }



    private int[] GenerateTriangles()
    {
        // Generate a Quad by 2 triangles
        var triangles = new int[mesh.vertices.Length * 6];

        // Generate triangles for every tile
        for (int x = 0; x < dimensions; x++)
        {
            for (int z = 0; z < dimensions; z++)
            {
                triangles[Index(x, z) * 6 + 0] = Index(x, z);           // Top left
                triangles[Index(x, z) * 6 + 1] = Index(x + 1, z + 1);   // Bottom right
                triangles[Index(x, z) * 6 + 2] = Index(x + 1, z);       // Top right
                triangles[Index(x, z) * 6 + 3] = Index(x, z);           // Top left (left triangle)
                triangles[Index(x, z) * 6 + 4] = Index(x, z + 1);       // Bottom left
                triangles[Index(x, z) * 6 + 5] = Index(x + 1, z + 1);   // Bottom right (left triangle)
            }
        }

        return triangles;
    }

    private Vector3[] GenerateVerticies()
    {
        var verticies = new Vector3[(dimensions + 1) * (dimensions + 1)];

        // distribute the vertices equally
        for (int x = 0; x <= dimensions; x++)
        {
            for (int z = 0; z <= dimensions; z++)
            {
                verticies[Index(x, z)] = new Vector3(x, 0, z);
            }
        }

        return verticies;
    }

    private int Index(int x, int z)
    {
        return x * (dimensions + 1) + z;
    }

    private Vector2[] GenerateUV()
    {
        var uvs = new Vector2[mesh.vertices.Length];

        for (int x = 0; x <= dimensions; x++)
        {
            for (int z = 0; z <= dimensions; z++)
            {
                var uvVectors = new Vector2((x / uvScale) % 2, (z / uvScale) % 2); // it is either 1 or 0

                uvs[Index(x, z)] = new Vector2(
                    uvVectors.x <= 1 ? uvVectors.x : 2 - uvVectors.x, // If x is 0 its gonna be 1 otherwise its gonna be 0
                    uvVectors.y <= 1 ? uvVectors.y : 2 - uvVectors.y);
            }
        }

        return uvs;
    }

    // Update is called once per frame
    void Update()
    {
        var verticies = mesh.vertices;

        for (int x = 0; x <= dimensions; x++)
        {
            for (int z = 0; z <= dimensions; z++)
            {
                // Create height for the verticies
                var y = 0f;

                for (int o = 0; o < octaves.Length; o++)
                {
                    if (octaves[o].alternate)
                    {
                        var perlinNoise = Mathf.PerlinNoise(
                            (x * octaves[o].scale.x) / dimensions,
                            (z * octaves[o].scale.y) / dimensions)
                            * Mathf.PI * 2f;
                        // Increase and decrease the height of the octave
                        y += Mathf.Cos(perlinNoise + octaves[o].speed.magnitude * Time.time) * octaves[o].height;
                    }
                    else
                    {
                        var perlinNoise = Mathf.PerlinNoise(
                            (x * octaves[o].scale.x + Time.time * octaves[o].speed.x) / dimensions,
                            (z * octaves[o].scale.y + Time.time * octaves[o].speed.y) / dimensions)
                            - 0.5f; // Perlin Noise gives a number between 0 and 1 --> get value between -0.5 and 0.5

                        y += perlinNoise * octaves[o].height;
                    }
                }

                verticies[Index(x, z)] = new Vector3(x, y, z);
            }
        }

        mesh.vertices = verticies;
        mesh.RecalculateNormals();
    }

    [Serializable]
    public struct Octave
    {
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }
}
