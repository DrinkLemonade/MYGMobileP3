//By ManBoy Games
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class RepeatingTiledBackground : MonoBehaviour
{
    [SerializeField]
    private float tweenDuration;
    private void Awake()
    {
        var image = GetComponent<Image>();
        var rectTransform = GetComponent<RectTransform>();
        var sprite = image.sprite;
        var posTween = DOTween.To
            (
                () => rectTransform.anchoredPosition,
                x => rectTransform.anchoredPosition = x,
                new Vector2(-sprite.texture.width * 0.5f, -sprite.texture.height * 0.5f),
                tweenDuration
            );
        posTween.SetEase(Ease.Linear);
        posTween.SetLoops(-1, LoopType.Restart);

        var sizeTween = DOTween.To
            (
                () => rectTransform.sizeDelta,
                x => rectTransform.sizeDelta = x,
                new Vector2(sprite.texture.width, sprite.texture.height), tweenDuration
            );
        sizeTween.SetEase(Ease.Linear);
        sizeTween.SetLoops(-1, LoopType.Restart);
    }
}