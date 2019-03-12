using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    SceneLoader sceneLoader;
    PauseMenu pauseMenu;

    public void LoseGame()
    {
        sceneLoader.LoadLoseScene();
    }

    // Start is called before the first frame update
    void Awake()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        // Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape))
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

        // Losing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoseGame();
        }
    }
}
