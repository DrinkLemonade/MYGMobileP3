using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager i;
    public List<WordToggle> WordButtonsSelected;
    List<WordToggle> WordButtons;
    [SerializeField]
    Button confirmButton;

    private void Awake()
    {
        i = this;
        WordButtonsSelected = new();
        WordButtons = new();
        WordButtons = FindObjectsOfType<WordToggle>().ToList();
        confirmButton.interactable = false;

        //Should take care of the problem where one value was mysteriously on at the start
        //TODO: Find why?
        foreach (var item in WordButtons)
        {
            item.toggle.SetIsOnWithoutNotify(false);
        }
    }

    public void SelectWord(WordToggle btn)
    {
        if (WordButtonsSelected.Count < 4)
        {
            WordButtonsSelected.Add(btn);
            Debug.Log("adding!");

            if (WordButtonsSelected.Count == 4)
            {
                DisableUnselectedButtons();
                confirmButton.interactable = true;
            }
        }
    }

    public void DeselectWord(WordToggle btn)
    { 
        WordButtonsSelected.Remove(btn);
        if (WordButtonsSelected.Count == 3)
        {
            EnableButtons();
            confirmButton.interactable = false;
        }
    }

    void DisableUnselectedButtons()
    {
        foreach (var item in WordButtons)
        {
            if (!WordButtonsSelected.Contains(item)) item.toggle.interactable = false;
        }
    }

    void EnableButtons()
    {
        foreach (var item in WordButtons)
        {
            item.toggle.interactable = true;
        }
    }
}
