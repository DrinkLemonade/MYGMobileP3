using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameSession
{
    enum CategoryType
    {
        Green, Yellow, Blue, Purple
    }

    struct Category
    {
        public CategoryType myType;
        string name;

        public Category(CategoryType _myType, string _name)
        {
            myType = _myType;
            name = _name;
        }
    }

    Dictionary<string, Category> Words;
    public GameSession()
    {
        Words = new();
        AddWords(CategoryType.Green, "Villes", "Paris", "Londres", "Helsinki", "Pékin");
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


    public void MakeGuess()
    {
        //could probably do something clean with LINQ
        int i = 0;
        List<Category> cats = new(); //hehehe
        foreach (var btn in UIManager.i.WordButtonsSelected)
        {
            //Find the word in the words list and add its category to the "cats" temporary list.
            cats.Add(Words.Where(x => x.Key == btn.associatedWord).Select(x => x.Value).FirstOrDefault());
        }
        //We now have a list of four categories. If they all match, the player has correctly selected 4 words of the same category.
        //Does anything not match?
        if (cats.Any(o => o.myType != cats[0].myType))
        {
            Debug.Log("Failure!");
        }
        else Debug.Log("Success!");
    }
}
