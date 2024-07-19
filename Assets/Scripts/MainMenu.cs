using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    string gameplayScene;
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadScene(gameplayScene);
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
    }
}
