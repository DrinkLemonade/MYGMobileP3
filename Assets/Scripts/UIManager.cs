using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using static GameSession;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager i;
    //[NonSerialized]
    public List<WordToggle> WordButtonsSelected;
    //[NonSerialized]
    public List<WordToggle> WordButtons;
    //[NonSerialized]
    public List<WordToggleAnchor> ButtonAnchors;
    [SerializeField]
    Button confirmButton;
    [SerializeField]
    List<GameObject> CategoryPanels;
    [SerializeField]
    Color CategoryGreenColor, CategoryYellowColor, CategoryBlueColor, CategoryPurpleColor;
    [SerializeField]
    public LivesTracker livesTracker;
    [SerializeField]
    public InfoBanner infoBannerAlreadyGuessed, infoBannerOneAway;
    [SerializeField]
    GameEndPanel gameEndPanel;

    [SerializeField]
    public GameObject ButtonAnchorHolder, ButtonFreeHolder;
    public float wordSortIntoCategoryAnimationDuration = 1f;

    [SerializeField]
    float onCategoryFoundBounceScale, onCategoryFoundBounceDuration, onCategoryFoundBounceElasticity;
    [SerializeField]
    int onCategoryFoundBounceVibrato;

    private void Awake()
    {
        i = this;
        Init();
    }

    //Called by GameManager
    public void Init()
    {
        WordButtonsSelected = new();
        WordButtons = new();
        WordButtons = ButtonFreeHolder.GetComponentsInChildren<WordToggle>().ToList();
        ButtonAnchors = ButtonAnchorHolder.GetComponentsInChildren<WordToggleAnchor>().ToList();

        confirmButton.interactable = false;

        //Should take care of the problem where one value was mysteriously on at the start
        //TODO: Find why?

        var wordsStringOnly = GameManager.i.currentSession.Words.Keys.ToList();
        wordsStringOnly.Shuffle();
        int i = 0;
        foreach (var item in WordButtons)
        {
            item.toggle.SetIsOnWithoutNotify(false);

            Category.CategoryType type = GameManager.i.currentSession.Words.Where(x => x.Key == wordsStringOnly[i]).First().Value.myType;
            Color col = GetCategoryColor(type);

            item.SetWord(wordsStringOnly[i], col); //GameManager.i.currentSession.Words.Where(x => x.).);
            item.anchor = UIManager.i.ButtonAnchors[i].gameObject;
            item.anchor.name = $"{item.gameObject.name} Anchor";
            i++;
        }

        livesTracker.UpdateText(GameManager.i.currentSession.livesLeft);
        infoBannerAlreadyGuessed.HideInstantly();
        infoBannerOneAway.HideInstantly();
    }

    public void GridAnimation(int totalCatFound, string catName, Category.CategoryType catType, string words)
    {
        foreach (var item in UIManager.i.WordButtons)
        {
            item.MoveToAnchor();
        }
        var coroutine = EnablePanelDelayed(totalCatFound, catName, catType, words);
        StartCoroutine(coroutine);
        Debug.Log($"enabling: {words}");
    }


    private IEnumerator EnablePanelDelayed(int totalCatFound, string catName, Category.CategoryType catType, string words)
    {
        yield return new WaitForSeconds(wordSortIntoCategoryAnimationDuration);
        Debug.Log($"Enabling pane: {catName} - {words}");
        EnablePanel(totalCatFound, $"{catName} - {words}", catType);
        yield break;
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

    public void EnablePanel(int atSiblingIndex, string catName, Category.CategoryType type)
    {
        //Change this to a less stupid version
        Color col = GetCategoryColor(type);

        CategoryPanels[atSiblingIndex].SetActive(true);
        CategoryPanels[atSiblingIndex].GetComponentInChildren<TextMeshProUGUI>().text = catName;
        CategoryPanels[atSiblingIndex].GetComponent<Image>().color = col;

        CategoryPanels[atSiblingIndex].transform.DOPunchScale(new Vector3(onCategoryFoundBounceScale, onCategoryFoundBounceScale, onCategoryFoundBounceScale), onCategoryFoundBounceDuration, onCategoryFoundBounceVibrato, onCategoryFoundBounceElasticity);
    }

    public void ShakeSelectedButtons()
    {
        foreach (var item in WordButtonsSelected)
        {
            item.Shake(10f, 1f);
        }
    }

    public void Victory(float secs, int mistakes)
    {
        gameEndPanel.ShowSuccess(secs, mistakes);
    }
    public void Defeat()
    {
        gameEndPanel.ShowFailure();
    }

    Color GetCategoryColor(Category.CategoryType type)
    {
        Color col;
        switch (type)
        {
            case Category.CategoryType.Green:
                col = CategoryGreenColor;
                break;
            case Category.CategoryType.Yellow:
                col = CategoryYellowColor;
                break;
            case Category.CategoryType.Blue:
                col = CategoryBlueColor;
                break;
            case Category.CategoryType.Purple:
                col = CategoryPurpleColor;
                break;
            default:
                col = Color.red;
                Debug.LogError("Unassigned category?");
                break;
        }
        return col;
    }
}
