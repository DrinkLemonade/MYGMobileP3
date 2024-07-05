using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordToggle : MonoBehaviour
{
    public string associatedWord;
    [SerializeField]
    TextMeshProUGUI tmp;
    [SerializeField]
    public Toggle toggle;

    public void SetWord(string word)
    {
        associatedWord = word;
        tmp.text = word;
    }

    public void ToggleMe(bool toggle)
    {
        if (toggle) UIManager.i.SelectWord(this);
        else UIManager.i.DeselectWord(this);
    }
}
