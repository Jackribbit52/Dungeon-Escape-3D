using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    private AudioSource audioSource;

    [SerializeField] private AudioClip normalMusic;
    [SerializeField] private AudioClip intenseMusic;
    [SerializeField] private AudioClip endingMusic;

    private bool isIntenseMusicPlaying = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = normalMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void SwitchToIntenseMusic()
    {
        if (isIntenseMusicPlaying) return;

        isIntenseMusicPlaying = true;
        audioSource.Stop();
        audioSource.clip = intenseMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayEndingMusic()
    {
        audioSource.Stop();
        audioSource.clip = endingMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}

