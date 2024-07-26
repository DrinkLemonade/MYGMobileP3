using System;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [NonSerialized]
    public static SoundManager i;
    [NonSerialized]
    public AudioSource sfxSource;
    [SerializeField]
    public AudioSource musicSource;
    [SerializeField]
    public AudioClip JingleVictory, JingleDefeat, MainMenuMusic, MainGameMusic;
    [SerializeField]
    public AudioClip GuessCorrect, GuessIncorrect;


    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        if (i != null)
            Destroy(i.gameObject);

        i = this;
        DontDestroyOnLoad(this);

        //Make sure this doesn't happen before we've loaded the music volume!
        SoundManager.i.StartCoroutine(SoundManager.i.CrossFade(SoundManager.i.MainMenuMusic));
        /*
                  musicSource.clip = MainMenuMusic;
        musicSource.loop = true;
        musicSource.Play();*/
    }

    private void Start()
    {
        sfxSource = GetComponent<AudioSource>();
        //StartCoroutine(FadeMixerGroup.StartFade(GameSettings.i.audioMixer, "MusicVolume", 0.01f, targetVolume: 0f));
        //StartCoroutine(FadeMixerGroup.StartFade(GameSettings.i.audioMixer, "MusicVolume", 1f, targetVolume: GameSettings.i.mixerValues["MusicVolume"]));
        if (this == null) Debug.Log("wat");
        if (MainMenuMusic == null) Debug.Log("wut");
        //LoopMusic(MainMenuMusic);
    }

    // Update is called once per frame
    public void PlaySound(AudioClip sound)
    {
        if (sound == null) Debug.LogError("Audio clip is null!");
        else if (sfxSource == null) Debug.LogError($"Playing {sound.name} but source is null!");
        else sfxSource.PlayOneShot(sound);
    }

    public void LoopMusic(AudioClip sound)
    {
        if (sound == null) Debug.LogError("Audio clip is null!");
        if (musicSource == null) Debug.LogError($"Playing {sound.name} but source is null!");
        musicSource.clip = sound;
        musicSource.loop = true;
        musicSource.Play();

    }

    public void SetMenuMusic()
    {
        Debug.Log("SOUND: Menu music");
        StartCoroutine(CrossFade(MainMenuMusic));
    }

    public IEnumerator CrossFade(AudioClip toSound, float delayBetween = 0f, bool stopLooping = false)
    {
        if (GameSettings.i is null) Debug.LogError("settings null");
        if (GameSettings.i.audioMixer is null) Debug.LogError("mixer null");

        //_ = GameSettings.i.musicMixer.GetFloat("MusicVolume", out float toValue);
        //float value = GameSettings.i.SettingsHolder["MusicVolume"].TryP
        
        //TODO fix this
        float value = 1f;
        float toValueNormalized = Mathf.Lerp(-80f, 20f, value);
        
            //(GameSettings.i.mixerValuesDecibel["MusicVolume"]); //Not sure why MusicVolume only goes from -80f to 0f and not up to 20f?

        Debug.Log($"SOUND: Music volume in settings is {value}, normalized to {toValueNormalized}. Crossfading to {toSound.name}");

        yield return StartCoroutine(FadeMixerGroup.StartFade(GameSettings.i.audioMixer, "MusicVolume", 1f, 0f));
        Debug.Log("SOUND: Fade out done");
        yield return new WaitForSeconds(delayBetween);
        LoopMusic(toSound);
        if (stopLooping) musicSource.loop = false;
        yield return StartCoroutine(FadeMixerGroup.StartFade(GameSettings.i.audioMixer, "MusicVolume", 1f, targetVolume: toValueNormalized));
        Debug.Log("SOUND: Fade in done");
        yield break;


    }
}
