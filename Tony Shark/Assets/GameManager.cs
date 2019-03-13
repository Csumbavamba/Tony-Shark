using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float maxTimeToTrick = 3.0f;

    SceneLoader sceneLoader;
    PauseMenu pauseMenu;
    // Spawner waveSpawner;
    TrickManager trickManager;

    Spawner[] spawners;
    

    public void LoseGame()
    {
        sceneLoader.LoadLoseScene();
    }

    // Start is called before the first frame update
    void Awake()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        pauseMenu = FindObjectOfType<PauseMenu>();
        // waveSpawner = FindObjectOfType<Spawner>();

        spawners = FindObjectsOfType<Spawner>();

        // Setup Trick Manager
        trickManager = FindObjectOfType<TrickManager>();
        trickManager.MaxTimeToTrick = maxTimeToTrick;
        trickManager.gameObject.SetActive(false);
    }

    private void Start()
    {
        Invoke("StartSpawning", 2f);
    }

    public void StartSpawning()
    {
        foreach (Spawner spawner in spawners)
        {
            spawner.SpawnWaves();
        }
        // waveSpawner.SpawnWaves();
    }

    // Update is called once per frame
    void Update()
    {
        // Pause Menu - only if the trick manager is not active
        if (Input.GetKeyDown(KeyCode.Escape) && !trickManager.gameObject.activeSelf)
        {
            if (PauseMenu.isPaused)
            {
                pauseMenu.Resume();
            }
            else
            {
                pauseMenu.PauseGame();
            }
        }

        if (!PauseMenu.isPaused)
        {
            // TODO - remove after testing
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // If It is active, enable it
                if (trickManager.gameObject.activeSelf)
                {
                    trickManager.gameObject.SetActive(false);
                }
                else
                {
                    trickManager.gameObject.SetActive(true);
                    trickManager.TryToDoTrick();
                }

            }
        }

        

    }
}
