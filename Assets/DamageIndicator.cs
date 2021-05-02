using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Tank tank;
    public Image indic;
    public float oldHp;


    public void Start()
    {
        oldHp = tank.health;
    }


    private void Update()
    {
        if (oldHp != tank.health)
        {
            indic.color = new Color(1, 1, 1, 0.2f);
            oldHp = tank.health;
        }
        else
        {
            indic.color = Color.Lerp(indic.color,new Color(1, 1, 1, 0f), 5 * Time.deltaTime);
        }
    }
}
