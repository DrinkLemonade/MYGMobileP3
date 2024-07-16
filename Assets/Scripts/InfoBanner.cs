using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoBanner : MonoBehaviour
{
    [SerializeField]
    float fadeInTimeSeconds, staysVisibleTimeSeconds, fadeOutTimeSeconds;

    [SerializeField]
    Image img;
    [SerializeField]
    TextMeshProUGUI tmp;

    Sequence imgFadeSeq, tmpFadeSeq;
    public void Awake()
    {
        
    }
    public void Show()
    {
        imgFadeSeq.Rewind();
        tmpFadeSeq.Rewind();

        imgFadeSeq = DOTween.Sequence();
        imgFadeSeq.Append(img.DOFade(1f, fadeInTimeSeconds));
        imgFadeSeq.AppendInterval(staysVisibleTimeSeconds);
        imgFadeSeq.Append(img.DOFade(0f, fadeOutTimeSeconds));

        tmpFadeSeq = DOTween.Sequence();
        tmpFadeSeq.Append(tmp.DOFade(1f, fadeInTimeSeconds));
        tmpFadeSeq.AppendInterval(staysVisibleTimeSeconds);
        tmpFadeSeq.Append(tmp.DOFade(0f, fadeOutTimeSeconds));
        //imgFadeSeq.Play();
        //tmpFadeSeq.Play();
    }
    public void HideInstantly()
    {
        img.DOFade(0f, 0f);
        tmp.DOFade(0f, 0f);
        //Make transparent
        //img.color = new Color(img.color.r, img.color.g, img.color.b, 0);

    }
}
