using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using System.Globalization;

public class GameSettings : MonoBehaviour
{
    public static GameSettings i = null;

    //Visuals
    public float animationSpeedMultiplier = 1f;
    public float delaySecondsBetweenCombatEffects = 0.25f;
    public float delaySecondsBetweenAIAbilities = 1f;
    public float delaySecondsBetweenEndTurnEffects = 0.5f;
    [SerializeField]
    private float canDivideAnimationSpeedUpTo = 3f, canMultiplyAnimationSpeedByUpTo = 3f;

    //Audio
    [SerializeField]
    public AudioMixer audioMixer;

    //Changing settings changes this holder. When leaving the settings menu, the stored changes are written to PlayerPrefs
    //We don't read these values directly from source, because e.g. what if the AudioMixers are decreasing in volume due to a fadeout?
    public Dictionary<string, string> SettingsHolder;

    /*void Awake()
    {
        i = this;
        Debug.Log("settings: i exist! (awake)");
    }*/

    private void Awake()
    {
        if (i != null)
            Destroy(i.gameObject);

        i = this;
        DontDestroyOnLoad(this);
        //We fill the holder when executing the Update functions below.
        SettingsHolder = new();

        UpdateMainAudioMixer("MusicVolume", Mathf.Lerp(0.0001f, 1f, PlayerPrefs.GetInt("MusicVolume", 100)/100f));
        UpdateMainAudioMixer("AmbientVolume", Mathf.Lerp(0.0001f, 1f, PlayerPrefs.GetInt("AmbientVolume", 100)/100f));
        UpdateMainAudioMixer("SFXVolume", Mathf.Lerp(0.0001f, 1f, PlayerPrefs.GetInt("SFXVolume", 50)/100f));

        UpdateAnimationSpeed(PlayerPrefs.GetInt("AnimationSpeed", 50) / 100f);
        UpdateFullscreen(PlayerPrefs.GetInt("Fullscreen", 1) == 1);
    }

    public void SaveSettings()
    {
        Debug.Log($"Saving settings. Music volume in holder is {SettingsHolder["MusicVolume"]}. To string, it's {int.Parse(SettingsHolder["MusicVolume"], CultureInfo.InvariantCulture.NumberFormat)}");

        PlayerPrefs.SetString("Language", SettingsHolder["Language"]);

        PlayerPrefs.SetInt("MusicVolume", int.Parse(SettingsHolder["MusicVolume"], CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetInt("AmbientVolume", int.Parse(SettingsHolder["AmbientVolume"], CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetInt("SFXVolume", int.Parse(SettingsHolder["SFXVolume"], CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetInt("AnimationSpeed", int.Parse(SettingsHolder["AnimationSpeed"], CultureInfo.InvariantCulture.NumberFormat));
    }

    void TempSettingsAddOrChange(string key, string newValue)
    {
        Debug.Log($"TempSettings: {key}: value now {newValue}");
        if (SettingsHolder.TryGetValue(key, out _))
            SettingsHolder[key] = newValue;
        else SettingsHolder.Add(key, newValue);
    }

    //Things like "we set volume to 100 in the settings", not "changing the volume for a fade effect"
    public void UpdateMainAudioMixer(string parameter, float sliderValue)
    {
        //1-100
        int intValue = (int)Mathf.Lerp(0, 100, sliderValue);

        float dBValue = Mathf.Log10(sliderValue) * 20;
        TempSettingsAddOrChange(parameter, intValue.ToString());
        Debug.Log($"Changing parameter {parameter}. Slider value is {sliderValue}. int version is {intValue}. Converted to dB, it's {dBValue}");

        audioMixer.SetFloat(parameter, dBValue);
    }

    public void UpdateAnimationSpeed(float sliderValue)
    {
        int intValue = (int)Mathf.Lerp(0f, 100f, sliderValue);

        float value = 0;
        //There's probably a smart mathy way of doing this but it's late and I'm not smart.
        if (sliderValue < 0.5f) value = Mathf.Lerp((1 / canDivideAnimationSpeedUpTo), 1, sliderValue * 2);
        else if (sliderValue > 0.5f) value = Mathf.Lerp(1, (1 * canMultiplyAnimationSpeedByUpTo), (sliderValue - 0.5f) * 2);
        else value = 1;
        //if (sliderValue < 0.1) value = 0.1f; //Don't allow divide by 0

        Debug.Log($"Animations: slider is {sliderValue}, int value is {intValue}, actual multiplier is {value}");


        TempSettingsAddOrChange("AnimationSpeed", intValue.ToString());
        animationSpeedMultiplier = value;
    }
    public void UpdateFullscreen(bool value)
    {
        TempSettingsAddOrChange("Fullscreen", value ? "1" : "0");
        Screen.fullScreen = value;
    }
}
