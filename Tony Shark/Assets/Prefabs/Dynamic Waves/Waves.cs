using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For Playing with Wave Lengths for the arrtists
[Serializable]
public struct Octave
{
    public Vector2 speed;
    public Vector2 scale;
    public float height;
    public bool alternate;
}

// TODO make this pluggable to a Cube Mesh
public class Waves : MonoBehaviour
{
    [SerializeField] int dimensions = 10;
    [SerializeField] Octave[] octaves;
    [SerializeField] float uvScale;

    protected MeshFilter meshFilter;
    protected Mesh mesh;

    // Start is called before the first frame update
    
    void Start()
    {
        // Create mesh at runtime
        mesh = new Mesh();
        mesh.name = gameObject.name;

        // Generate the mesh vertices and triangles
        mesh.vertices = GenerateVerticies();
        mesh.triangles = GenerateTriangles();
        mesh.uv = GenerateUV();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        // Add the mesh filter component to the object
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    // For Collision Physics
    public float GetHeightAt(Vector3 currentPosition)
    {
        // Adjust to current Scale
        var scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
        var localPosition = Vector3.Scale((currentPosition - transform.position), scale);

        // Get the edge points
        var p1 = new Vector3(Mathf.Floor(localPosition.x), 0, Mathf.Floor(localPosition.z));
        var p2 = new Vector3(Mathf.Floor(localPosition.x), 0, Mathf.Ceil(localPosition.z));
        var p3 = new Vector3(Mathf.Ceil(localPosition.x), 0, Mathf.Floor(localPosition.z));
        var p4 = new Vector3(Mathf.Ceil(localPosition.x), 0, Mathf.Ceil(localPosition.z));

        // Clamp if the position is outside of the plane
        p1.x = Mathf.Clamp(p1.x, 0, dimensions);
        p1.z = Mathf.Clamp(p1.z, 0, dimensions);
        p2.x = Mathf.Clamp(p2.x, 0, dimensions);
        p2.z = Mathf.Clamp(p2.z, 0, dimensions);
        p3.x = Mathf.Clamp(p3.x, 0, dimensions);
        p3.z = Mathf.Clamp(p3.z, 0, dimensions);
        p4.x = Mathf.Clamp(p4.x, 0, dimensions);
        p4.z = Mathf.Clamp(p4.z, 0, dimensions);

        // the maximum distance possible
        var maxDistance = Mathf.Max(
            Vector3.Distance(p1, localPosition),
            Vector3.Distance(p2, localPosition),
            Vector3.Distance(p3, localPosition),
            Vector3.Distance(p4, localPosition) + Mathf.Epsilon);

        // Actual distance
        var distance = (maxDistance - Vector3.Distance(p1, localPosition))
                        + (maxDistance - Vector3.Distance(p2, localPosition))
                        + (maxDistance - Vector3.Distance(p3, localPosition))
                        + (maxDistance - Vector3.Distance(p4, localPosition) + Mathf.Epsilon);

        // Weight the height
        var height = mesh.vertices[Index((int)p1.x, (int)p1.z)].y * (maxDistance - Vector3.Distance(p1, localPosition))
                        + mesh.vertices[Index((int)p2.x, (int)p2.z)].y * (maxDistance - Vector3.Distance(p2, localPosition))
                        + mesh.vertices[Index((int)p3.x, (int)p3.z)].y * (maxDistance - Vector3.Distance(p3, localPosition))
                        + mesh.vertices[Index((int)p4.x, (int)p4.z)].y * (maxDistance - Vector3.Distance(p4, localPosition));


        return height * transform.lossyScale.y / distance;
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

        GeneratePerlinNoise(verticies);

        mesh.vertices = verticies;
        mesh.RecalculateNormals();
    }

    private void GeneratePerlinNoise(Vector3[] verticies)
    {
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
    }
}
