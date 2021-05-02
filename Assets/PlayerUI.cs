using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviourPun
{
    public RectTransform HP, Rel;
    public TMP_Text text;
    public Tank tank;
    public GameObject scores;
    public GameObject scoresHolder, scoresItem;
    [Space]
    public GameObject dmgIcon, healIcon, spIcon, relIcon;

    private void Update()
    {
        HP.sizeDelta = new Vector2(100, (tank.health / tank.corpuses[tank.corpus].maxHealth) *100 );
        Rel.sizeDelta = new Vector2(100, tank.weapons[tank.weapon].value);
        text.text = "K:" + tank.k + "\nD:" + tank.d;
        scores.active = Input.GetKey(KeyCode.Tab);   
    }
    private void FixedUpdate()
    {
        if (tank.controll)
        {
            foreach (Transform item in scoresHolder.transform)
            {
                Destroy(item.gameObject);
            }
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("K", out object k) && PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("D", out object d))
                {
                    GameObject g = Instantiate(scoresItem, scoresHolder.transform);
                    g.transform.GetComponentInChildren<TMP_Text>().text = "" + PhotonNetwork.PlayerList[i].NickName + "\nK: " + (int)k + "\nD: " + (int)d;
                    g.SetActive(true);
                }
            }
            dmgIcon.SetActive(tank.dmgBonus != null);
            healIcon.SetActive(tank.healBonus != null);
            spIcon.SetActive(tank.spBonus != null);
            relIcon.SetActive(tank.relBonus != null);
        }
    }
}
