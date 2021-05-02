using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class RailBullet : MonoBehaviour, IPunObservable
{
    public LineRenderer line;
    public bool hitPlayer;
    public Vector3 hitPoint;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hitPlayer);
            stream.SendNext(hitPoint);
        }
        else
        {
            hitPlayer = (bool)stream.ReceiveNext();
            hitPoint = (Vector3)stream.ReceiveNext();
        }
    }

    private void Start()
    {
        if (hitPlayer == false)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.collider != null)
                {
                    line.SetPosition(0, transform.position);
                    line.SetPosition(1, hit.point);
                }
            }
            else
            {
                line.SetPosition(0, transform.position);
                line.SetPosition(1, transform.position + transform.TransformVector((Vector3.forward * 10000)));
            }
        }
        else
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, hitPoint);
        }
        if (GetComponent<PhotonView>().IsMine)
        {
            StartCoroutine(wait());
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(4);
        PhotonNetwork.Destroy(gameObject);
    }
}
