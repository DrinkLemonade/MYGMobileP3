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

    public bool Found = false; //Found words are permanently unusable and hidden behind the category banner

    public void SetWord(string word)
    {
        associatedWord = word;
        tmp.text = word;
        gameObject.name = word;
    }

    public void ToggleMe(bool toggle)
    {
        if (Found) return;
        if (toggle) UIManager.i.SelectWord(this);
        else UIManager.i.DeselectWord(this);
    }

    public void SetFound()
    {
        Found = true;
        toggle.interactable = false;
        toggle.isOn = false;
        UIManager.i.DeselectWord(this);
    }
}
