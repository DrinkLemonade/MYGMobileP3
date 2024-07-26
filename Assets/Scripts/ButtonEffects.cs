using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEffects : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    Toggle toggle;
    [SerializeField]
    Button button;

    [SerializeField]
    float onSelectBounceScale, onSelectBounceDuration, onSelectBounceElasticity;
    [SerializeField]
    int onSelectBounceVibrato;
    [SerializeField]
    Ease onSelectEaseType;

    [SerializeField]
    AudioClip audioOnSelect, audioOnDeselect;

    //OnPointerDown is also required to receive OnPointerUp callbacks
    public void OnPointerDown(PointerEventData eventData)
    {

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //var imgFadeSeq = DOTween.Sequence().Join(

        //Cancel ongoing animations in case the user spams clicks
        transform.DORewind();

        //Different check depending on whether the attached UI is a button or a toggle
        bool interactable = true;
        if (toggle != null)
        {
            interactable = toggle.interactable;
            if (interactable) SoundManager.i.PlaySound(toggle.isOn ? audioOnSelect : audioOnDeselect);
        }
        else if (button != null)
        {
            interactable = button.interactable;
            if (interactable) SoundManager.i.PlaySound(audioOnSelect);
            Debug.Log("button not null. interactable? " + interactable);
        }

        if (interactable) transform.DOPunchScale(new Vector3(onSelectBounceScale, onSelectBounceScale, onSelectBounceScale), onSelectBounceDuration, onSelectBounceVibrato, onSelectBounceElasticity)
            //Not sure if this does anything?
            .SetEase(onSelectEaseType);
        else Shake(5f, 0.5f);
        //imgFadeSeq.Append(transform.DOScale(onSelectBounceScale, onSelectBounceDuration));
        //imgFadeSeq.Append(transform.DOScale(1f, onSelectBounceDuration));
        //this.transform.DOScale(onSelectBounceScale, onSelectBounceDuration)
    }

    public void Shake(float force, float duration)
    {
        gameObject.transform.DOPunchPosition(new Vector3(force, 0f, 0f), duration: duration);
    }
}
