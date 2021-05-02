using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public int id;
    public bool dest = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponentInParent<Player>() != null)
        {
            if (other.GetComponentInParent<PhotonView>().IsMine)
            {
                if (id == 0)
                {
                    other.GetComponentInParent<Tank>().health += 50;
                    if (other.GetComponentInParent<Tank>().health > 100)
                    {
                        other.GetComponentInParent<Tank>().health = 100;
                    }
                    other.GetComponentInParent<Tank>().AddHealthBonus();
                }
                if (id == 1)
                {
                    other.GetComponentInParent<Tank>().AddDamageBonus();
                }
                if (id == 2)
                {
                    other.GetComponentInParent<Tank>().AddSpeedBonus();
                }
                if (id == 3)
                {
                    other.GetComponentInParent<Tank>().AddRelBonus();
                }
            }

            dest = true;
            FindObjectOfType<BonusesSpawn>().FindBonuses();
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
