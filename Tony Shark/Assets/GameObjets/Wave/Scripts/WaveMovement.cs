using System.Collections;
using UnityEngine;


public class WaveMovement : MonoBehaviour
{
    WaveMovementSettings movementSettings;

    [SerializeField] float sideMovementSpeed = 5.0f; // MOVE TO SPAWNER

    // Time based wave calculation
    float timeTravelled = 0.0f;
    float crushingTime = 3.0f;

    float distanceRaised = 0.0f;
    float distanceLowered = 0.0f;

    // State bools for - TODO consider making it an enum
    bool startedBringingDown = false;
    bool isStopped = false;
    bool reachedMaxHeight = false;

    // Just Getters
    public bool IsStopped { get => isStopped;}
    public bool StartedBringingDown { get => startedBringingDown; }
    public bool ReachedMaxHeight { get => reachedMaxHeight;}

    // To Call from Wave
    public void SetupMovementSettings(WaveMovementSettings settings)
    {
        movementSettings = settings;
    }
    public void LaunchWave()
    {
        if (movementSettings)
        {
            StopCoroutine(MoveForward());
            StartCoroutine(MoveForward());
        }
        else
        {
            // Make sure to call SetupWaveSettings() after creating the WaveMovementComponent
            Debug.LogWarning("Wave Setting has not been assigned.");
        }
        
    }

    IEnumerator MoveForward()
    {
        // Start raising the  wave up
        StopCoroutine(RaiseWaveUp());
        StartCoroutine(RaiseWaveUp());

        // TODO Move Wave sideways
        StopCoroutine(MoveSideways());
        StartCoroutine(MoveSideways());

        // While the wave did not travelled his maximum travel length
        while (timeTravelled < movementSettings.TravelTime)
        {
            // TIME CALCULATION
            timeTravelled += Time.deltaTime;
            print(timeTravelled);

            // Calculate the new position
            Vector3 moveToPosition = -transform.right * movementSettings.TravelSpeed * Time.deltaTime;

            // Move the wave forward
            transform.Translate(moveToPosition);

            // If it's time for the wave to crush down
            if (timeTravelled > movementSettings.TravelTime - crushingTime && !startedBringingDown)
            {
                // Only do this once
                startedBringingDown = true;

                // Bring Wave Down
                StopCoroutine(BringWaveDown());
                StartCoroutine(BringWaveDown());

                // TODO Start Wave-Crushing Animation
            }

            yield return null;
        }

        isStopped = true;
    }

    //IEnumerator MoveForward()
    //{
    //    // Start raising the  wave up
    //    StopCoroutine(RaiseWaveUp());
    //    StartCoroutine(RaiseWaveUp());

    //    // TODO Move Wave sideways
    //    StopCoroutine(MoveSideways());
    //    StartCoroutine(MoveSideways());

    //    // While the wave did not travelled his maximum travel length
    //    while (distanceTravelled < movementSettings.TravelDistance)
    //    {
    //        // TIME CALCULATION
    //        movementSettings.TravelTime += Time.deltaTime;

    //        distanceTravelled += movementSettings.TravelSpeed * Time.deltaTime;

    //        // Calculate the new position
    //        Vector3 moveToPosition = -transform.right * movementSettings.TravelSpeed * Time.deltaTime;

    //        // Move the wave forward
    //        transform.Translate(moveToPosition);

    //        // If it's time for the wave to crush down
    //        if (distanceTravelled > movementSettings.TravelDistance / 2 && !startedBringingDown)
    //        {
    //            // Only do this once
    //            startedBringingDown = true;

    //            // Bring Wave Down
    //            StopCoroutine(BringWaveDown());
    //            StartCoroutine(BringWaveDown());

    //            // TODO Start Wave-Crushing Animation
    //        }

    //        yield return null;
    //    }

    //    isStopped = true;
    //}

    IEnumerator BringWaveDown()
    {
        // The amount the Wave has to be raised
        float distanceToBringDown = movementSettings.RaiseSpeed * transform.localScale.y * Time.deltaTime * crushingTime; // TODO Remake calculation
        
        // Raise the wave out of the sea until it reaches it's maximum height
        while (distanceLowered < (movementSettings.WaveRaiseHeight * transform.localScale.y))
        {
            distanceLowered += distanceToBringDown;

            // Calculate the new position
            Vector3 lowerPosition = -Vector3.up * distanceToBringDown;

            // Move Wave up
            transform.Translate(lowerPosition);

            yield return null;
        }
    }

    IEnumerator RaiseWaveUp()
    {
        // The amount the Wave has to be raised
        float distanceToRaise = movementSettings.RaiseSpeed * Time.deltaTime;

        // Raise the wave out of the sea until it reaches it's maximum height
        while (distanceRaised < movementSettings.WaveRaiseHeight)
        {
            distanceRaised += distanceToRaise;

            // Calculate the new position
            Vector3 raisePosition = Vector3.up * distanceToRaise;

            // Move Wave up
            transform.Translate(raisePosition);

            yield return null;
        }

        reachedMaxHeight = true;
        print("Max height reached");
    }

    IEnumerator MoveSideways()
    {
        while(true)
        {
            // Start moving the wave sideway
            Vector3 sideMovementPosition = transform.forward * sideMovementSpeed * Time.deltaTime; // TODO make this decided by the spawner

            // Move the way sideways
            transform.Translate(sideMovementPosition);

            yield return null;
        }
        
        
    }
}