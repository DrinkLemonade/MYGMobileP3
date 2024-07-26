using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    string gameplayScene;
    List<GameLevel> levelList;
    [SerializeField]
    List<LevelSelectButton> levelButtons;

    private void Start()
    {
        levelList = new();
    }

    public void ParseLevelContent(string content)
    {
        JObject levelsFound = JObject.Parse(content);
        for (int i = 1; i < levelsFound.Count+1; i++)
        {
            List<Category> categoryList = new();
            var iString = i.ToString();
            //Debug.Log(levelsFound["1"]);
            Debug.Log(levelsFound[iString]);

            //TODO: Do this in a less stupid way;
            string[][] levelInfo = new string[4][];
            //4 categories
            for (int j = 0; j < 4; j++)
            {
                List<string> words = new();
                for (int k = 0; k < 4; k++)
                {
                    words.Add(levelsFound[iString][j]["words"][k].ToString());
                }
                
                Category cat = new((Category.CategoryType)i, levelsFound[iString][j]["name"].ToString(), words);
                categoryList.Add(cat);
            }
            GameLevel level = new(i, categoryList);
            levelList.Add(level);
        }

        int z = 0;
        foreach (GameLevel level in levelList)
        {
            levelButtons[z].associatedLevel = level;
            levelButtons[z].buttonText.text = level.number.ToString();
            z++;
        }
    }
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
