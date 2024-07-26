using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Category
{
    public CategoryType myType;
    public string name;
    public List<string> words;

    public enum CategoryType
    {
        Green, Yellow, Blue, Purple
    }

    public Category(CategoryType _myType, string _name, List<string> _words)
    {
        myType = _myType;
        name = _name;
        words = _words;
    }
}
