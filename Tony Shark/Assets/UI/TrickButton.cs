using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TrickButton : MonoBehaviour
{
    ButtonSettings settings;
    Image uiImage;


    private void Awake()
    {
        uiImage = GetComponent<Image>();
        settings = new ButtonSettings();
    }

    // Get button to hit and the sprite for it
    public void SetupButtonSettings(ButtonSettings generatedSettings)
    {
        settings = generatedSettings;

        // Set the UI image
        uiImage.sprite = settings.UISprites.Normal;
    }

    // Check if the right button was hit
    public bool IsSuccess(KeyCode buttonHit)
    {
        if (buttonHit == settings.CodeToHit)
        {
            // Set the image green
            uiImage.sprite = settings.UISprites.Green;
            return true;
        }

        // Set the image red
        uiImage.sprite = settings.UISprites.Red;
        return false;
    }
}
