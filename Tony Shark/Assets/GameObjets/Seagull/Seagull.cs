using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull : MonoBehaviour
{
    [SerializeField] float travelSpeed = 10.0f;
    [SerializeField] float maxTravelTime = 40f;

    [SerializeField] Vector3 travelDirection;

    Spawner spawner;


    float timeSpentTravelling = 0.0f;

    // Locate the  right spawner
    private void Awake()
    {
        var spawners = FindObjectsOfType<Spawner>();

        foreach (Spawner potentialSpawner in spawners)
        {
            if (potentialSpawner.tag == "SeagullSpawner")
            {
                spawner = potentialSpawner;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // TODO - LOUIS, remove this if you are setting the travel direction in the EDITOR
        travelDirection = transform.forward;

        StopCoroutine(TravelForward());
        StartCoroutine(TravelForward());
    }

    IEnumerator TravelForward()
    {
        while(timeSpentTravelling < maxTravelTime)
        {
            timeSpentTravelling += Time.deltaTime;

            Vector3 movePosition = travelDirection * travelSpeed * Time.deltaTime;

            transform.Translate(movePosition);

            yield return null;
        }

        // Destroy object after travelling enough
        spawner.SpawnedWaves.Remove(gameObject);
        Destroy(gameObject);
    }
}
