using UnityEngine;

public class UISounds : MonoBehaviour
{
    [SerializeField] AudioClip timeSlowDown;
    [SerializeField] AudioClip timeToNormal;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip failure;

    AudioSource audioSource;

    public AudioClip TimeSlowDown { get => timeSlowDown; set => timeSlowDown = value; }
    public AudioClip TimeToNormal { get => timeToNormal; set => timeToNormal = value; }
    public AudioClip Success { get => success; set => success = value; }
    public AudioClip Failure { get => failure; set => failure = value; }

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clipToPlay, float volumeScale)
    {
        audioSource.PlayOneShot(clipToPlay, volumeScale);
    }

    public bool IsPlayingSound()
    {
        if (audioSource.isPlaying)
        {
            return true;
        }
        return false;
    }

    public void StopPlayingSound()
    {
        audioSource.Stop();
    }

}
