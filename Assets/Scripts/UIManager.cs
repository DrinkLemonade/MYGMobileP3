using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using static GameSession;
using Unity.Collections.LowLevel.Unsafe;

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
    [SerializeField]
    public LivesTracker livesTracker;
    [SerializeField]
    public InfoBanner infoBannerAlreadyGuessed, infoBannerOneAway;
    [SerializeField]
    GameEndPanel gameEndPanel;

    [SerializeField]
    GameObject wordDummy;
    [SerializeField]
    public GameObject ButtonHolder;

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

        livesTracker.UpdateText(GameManager.i.currentSession.livesLeft);
        infoBannerAlreadyGuessed.HideInstantly();
        infoBannerOneAway.HideInstantly();
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

    public void ShakeSelectedButtons()
    {
        foreach (var item in WordButtonsSelected)
        {
            item.gameObject.transform.DOPunchPosition(new Vector3(10f, 0f, 0f), duration: 1f);
        }
    }

    public void SwapAnimation(WordToggle first, WordToggle second)
    {
        //Instantiate(wordDummy);
        //wordDummy.transform.position = first.transform.position;
        //Instantiate(wordDummy);
        //wordDummy.transform.position = second.transform.position;
        first.gameObject.transform.DOMove(second.gameObject.transform.position, duration: 1f).SetEase(Ease.OutCubic);
        second.gameObject.transform.DOMove(first.gameObject.transform.position, duration: 1f).SetEase(Ease.OutCubic);

    }

    public void Victory(float secs, int mistakes)
    {
        gameEndPanel.ShowSuccess(secs, mistakes);
    }
    public void Defeat()
    {
        gameEndPanel.ShowFailure();
    }
}
