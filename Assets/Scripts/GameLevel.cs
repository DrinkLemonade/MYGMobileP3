using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameLevel
{
    public int number;
    public List<Category> categories;

    public GameLevel(int num, List<Category> cat)
    {
        number = num;
        categories = new(cat);
    }
}
