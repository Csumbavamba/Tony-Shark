using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[System.Serializable]
public struct ButtonSprites
{
    public Sprite Normal;
    public Sprite Green;
    public Sprite Red;
}

public class ButtonSettings
{
    private KeyCode codeToHit = KeyCode.LeftArrow;
    private ButtonSprites uiSprites;

    public ButtonSettings() { }

    public void SetButtonSettings(KeyCode codeToHit, ButtonSprites uiImage)
    {
        this.codeToHit = codeToHit;
        this.uiSprites = uiImage;
    }

    public ButtonSprites UISprites { get => uiSprites;}
    public KeyCode CodeToHit { get => codeToHit; }
}

public class TrickManager : MonoBehaviour
{
    
    [SerializeField] ButtonSprites[] buttonSprites;
    [SerializeField] GameObject successParticles;
    [SerializeField] GameObject mistakeImage;
    [SerializeField] float mistakeDisplaySpeed = 4.0f;


    [SerializeField] UISounds uiSounds;

    float maxTimeToTrick = 5.0f;
    Slider timeLeftSlider;
    KeyCode[] validKeys;
    TrickButton[] buttons;
    float timeLeftToTrick;
    int currentStep = 0;
    bool readyToSpawn = false;
    Color mistakeColor;
    bool isTrickFailed = false;

    bool wasTrickSuccessful = false;


    public bool IsTrickFailed { get => isTrickFailed; }
    public float MaxTimeToTrick { get => maxTimeToTrick; set => maxTimeToTrick = value; }
    public bool ReadyToSpawn { get => readyToSpawn; set => readyToSpawn = value; }
    public bool WasTrickSuccessful { get => wasTrickSuccessful; }

    // Start is called before the first frame update
    void Awake()
    {
        // buttons = new TrickButton[4];
        buttons = GetComponentsInChildren<TrickButton>();

        timeLeftToTrick = maxTimeToTrick;

        // Add valid key codes
        validKeys = new KeyCode[4];        
        validKeys[0] = KeyCode.LeftArrow;
        validKeys[1] = KeyCode.UpArrow;
        validKeys[2] = KeyCode.DownArrow;
        validKeys[3] = KeyCode.RightArrow;

        // Setup Slider
        timeLeftSlider = GetComponentInChildren<Slider>();
        timeLeftSlider.maxValue = maxTimeToTrick;

        // Setup Particles and Mistake Image
        DisableParticles();
        mistakeColor = mistakeImage.GetComponent<Image>().color;
        mistakeColor.a = 0.0f;
    }

    public void TryToDoTrick()
    {
        ResetManager();

        // Slow down time for epic feeling
        SlowDownTime();

        // Start the Challange
        StopCoroutine(RunTrickChallange());
        StartCoroutine(RunTrickChallange());
    }

    void SlowDownTime()
    {
        // Play the sound if it's not playing
        if (!uiSounds.IsPlayingSound())
        {
            uiSounds.PlaySound(uiSounds.TimeSlowDown, 1.0f);
        }

        StopCoroutine(SlowTimeGradually());
        StartCoroutine(SlowTimeGradually());
    }

    void ResetManager()
    {
        // Reset all buttons
        foreach (TrickButton button in buttons)
        {
            button.SetupButtonSettings(GenerateRandomButtonSettings());
        }

        isTrickFailed = false;
        currentStep = 0;
        timeLeftToTrick = maxTimeToTrick;
    }

    IEnumerator RunTrickChallange()
    {
        // Run until the time is not up and the trick isn't failed
        while (timeLeftToTrick > 0.0f && currentStep < buttons.Length)
        {
            foreach (KeyCode validKey in validKeys)
            {
                if (Input.GetKeyDown(validKey))
                {
                    // Check against the currently selected button
                    if (!buttons[currentStep].IsSuccess(validKey))
                    {
                        isTrickFailed = true;
                    }

                    // Swap to next button
                    currentStep++;
                }
            }

            // Decrease the time that's left
            timeLeftToTrick -= Time.deltaTime / Time.timeScale;
            timeLeftSlider.value = timeLeftToTrick;
            yield return null;
        }

        // Scale the timeScale back to normal
        ScaleTimeBack();

        // Decide on outcome
        if (timeLeftToTrick < 0.0f || isTrickFailed)
        {
            // Run Failed Animation - TODO add from Player
            wasTrickSuccessful = false;
            uiSounds.PlaySound(uiSounds.Failure, 1.0f);
            StopCoroutine(PlayUIDamageOverlay());
            StartCoroutine(PlayUIDamageOverlay());
        }
        else
        {
            // Run success Animation - TODO add from Player
            wasTrickSuccessful = true;
            EnableParticles();
            uiSounds.PlaySound(uiSounds.Success, 1.0f);
            // Add Score
            GameManager.score += 500;
            Invoke("DisableParticles", .5f);
        }
    }

    void ScaleTimeBack()
    {
        // Play the sound if it's not playing
        uiSounds.StopPlayingSound();
        uiSounds.PlaySound(uiSounds.TimeToNormal, 1.0f);

        StopCoroutine(SetTimeBackToNormal());
        StartCoroutine(SetTimeBackToNormal());
    }

    void EnableParticles()
    {
        var emissionModule = successParticles.GetComponent<ParticleSystem>().emission;

        emissionModule.enabled = true;
        

    }

    void DisableParticles()
    {
        var emissionModule = successParticles.GetComponent<ParticleSystem>().emission;

        emissionModule.enabled = false;
    }

    IEnumerator PlayUIDamageOverlay()
    {
        mistakeColor.a = 0.8f;

        while (mistakeColor.a > 0.0f)
        {
            mistakeColor.a -= Time.deltaTime * mistakeDisplaySpeed;
            mistakeImage.GetComponent<Image>().color = mistakeColor;
            yield return null;
        }
    }

    IEnumerator SlowTimeGradually()
    {
        while (Time.timeScale > 0.1f)
        {
            Time.timeScale -= 2f * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator SetTimeBackToNormal()
    {
        while (Time.timeScale < 1f)
        {
            Time.timeScale += 6f * Time.deltaTime;
            yield return null;
        }

        Time.timeScale = 1f;

        // Disable the trick manager
        gameObject.SetActive(false);
    }

    ButtonSettings GenerateRandomButtonSettings()
    {
        ButtonSettings randomButton = new ButtonSettings();

        int generatedDirection = Random.Range(0, 4);


        // Left Arrow
        if (generatedDirection == 0)
        {
            randomButton.SetButtonSettings(KeyCode.LeftArrow, buttonSprites[0]);
        }
        // Up Arrow
        else if (generatedDirection == 1)
        {
            randomButton.SetButtonSettings(KeyCode.UpArrow, buttonSprites[1]);
        }
        // Down Arrow
        else if (generatedDirection == 2)
        {
            randomButton.SetButtonSettings(KeyCode.DownArrow, buttonSprites[2]);
        }
        // Right Arrow
        else
        {
            randomButton.SetButtonSettings(KeyCode.RightArrow, buttonSprites[3]);
        }

        return randomButton;
    }
}
