using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] float sideMovementSpeed = 5.0f;
    [SerializeField] GameObject wavePrefab;
    [SerializeField] Vector3 size;

    public float SideMovementSpeed { get => sideMovementSpeed; set => sideMovementSpeed = value; }

    

    // Create a Rectangular Area around the spawning point (has to be long to look like it was long)
    // Visualize the Spawning Area - Gizmos
    // Choose a random point from the Spawning Area
    // Spawn a wave

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition = new Vector3(
            transform.position.x + Random.Range(-size.x / 2f, size.x / 2f),
            transform.position.y + Random.Range(-size.y / 2f, size.y / 2f),
            transform.position.z + Random.Range(-size.z / 2f, size.z / 2f));

        return spawnPosition;
    }

    void SpawnWave()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // Instantiate(wavePrefab, spawnPosition);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, size);
    }
}
