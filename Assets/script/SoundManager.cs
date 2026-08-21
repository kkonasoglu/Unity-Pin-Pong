using UnityEditor.Timeline.Actions;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips - SFX")]
    public AudioClip paddleHitClip;
    public AudioClip wallBounceClip;
    public AudioClip speedBostClip;
    public AudioClip scoreClip;
    public AudioClip winClip;

    [Header("Audio Clips - Music")]
    public AudioClip bgmMusic;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayBGM();
    }

    public void PlayBGM()
    {
        if(bgmSource !=null && bgmMusic != null)
        {
            bgmSource.clip = bgmMusic;
            bgmSource.loop = true;
            bgmSource.Play();
        }

    }


    public void PlayPaddleHit()
    {
        PlaySFX(paddleHitClip);
    }
    public void PlayWallBounce()
    {
        PlaySFX(wallBounceClip);
    }
    public void PlaySpeedBoost()
    {
        PlaySFX(speedBostClip);
    }
    public void PlayScore()
    {
        PlaySFX(scoreClip);
    }
    public void PlayWin()
    {
        if(bgmSource != null)
        {
            bgmSource.Stop();
        }

        if(sfxSource != null)
        {
            sfxSource.Stop();
        }

        
        PlaySFX(winClip);
    }
    public void PlaySFX(AudioClip clip)
    {
        if(sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
