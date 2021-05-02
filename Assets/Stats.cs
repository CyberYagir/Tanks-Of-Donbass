using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public List<Rank> ranks = new List<Rank>();

    public Image rankSprite;
    public RectTransform rankBar;
    public TMP_Text rankProgess;

    public GameObject canvas;

    public WebData webData;

    private void Start()
    {
        for (int i = 1; i < ranks.Count; i++)
        {
            ranks[i].maxExp = ranks[i - 1].maxExp + ((ranks[i - 1].maxExp / 100) * 34);
        }
    }
    private void Update()
    {
        if (webData.isLogged)
        {
            if (webData.rank + 1 < ranks.Count)
            {
                if (webData.exp >= ranks[webData.rank].maxExp)
                {
                    webData.rank++;
                    webData.exp = 0;
                }
            }
            rankSprite.sprite = ranks[webData.rank].sprite;
            
            {
                rankBar.sizeDelta = new Vector2(((float)webData.exp/(float)ranks[webData.rank].maxExp)* 1142f, 15f);
            }
            
            rankProgess.text = webData.userName + ": " + webData.exp + " of " + ranks[webData.rank].maxExp;
        }
    }

}
[System.Serializable]
public class Rank
{
    public string name;
    public int maxExp;
    public Sprite sprite;
}