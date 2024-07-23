using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordToggle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
    float onSelectBounceScale, onSelectBounceDuration, onSelectBounceElasticity;
    [SerializeField]
    int onSelectBounceVibrato;
    [SerializeField]
    Ease onSelectEaseType;

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

    //OnPointerDown is also required to receive OnPointerUp callbacks
    public void OnPointerDown(PointerEventData eventData)
    {
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //var imgFadeSeq = DOTween.Sequence().Join(
        transform.DORewind();
        transform.DOPunchScale(new Vector3(onSelectBounceScale, onSelectBounceScale, onSelectBounceScale), onSelectBounceDuration, onSelectBounceVibrato, onSelectBounceElasticity)
            //Not sure if this does anything?
            .SetEase(onSelectEaseType);
        //imgFadeSeq.Append(transform.DOScale(onSelectBounceScale, onSelectBounceDuration));
        //imgFadeSeq.Append(transform.DOScale(1f, onSelectBounceDuration));
        //this.transform.DOScale(onSelectBounceScale, onSelectBounceDuration)
        Debug.Log("The mouse click was released");
    }
}
