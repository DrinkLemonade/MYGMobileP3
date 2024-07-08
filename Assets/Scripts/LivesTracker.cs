using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LivesTracker : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI TMP;
    // Start is called before the first frame update
    public void UpdateText(int count)
    {
        //We could add a little tween lerp animation to text size
        TMP.text = $"{count}";
    }
}
