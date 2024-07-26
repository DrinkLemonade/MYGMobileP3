using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LevelSelectButton : MonoBehaviour
{
    public GameLevel associatedLevel;
    [SerializeField]
    public List<TextMeshProUGUI> wordDisplay;
    [SerializeField]
    public TextMeshProUGUI buttonText;
    [SerializeField]
    GameObject wordDisplayHolder;
    // Start is called before the first frame update
    void Awake()
    {
        wordDisplay = wordDisplayHolder.GetComponentsInChildren<TextMeshProUGUI>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IsClicked()
    {
        Debug.Log($"associated level: {associatedLevel.number}");
        List<string> allWords = new();
        foreach (var cat in associatedLevel.categories)
        {
            foreach (var word in cat.words)
            {
                allWords.Add(word);
            }
        }

        int y = 0;
        foreach (var item in allWords)
        {
            wordDisplay[y].text = item;
            wordDisplay[y].gameObject.name = item;
            y++;
        }
    }

}
