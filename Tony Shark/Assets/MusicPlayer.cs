using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private void Awake()
    {
        int numberOfMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;

        // If there are more than one music players in the screen
        if (numberOfMusicPlayers > 1)
        {
            // Then destroy ourselves
            Destroy(gameObject);
        }
        else
        {
            // Otherwise, don't destroy on load
            DontDestroyOnLoad(this);
        }



    }
}
