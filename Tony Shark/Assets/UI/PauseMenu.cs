using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject slider;
    // Start is called before the first frame update


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        slider.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        print("Resume");
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        slider.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void MainMenu()
    {
        FindObjectOfType<SceneLoader>().LoadMainMenu();
    }
}
