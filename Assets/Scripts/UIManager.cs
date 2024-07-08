using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameSession;

public class UIManager : MonoBehaviour
{
    public static UIManager i;
    public List<WordToggle> WordButtonsSelected;
    List<WordToggle> WordButtons;
    [SerializeField]
    Button confirmButton;
    [SerializeField]
    List<GameObject> CategoryPanels;
    [SerializeField]
    Color CategoryGreenColor, CategoryYellowColor, CategoryBlueColor, CategoryPurpleColor;

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
            //Disable all buttons that aren't currently in the selected buttons list.
            if (!WordButtonsSelected.Contains(item)) item.toggle.interactable = false;
        }
    }

    public void EnableButtons()
    {
        foreach (var item in WordButtons)
        {
            //Don't re-enable buttons that have been found.
            if (!item.Found) item.toggle.interactable = true;
        }
    }

    public void EnablePanel(int atSiblingIndex, string catName, GameSession.CategoryType type)
    {
        //Change this to a less stupid version
        Color col;
        switch (type)
        {
            case CategoryType.Green:
                col = CategoryGreenColor;
                break;
            case CategoryType.Yellow:
                col = CategoryYellowColor;
                break;
            case CategoryType.Blue:
                col = CategoryBlueColor;
                break;
            case CategoryType.Purple:
                col = CategoryPurpleColor;
                break;
            default:
                col = Color.red;
                Debug.LogError("Unassigned category?");
                break;
        }

        CategoryPanels[atSiblingIndex].SetActive(true);
        CategoryPanels[atSiblingIndex].GetComponentInChildren<TextMeshProUGUI>().text = catName;
        CategoryPanels[atSiblingIndex].GetComponent<Image>().color = col;
    }
}
