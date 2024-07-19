using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameSession
{
    //TODO: Currently receives time from GameManager
    public float secondsElapsed;
    public int livesLeft;
    HashSet<Category> categoriesFound;
    //TODO: Is this stupid or will this let me avoid duplicate guesses? I dunno if adding identical classes/structs to a HashSet counts as unique.
    //It's kinda stupid in that it hardcodes the amount of words per guess/category. But that's premature optimization, isn't it?
    HashSet<HashSet<string>> guesses; 
    const int wordsPerCategory = 4;

    public enum CategoryType
    {
        Green, Yellow, Blue, Purple
    }

    public struct Category
    {
        public CategoryType myType;
        public string name;

        public Category(CategoryType _myType, string _name)
        {
            myType = _myType;
            name = _name;
        }
    }

    public Dictionary<string, Category> Words;
    public GameSession()
    {
        livesLeft = GameManager.i.LivesInASession;

        Words = new();
        categoriesFound = new();
        guesses = new();
        AddWords(CategoryType.Green, "Capitales", "Paris", "Londres", "Helsinki", "Pékin");
        AddWords(CategoryType.Yellow, "Phases de la lune", "Croissante", "Nouvelle", "Pleine", "Décroissante");
        AddWords(CategoryType.Blue, "Patisseries", "Religieuse", "Eclair", "Tulipe", "Choux");
        AddWords(CategoryType.Purple, "Légumes-racines", "Navet", "Raifort", "Topinambour", "Ginseng");

        //Local function
        //TODO: Where do these Category structs live in memory? Should they be held by GameSession? Who needs to know the categories?
        void AddWords(CategoryType categoryType, string categoryName, string word1, string word2, string word3, string word4)
        {
            var cat = new Category(categoryType, categoryName);
            Words.Add(word1, cat);
            Words.Add(word2, cat);
            Words.Add(word3, cat);
            Words.Add(word4, cat);
        }
    }

    public List<string> GetWords()
    {
        return new List<string>(Words.Keys);
    }


    public void EvaluateGuess()
    {
        var guess = new HashSet<string>
        {
        UIManager.i.WordButtonsSelected[0].associatedWord,
        UIManager.i.WordButtonsSelected[1].associatedWord,
        UIManager.i.WordButtonsSelected[2].associatedWord,
        UIManager.i.WordButtonsSelected[3].associatedWord
        };

        foreach (var g in guesses)
        {
            if (g.SetEquals(guess))
            {
                //We already made a guess like this
                UIManager.i.infoBannerAlreadyGuessed.Show();
                return;
            }
        }
        //We haven't made a guess like this yet
        guesses.Add(guess);


        //could probably do something clean with LINQ
        int i = 0;
        List<Category> cats = new(); //hehehe
        foreach (var btn in UIManager.i.WordButtonsSelected)
        {
            //Find the word in the words list and add its category to the "cats" temporary list.
            cats.Add(Words.Where(x => x.Key == btn.associatedWord).Select(x => x.Value).FirstOrDefault());
        }

        Debug.Log($"evaluating: {cats}");

        //We now have a list of four categories. If they all match, the player has correctly selected 4 words of the same category.
        //TODO: Do this with a less stupid linq query
        //First, find the most commonly repeated category
        var maxRepeatedCat = cats.GroupBy(x => x)
                          .OrderByDescending(x => x.Count())
                          .First().Key;
        //Next, how often does it repeat exactly?
        int matches = cats.Where(x => x.Equals(maxRepeatedCat)).Count();
        
        switch (matches)
        {
            case 4:
                //cor probably shouldn't be local like that
                //var cor =
                    GuessIsCorrect(maxRepeatedCat);
                //GameManager.i.StartCoroutine(cor);
                break;
            case 3:
                Debug.LogWarning("One away...");
                UIManager.i.infoBannerOneAway.Show();
                GuessIsIncorrect();
                break;
            default:
                GuessIsIncorrect();
                break;
        }
    }

    void GuessIsIncorrect()
    {
        Debug.Log("Failure");
        UIManager.i.ShakeSelectedButtons();
        DecreaseLives();
        if (livesLeft == 0) Defeat();

        //TEST
        //UIManager.i.SwapAnimationTest();
    }

    void GuessIsCorrect(Category categoryFound)
    {
        string words = "";
        int i = 0;
        foreach (var item in UIManager.i.WordButtonsSelected)
        {
            words += item.associatedWord;
            if (i < 3) words += ", ";
            i++;
        }
        //Shift the index of words in the child list, which instantly rearranges them on the GridLayout.
        var indexShiftThese = new List<WordToggle>(UIManager.i.WordButtonsSelected);
        int j = 0;
        foreach (var item in indexShiftThese)
        {            
            item.anchor.transform.SetSiblingIndex((categoriesFound.Count * wordsPerCategory) + j);
            item.SetFound();
            j++;
        }

        UIManager.i.GridAnimation(categoriesFound.Count, categoryFound.name, categoryFound.myType, words);
        categoriesFound.Add(categoryFound);
        Debug.Log($"Success! Categories found: {categoriesFound.Count}");

        UIManager.i.WordButtonsSelected.Clear();

        if (categoriesFound.Count == 4) Victory(secondsElapsed, GameManager.i.LivesInASession - livesLeft);
        else UIManager.i.EnableButtons();
    }

    void DecreaseLives()
    {
        livesLeft--;
        //Should probably be its own function, UpdateLifeCount?
        UIManager.i.livesTracker.UpdateText(livesLeft);
        if (livesLeft == 0) Defeat();
    }

    void Victory(float time, int mistakes)
    {
        UIManager.i.Victory(time, mistakes);
    }
    void Defeat()
    {
        UIManager.i.Defeat();
    }
}
