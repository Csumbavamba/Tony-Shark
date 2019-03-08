using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] float sideMovementSpeed = 5.0f;
    [SerializeField] GameObject wavePrefab;
    [SerializeField] Vector3 size;
    [SerializeField] float minimumDistanceToSpawn = 50f;

    List<GameObject> spawnedWaves;

    public float SideMovementSpeed { get => sideMovementSpeed; set => sideMovementSpeed = value; }
    public List<GameObject> SpawnedWaves { get => spawnedWaves; }

    private void Awake()
    {
        spawnedWaves = new List<GameObject>();
    }


    public void SpawnWaves()
    {
        StopCoroutine(SpawnWavesContinuously());
        StartCoroutine(SpawnWavesContinuously());
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        StopCoroutine(SpawnWavesContinuously());
    //        StartCoroutine(SpawnWavesContinuously());
    //    }
    //}

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomSpawnPosition;

        // Generate a random spawn position
        randomSpawnPosition = new Vector3(
        transform.position.x + UnityEngine.Random.Range(-size.x / 2f, size.x / 2f),
        transform.position.y + UnityEngine.Random.Range(-size.y / 2f, size.y / 2f),
        transform.position.z + UnityEngine.Random.Range(-size.z / 2f, size.z / 2f));
        
        return randomSpawnPosition;
    }

    private bool IsWaveTooClose(Vector3 spawnPosition)
    {
        // Spawn if there are no waves spawned
        if (spawnedWaves.Count < 1)
        {
            return false;
        }

        // Check for X distance
        foreach (GameObject wave in spawnedWaves)
        {
            // If x distance is too close, return true
            if (Vector3.Distance(wave.transform.position, spawnPosition) < minimumDistanceToSpawn)
            {
                print("Wave too close");
                return true;
            }
        }

        // Otherwise return false
        print("Wave in good distance");
        return false;
    }

    IEnumerator SpawnWavesContinuously()
    {
        while (true)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            // If it's too close keep looking
            if (IsWaveTooClose(spawnPosition))
            {
                yield return null;
            }
            else
            {
                // Instantiate the Wave and add to the List of Waves
                GameObject spawnedWave = Instantiate(wavePrefab, spawnPosition, Quaternion.identity);
                spawnedWaves.Add(spawnedWave);
            } 
        } 
    }

    // For Debug
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, size);
    }
}
