using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public ButtonSettings(KeyCode codeToHit, ButtonSprites uiImage)
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
    [SerializeField] float maxTimeToTrick = 5.0f;

    KeyCode[] validKeys;
    TrickButton[] buttons;
    float timeLeftToTrick;
    int currentStep = 0;
    bool isTrickFailed = false;

    public bool IsTrickFailed { get => isTrickFailed; }

    // Start is called before the first frame update
    void Start()
    {
        buttons = GetComponentsInChildren<TrickButton>();
        timeLeftToTrick = maxTimeToTrick;

        // Add valid key codes
        validKeys = new KeyCode[4];        
        validKeys[0] = KeyCode.LeftArrow;
        validKeys[1] = KeyCode.UpArrow;
        validKeys[2] = KeyCode.DownArrow;
        validKeys[3] = KeyCode.RightArrow;

        // Chose a random Sprite and set up the button
        foreach (TrickButton button in buttons)
        {
            button.SetupButtonSettings(GenerateRandomButtonSettings());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryToDoTrick();
        }
    }

    public void TryToDoTrick()
    {
        StopCoroutine(RunTrickChallange());
        StartCoroutine(RunTrickChallange());
    }

    public void ResetManager()
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
            timeLeftToTrick -= Time.deltaTime;
            yield return null;
        }

        // Decide on outcome
        if (timeLeftToTrick < 0.0f || isTrickFailed)
        {
            // Run Failed Animation - TODO add from Player
            print("Trick failed.");
        }
        else
        {
            // Run success Animation - TODO add from Player
            print("Trick SUCCESS!!!");
        }

        // Reset the button settings
        Invoke("ResetManager", 2f);
    }

    ButtonSettings GenerateRandomButtonSettings()
    {
        ButtonSettings randomButton;

        int generatedDirection = Random.Range(0, 4);


        // Left Arrow
        if (generatedDirection == 0)
        {
            randomButton = new ButtonSettings(KeyCode.LeftArrow, buttonSprites[0]);
        }
        // Up Arrow
        else if (generatedDirection == 1)
        {
            randomButton = new ButtonSettings(KeyCode.UpArrow, buttonSprites[1]);
        }
        // Down Arrow
        else if (generatedDirection == 2)
        {
            randomButton = new ButtonSettings(KeyCode.DownArrow, buttonSprites[2]);
        }
        // Right Arrow
        else
        {
            randomButton = new ButtonSettings(KeyCode.RightArrow, buttonSprites[3]);
        }

        return randomButton;
    }
}
