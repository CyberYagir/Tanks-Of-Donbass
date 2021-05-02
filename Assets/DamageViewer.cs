using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageViewer : MonoBehaviour
{
    public Player player;
    public Tank tank;
    public TMPro.TMP_Text text;


    public float secDMG;

    private void Start()
    {
        if (!player.photonView.IsMine)
        {
            StartCoroutine(wait());
        }
    }

    private void Update()
    {
        if (Camera.main != null)
        {
            text.transform.LookAt(Camera.main.transform);
        }
        secDMG = 0;
        for (int i = 0; i < player.dmgs.Count; i++)
        {
            secDMG += int.Parse(player.dmgs[i]);
        }
        if (secDMG != 0)
        {
            text.text = secDMG.ToString();
        }
    }

    IEnumerator wait()
    {
        while (true)
        {
            player.dmgs.Clear();
            yield return new WaitForSeconds(1);
            text.text = "";
        }
    }


}
