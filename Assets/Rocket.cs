using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class Rocket : MonoBehaviourPunCallbacks, IPunObservable
{
    public string player, target = "";
    public int damage;
    public float speed;
    public ParticleSystem particleSystem;
    private void Start()
    {
        particleSystem.Play();
        if (photonView.IsMine)
        {
            StartCoroutine(wait());
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (GameObject.Find(target) && target != "")
        {
            var id = GameObject.Find(target);
            transform.LookAt(id.GetComponent<Tank>().weapons[id.GetComponent<Tank>().weapon].weapon.transform);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.isTrigger == false)
            {
                if (other.GetComponent<Player>() != null)
                {
                    other.GetComponent<Player>().photonView.RPC("TakeDamage", Photon.Pun.RpcTarget.OthersBuffered, (int)damage, player); 
                    other.GetComponent<Player>().GetComponent<Player>().dmgs.Add("-" + (int)damage);
                }
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void OnDestroy()
    {
        particleSystem.Stop();
        particleSystem.transform.parent = null;
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
            stream.SendNext(player);
            stream.SendNext(target);
            stream.SendNext(damage);
        }
        else
        {
            player = (string)stream.ReceiveNext();
            target = (string)stream.ReceiveNext();
            damage = (int)stream.ReceiveNext();
        }
    }
}
