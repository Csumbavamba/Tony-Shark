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
    TrickManager trickManager;

    Spawner[] spawners;

    public static float score = 0.0f;
    
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
        avatarScript = FindObjectOfType<AvatarControl>();

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

    void StartSpawning()
    {
        foreach(Spawner spawner in spawners)
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

        UpdateScore();
        SetUIText();
    }

    void SetUIText()
    {
        string setText = "Speed, ";
        setText += ((int)avatarScript.velocity.z).ToString();
        speedText.SetText(setText);

        setText = "Turbo, ";
        setText += ((int)avatarScript.turboPower).ToString();
        turboText.SetText(setText);

        // For score calculation
        string textToSet = "Score, " + score;
        scoreText.SetText(textToSet);
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

    void UpdateScore()
    {
        score += avatarScript.velocity.magnitude * Time.deltaTime;
        score = Mathf.Round(score * 10f) / 10f;
    }

}
