using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordToggle : MonoBehaviour
{
    public string associatedWord;
    private Color associatedColor;
    [SerializeField]
    TextMeshProUGUI tmp;
    [SerializeField]
    Image img;
    [SerializeField]
    public Toggle toggle;
    public GameObject anchor;

    [SerializeField]
    ButtonEffects effects;


    public bool Found = false; //Found words are permanently unusable and hidden behind the category banner
    private IEnumerator coroutine;
    public void SetWord(string word, Color col)
    {
        associatedWord = word;
        tmp.text = word;
        gameObject.name = word;
        associatedColor = col;
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

    public void MoveToAnchor()
    {
        coroutine = ActualMove();
        StartCoroutine(coroutine);
    }

    private IEnumerator ActualMove()
    {
        yield return null;
        transform.DOMove(anchor.transform.position, duration: UIManager.i.wordSortIntoCategoryAnimationDuration).SetEase(Ease.OutCubic);
        if (Found) toggle.enabled = false;
        if (Found) img.DOColor(associatedColor, duration: UIManager.i.wordSortIntoCategoryAnimationDuration).SetEase(Ease.OutCubic);
        Debug.Log(associatedColor);
        yield break;
    }

    public void Shake(float _force, float _direction)
    {
        effects.Shake(_force, _direction);
    }


}
