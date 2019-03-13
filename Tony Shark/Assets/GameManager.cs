using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    SceneLoader sceneLoader;
    PauseMenu pauseMenu;
    Spawner waveSpawner;

    public GameObject avatar;
    AvatarControl avatarScript;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI turboText;
    public TextMeshProUGUI scoreText;

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
    }

    private void Start()
    {
        avatarScript = avatar.GetComponent<AvatarControl>();
        Invoke("SpawnWaves", 2f); 
    }

    void SpawnWaves()
    {
        waveSpawner.SpawnWaves();
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

        // Losing - TODO make it based on falling
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //LoseGame();
        }

        SetUIText();
    }

    void SetUIText()
    {
        string setText = "Speed: ";
        setText += ((int)avatarScript.velocity.z).ToString();
        speedText.SetText(setText);

        setText = "Turbo Power: ";
        setText += ((int)avatarScript.turboPower).ToString();
        turboText.SetText(setText);
    }
}
