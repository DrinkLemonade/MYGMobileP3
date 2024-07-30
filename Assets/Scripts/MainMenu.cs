using DG.Tweening;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    string gameplayScene;
    List<GameLevel> levelList;
    [SerializeField]
    List<LevelSelectButton> levelButtons;
    [SerializeField]
    GameObject containerMainMenu, containerLevelSelection, containerSettings;
    [SerializeField]
    float offScreenMenuAnimationPositionY, offScreenMenuAnimationTime;
    float previousMenuPositionY;
    [SerializeField]
    Ease offScreenMenuAnimationEase;
    Sequence menuAppear;


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
            levelButtons[z].toggle.interactable = true;
            z++;
        }

        //TODO: Right now we start with all level buttons disabled, then enable those within the level count.
        //In the future, unavailable levels should be clickable, but will display "this level will be available on [date]" instead

    }

    //TODO: There has to be a smarter way to do this
    private void ChangeMenus(GameObject from, GameObject to)
    { 
    
        var rT = from.GetComponent<RectTransform>();
        rT.DOAnchorPosY(offScreenMenuAnimationPositionY, offScreenMenuAnimationTime).SetEase(offScreenMenuAnimationEase);

        var rT2 = to.GetComponent<RectTransform>();
        rT2.DOAnchorPosY(0f, offScreenMenuAnimationTime).SetEase(offScreenMenuAnimationEase);
    public void ChangeMenuMainToSettings()
    {
        ChangeMenus(containerMainMenu, containerSettings);
    }
    public void ChangeMenuSettingsToMain()
    {
        ChangeMenus(containerSettings, containerMainMenu);
    }
    public void ChangeMenuMainToLevels()
    {
        ChangeMenus(containerMainMenu, containerLevelSelection);
    }
    public void ChangeMenuLevelsToMain()
    {
        ChangeMenus(containerLevelSelection, containerMainMenu);
    }



    public void PlayGame()
    {
        SceneManager.LoadScene(gameplayScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
