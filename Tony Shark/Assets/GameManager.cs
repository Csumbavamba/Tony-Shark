using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] float maxTimeToTrick = 3.0f;

    SceneLoader sceneLoader;
    PauseMenu pauseMenu;
    // Spawner waveSpawner;
    TrickManager trickManager;

    Spawner[] spawners;
    
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
        // waveSpawner = FindObjectOfType<Spawner>();

        spawners = FindObjectsOfType<Spawner>();

        // Setup Trick Manager
        trickManager = FindObjectOfType<TrickManager>();
        trickManager.MaxTimeToTrick = maxTimeToTrick;
        trickManager.gameObject.SetActive(false);
    }

    private void Start()
    {
        avatarScript = avatar.GetComponent<AvatarControl>();
        Invoke("StartSpawning", 2f);
    }

    public void StartSpawning()
    {
        foreach (Spawner spawner in spawners)
        {
            spawner.SpawnWaves();
        }
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
            // Add Game Logic here
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
    public void StartTrick()
    {
        // Make sure to not start it while it's going
        if (!trickManager.gameObject.activeSelf)
        {
            trickManager.gameObject.SetActive(true);
            trickManager.TryToDoTrick();
        }
        else
        {
            Debug.LogError("Trick is ongoing, you don't want to start it again...");
        }
        
    }

}
