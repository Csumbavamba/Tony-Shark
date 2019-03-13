using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    SceneLoader sceneLoader;
    PauseMenu pauseMenu;
    Spawner waveSpawner;
    TrickManager trickManager;

    public void LoseGame()
    {
        sceneLoader.LoadLoseScene();
    }

    // Start is called before the first frame update
    void Awake()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        pauseMenu = FindObjectOfType<PauseMenu>();
        waveSpawner = FindObjectOfType<Spawner>();
        trickManager = FindObjectOfType<TrickManager>();

        trickManager.gameObject.SetActive(false);
    }

    private void Start()
    {
        Invoke("StartSpawning", 2f);
    }

    public void StartSpawning()
    {
        waveSpawner.SpawnWaves();
    }

    // Update is called once per frame
    void Update()
    {
        // Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape) && !trickManager.enabled)
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

        // Losing - TODO make it based on falling
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Space pressed."); // TODO FIX
            if (trickManager.enabled)
            {
                trickManager.gameObject.SetActive(false);
            }
            else
            {
                trickManager.gameObject.SetActive(true);
            }
            
        }

    }
}
