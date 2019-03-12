using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Awake()
    {
        // Check if this object exists
        int numberOfSceneLoaders = FindObjectsOfType<SceneLoader>().Length;

        // Destroy object if there is more than one
        if (numberOfSceneLoaders > 1)
        {
            Destroy(gameObject);
        }
        // Otherwise keep it
        else
        {
            DontDestroyOnLoad(this);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void LoadControls()
    {
        SceneManager.LoadScene("Controls");
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game"); // TODO Create Scene
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
