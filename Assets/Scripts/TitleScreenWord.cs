using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleScreenWord : MonoBehaviour
{
    [SerializeField]
    float outlineWidth;
    Color outlineColor = Color.black;
    [SerializeField]
    float rotAmount, rotDuration, scaleMax, scaleDuration;
    Tweener tweener;
    void Start()
    {
        var textmeshPro = GetComponent<TextMeshProUGUI>();
        //textmeshPro.outlineWidth = outlineWidth;
        textmeshPro.outlineColor = outlineColor;


        Vector3 currentRot = transform.rotation.eulerAngles;
        var rotRight = currentRot;
        rotRight.z += rotAmount;

        //Rotate left by half of rotAmount. That way if rotAmount is 10, will rotate 5 left, then 5 right, 5 right, and 5 left, before repeating
        Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotAmount / 2);

        transform.DOScale(scaleMax, scaleDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        tweener = transform.DORotate(rotRight, rotDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);//.SetSpeedBased();

        //tweener.Play();
        //transform.DORotate(rotLeft, 1f).SetLoops(-1, LoopType.Incremental);
    }
}
