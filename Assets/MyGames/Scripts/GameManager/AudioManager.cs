using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("DangSon/AudioManager")]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance
    {
        get => instance;
    }
    private static AudioManager instance;
    /// <summary>
    [Header("Audio Source")]
    public AudioSource musicSource;
    public AudioSource sfxSourcePlayer;
    public AudioSource sfxSourceEnemySolider;
    public AudioSource sfxSourceEnemyAttack;
    [Header("Audio Background")]
    public AudioClip backgroundMusic;
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);   
            return;
        }
        instance = this;
       // DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayMusic(backgroundMusic);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void PlaySfxPlayer(AudioClip clip)
    {
        sfxSourcePlayer.PlayOneShot(clip);
    }
    public void PlaySfxEnemySolider(AudioClip clip)
    {
        sfxSourceEnemySolider.PlayOneShot(clip);
    }
    public void PlaySfxEnemyAttack(AudioClip clip)
    {
        sfxSourceEnemyAttack.PlayOneShot(clip);
    }
}
