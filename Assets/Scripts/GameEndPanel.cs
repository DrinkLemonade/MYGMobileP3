using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndPanel : MonoBehaviour
{
    [SerializeField]
    string victoryText, defeatText;
    [SerializeField]
    TextMeshProUGUI gameEndTMP;
    [SerializeField]
    string mainMenuScene;

    public void ShowFailure()
    {
        gameObject.SetActive(true);
        gameEndTMP.text = defeatText;

    }
    public void ShowSuccess(float seconds, int mistakes)
    {
        gameObject.SetActive(true);

        seconds %= 60;
        float minutes = (float)System.Math.Floor(seconds / 60);
        float hours = (float)System.Math.Floor(seconds / 360);

        string h = hours.ToString().PadLeft(2, '0');
        string m = minutes.ToString().PadLeft(2, '0');
        string s = seconds.ToString().PadLeft(2, '0');

        if (s.Length > 2) s = s.Substring(0, 2);

        string time;
        if (hours < 0) time = $"{m}:{s}";
        else time = $"{h}:{m}:{s}";

        string plural = mistakes == 1 ? "" : "s";
        gameEndTMP.text = victoryText + $"\nTrouvé en : {time} - {mistakes} erreur{plural}";
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
