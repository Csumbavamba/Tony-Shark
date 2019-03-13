using System.Collections;
using UnityEngine;


public class WaveMovement : MonoBehaviour
{
    WaveMovementSettings movementSettings;
    //WaveAnimator animationScript;

    // Time based wave calculation
    float timeTravelled = 0.0f;
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

    private void Awake()
    {
       // animationScript = GetComponent<WaveAnimator>();
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
       

        // While the wave did not travelled his maximum travel length
        while (timeTravelled < movementSettings.TravelTime * 2)
        {
            // TIME CALCULATION
            timeTravelled += Time.deltaTime;

            // Calculate the new position
            Vector3 moveToPosition = transform.right * movementSettings.TravelSpeed * Time.deltaTime;

            // Move the wave forward
            transform.Translate(moveToPosition);

            // If it's time for the wave to crush down
            if (timeTravelled > movementSettings.TravelTime && !startedBringingDown)
            {
                // Only do this once
                startedBringingDown = true;

                // Bring Wave Down
                StopCoroutine(BringWaveDown());
                StartCoroutine(BringWaveDown());

                // TODO Start Wave-Crushing Animation
               // animationScript.PlayCrushAnimation();
            }

            yield return null;
        }
    }

    IEnumerator BringWaveDown()
    {
        // The amount the Wave has to be raised
        float distanceToBringDown = movementSettings.RaiseSpeed * transform.localScale.y * Time.deltaTime * 4f; 
        
        // Raise the wave out of the sea until it reaches it's maximum height
        while (distanceLowered < (movementSettings.WaveRaiseHeight * 1.5f))
        {
            //print(movementSettings.WaveRaiseHeight * transform.localScale.y);
            distanceLowered += distanceToBringDown;

            // Calculate the new position
            Vector3 lowerPosition = -transform.up * distanceToBringDown;

            // Move Wave up
            transform.Translate(lowerPosition);

            yield return null;
        }

        isStopped = true;
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
        //animationScript.PlayTravelAnimation();

        print("Max height reached");
    }

}