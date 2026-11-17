using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance; ///{ get; private set; }

    private AudioSource audioSource;
    public float volume = 1.0f;
    private AudioClip menuMusic;
    private AudioClip levelMusic;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
            LoadAudioClips();
        }
    }

    void Start()
    {
        PlayMenuMusic();
    }

    public void PlayMenuMusic()
    {
        audioSource.clip = menuMusic;
        Debug.Log("Playing menu music: " + menuMusic.name);
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
        
    }

    public void PlayLevelMusic()
    {
        if (audioSource.clip != levelMusic)
        {
            audioSource.clip = levelMusic;
            audioSource.volume = volume;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void LoadAudioClips()
    {
        menuMusic = Resources.Load<AudioClip>("Music/Menu");
        levelMusic = Resources.Load<AudioClip>("Music/Game");
    }

    public void UpdateVolume(float newVolume)
    {
        volume = newVolume;
        audioSource.volume = volume;
    }
}
