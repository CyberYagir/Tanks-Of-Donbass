using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusesSpawn : MonoBehaviourPun
{
    public GameObject[] bonuses;
    public List<PhotonView> photonViews;

    public Transform[] points;
    public bool master;
    public List<Obj> objs = new List<Obj>();

    public IEnumerator loopwait;
    void Start()
    {
        master = PhotonNetwork.LocalPlayer.IsMasterClient;
    }
    [System.Serializable]
    public class Obj
    {
        public Vector3 pos, rot;
        public int type;

    }
    public void FindBonuses()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            objs.Clear();
            var obj = GameObject.FindGameObjectsWithTag("Bonus");
            for (int i = 0; i < obj.Length; i++)
            {
                var p = obj[i].GetComponent<Bonus>();
                if (!p.dest)
                    objs.Add(new Obj() { pos = obj[i].transform.position, rot = obj[i].transform.localEulerAngles, type = p.id });
            }
        }
    }
    public void ReSpawnBonuses()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            photonViews.Add(PhotonNetwork.Instantiate(bonuses[objs[i].type].name, objs[i].pos, Quaternion.Euler(objs[i].rot)).GetComponent<PhotonView>()); 
        }
        objs.Clear();
    }
    private void Update()
    {
        if (master != PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient == true)
            {
                ReSpawnBonuses();
            }
        }
        master = PhotonNetwork.LocalPlayer.IsMasterClient;
        if (loopwait == null)
        {
            loopwait = loop();
            StartCoroutine(loopwait);
        }
    }
    IEnumerator loop()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (photonViews.Count < points.Length + 2)
                {
                    int id = Random.Range(0, points.Length);
                    RaycastHit hit;
                    if (Physics.Raycast(points[id].position, Vector3.down, out hit))
                    {
                        if (hit.collider.GetComponent<Bonus>() == null)
                        {
                            photonViews.Add(PhotonNetwork.Instantiate(bonuses[Random.Range(0, bonuses.Length)].name, points[id].position, Quaternion.identity).GetComponent<PhotonView>());
                        }
                    }

                }
            }
            else
            {
                objs.Clear();
                var obj = GameObject.FindGameObjectsWithTag("Bonus");
                for (int i = 0; i < obj.Length; i++)
                {
                    objs.Add(new Obj() { pos = obj[i].transform.position, rot = obj[i].transform.localEulerAngles, type = obj[i].GetComponent<Bonus>().id });
                }
            }
            yield return new WaitForSeconds(10);
        }
    }
}
