using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDead : MonoBehaviourPun, IPunObservable
{
    public Tank tank;
    public Color deadColor;
    public Color nullcolor;
    void Start()
    {
        if (photonView.IsMine)
        {
            GetComponent<Rigidbody>().AddRelativeForce(new Vector3(Random.Range(-2000, 5000), Random.Range(1000, 2000), Random.Range(-2000, 2000)));
            GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000), Random.Range(-1000, 1000)));
            StartCoroutine(wait());
        }

        tank.corpuses[tank.corpus].coprus.GetComponent<Renderer>().material.color = deadColor;
        tank.weapons[tank.weapon].weapon.GetComponent<Renderer>().material.color = deadColor;



    }
    private void FixedUpdate()
    {
        //tank.corpuses[tank.corpus].coprus.GetComponent<Renderer>().material.color = Color.Lerp(tank.corpuses[tank.corpus].coprus.GetComponent<Renderer>().material.color, nullcolor, 1 * Time.deltaTime);
        //tank.weapons[tank.weapon].weapon.GetComponent<Renderer>().material.color = Color.Lerp(tank.weapons[tank.weapon].weapon.GetComponent<Renderer>().material.color, nullcolor, 1 * Time.deltaTime);
    }



    IEnumerator wait()
    {
        yield return new WaitForSeconds(5);
        PhotonNetwork.Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(tank.weapon);
            stream.SendNext(tank.corpus);
        }
        else
        {
            tank.weapon = (int)stream.ReceiveNext();
            tank.corpus = (int)stream.ReceiveNext();
            tank.Set(true);
        }
    }
}
