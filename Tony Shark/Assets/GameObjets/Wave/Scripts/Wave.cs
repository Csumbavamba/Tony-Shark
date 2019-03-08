using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wave : MonoBehaviour
{
    WaveMovement movementComponent;
    WaveGrowth growthComponent;
    Spawner spawner;

    [SerializeField] WaveSettings settings;

    public WaveMovement MovementComponent { get => movementComponent; }

    // Start is called before the first frame update
    private void Awake()
    {
        movementComponent = gameObject.AddComponent<WaveMovement>();
        movementComponent.SetupMovementSettings(settings.MovementSettings);

        growthComponent = gameObject.AddComponent<WaveGrowth>();

        spawner = FindObjectOfType<Spawner>();
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
        }

        
    }
}
