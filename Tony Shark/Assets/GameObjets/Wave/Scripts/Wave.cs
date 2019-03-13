using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wave : MonoBehaviour
{
    WaveMovement movementComponent;
    WaveGrowth growthComponent;
    WaveAnimator animatorComponent;
    Spawner spawner;
    

    [SerializeField] WaveSettings settings;

    public WaveMovement MovementComponent { get => movementComponent; }

    // Start is called before the first frame update
    private void Awake()
    {
        animatorComponent = gameObject.AddComponent<WaveAnimator>();
        movementComponent = gameObject.AddComponent<WaveMovement>();
        movementComponent.SetupMovementSettings(settings.MovementSettings);

        growthComponent = gameObject.AddComponent<WaveGrowth>();

        // Locate the  right spawner
        var spawners = FindObjectsOfType<Spawner>();

        foreach(Spawner potentialSpawner in spawners)
        {
            if (potentialSpawner.tag == "WaveSpawner")
            {
                spawner = potentialSpawner;
            }
        }
    }

    // TODO remove once Wave Controller spawns the waves
    void Start()
    {
        movementComponent.LaunchWave();
    }

    // For the Wave Manager
    public void LaunchWave()
    {
        movementComponent.LaunchWave();
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy object once it is stopped
        if (movementComponent.IsStopped)
        {
            // Remove from wave list
            spawner.SpawnedWaves.Remove(gameObject);
            Destroy(gameObject);
        }

        // Start Growing once Max Height is reached
        if (movementComponent.ReachedMaxHeight && !growthComponent.StartedGrowing)
        {
            growthComponent.StartGrowing();
            // TODO Start Wave-Travelling Animation
        }

        if (movementComponent.StartedBringingDown && !growthComponent.StoppedGrowing)
        {
            growthComponent.StopGrowing();
        }
        
    }
}
