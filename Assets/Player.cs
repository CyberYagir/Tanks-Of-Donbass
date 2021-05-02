using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviourPun, IPunObservable {

    public Tank tank;
    public TankCamera camera;
    public Timer timer;

    public List<string> dmgs = new List<string>();

    public void Start()
    {
        transform.name = photonView.Owner.NickName;
        timer = FindObjectOfType<Timer>();
        
    }



    private void Awake()
    {
        if (!photonView.IsMine)
        {
            tank.controll = false;
            camera.camera.SetActive(false);
            camera.enabled = false;
            tank.Set(true); //Оно всё работает
        }
        else
        {
            camera.camera.GetComponent<PlayerCameraHandle>().name = photonView.Owner.NickName;
            var data = FindObjectOfType<WebData>();
            tank.corpus = data.corpus;
            tank.weapon = data.weapon;
        }
    }
    public void Update()
    {
        if (FindObjectOfType<Timer>().isEnd)
        {
            Destroy(gameObject);
        }

        if (photonView.IsMine) {
            tank.k = (int)photonView.Owner.CustomProperties["K"];
            tank.d = (int)photonView.Owner.CustomProperties["D"];

            if (tank.health <= 0)
            {
                Dead();
            }
        }

    }

    public void Dead()
    {
        if (photonView.IsMine)
        {
            tank.d++;
            SaveKD();

            GameObject dead = PhotonNetwork.Instantiate(FindObjectOfType<GameManager>().deadTank.name, transform.position, transform.rotation);
            dead.GetComponent<Tank>().corpus = tank.corpus;
            dead.GetComponent<Tank>().weapon = tank.weapon;

            //Destroy(Camera.main.gameObject);
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    public void TakeDamage(int dmg, string actorName)
    {
        tank.health -= dmg;
        if (tank.health <= 0)
        {
            Player player = GameObject.Find(actorName).GetComponent<Player>();
            player.photonView.RPC("AddKill", RpcTarget.AllBuffered);
            Dead();
            return;
        }

    } 

    [PunRPC]
    public void ShootAnim(string actorName)
    {
        Tank player = GameObject.Find(actorName).GetComponent<Player>().tank;
        if (player != null)
        {
            print(player.name);
            if (player.weapons[player.weapon].animManager != null)
            {
                if (player.weapon == 0)
                    (player.weapons[player.weapon].animManager as CannonAnim).Shoot();

                if (player.weapon == 2)
                {
                    (player.weapons[player.weapon].animManager as RailAnim).Shoot();
                }
                if (player.weapon == 3)
                {
                    (player.weapons[player.weapon].animManager as LauncherAnim).Shoot();
                }
            }
        }
    }


    [PunRPC]
    public void AddKill()
    {
        tank.k++;
        SaveKD();
    }
    public void  SaveKD()
    {
        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        h.Add("K", tank.k);
        h.Add("D", tank.d);
        photonView.Owner.SetCustomProperties(h);
    }

    public static void RefreshInstance(ref Player player, Player playerPrefab)
    {
        var pos = FindObjectOfType<GameManager>().points[Random.Range(0, FindObjectOfType<GameManager>().points.Length)].position;
        var rot = Quaternion.identity;
        if (player != null)
        {
            pos = player.transform.position;
            rot = player.transform.rotation;
            PhotonNetwork.Destroy(player.gameObject);
        }
        player = PhotonNetwork.Instantiate(playerPrefab.gameObject.name, pos, rot).GetComponent<Player>();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tank.weapon);
            stream.SendNext(tank.corpus);
            stream.SendNext(Input.GetKey(KeyCode.Space));

            stream.SendNext(tank.health);
            stream.SendNext(tank.team);
            stream.SendNext(tank.k);
            stream.SendNext(tank.d);

            stream.SendNext(tank.weapons[tank.weapon].weapon.transform.localEulerAngles.x);
            stream.SendNext(tank.weapons[tank.weapon].weapon.transform.localEulerAngles.y);
            stream.SendNext(tank.weapons[tank.weapon].weapon.transform.localEulerAngles.z);
        }
        else
        {
            tank.weapon = (int)stream.ReceiveNext();
            tank.corpus = (int)stream.ReceiveNext();
            tank.isShoot = (bool)stream.ReceiveNext();


            tank.health = (float)stream.ReceiveNext();
            tank.team = (float)stream.ReceiveNext();
            tank.k = (int)stream.ReceiveNext();
            tank.d = (int)stream.ReceiveNext();

            float x = (float)stream.ReceiveNext();
            float y = (float)stream.ReceiveNext();
            float z = (float)stream.ReceiveNext();
            tank.weapons[tank.weapon].weapon.transform.localEulerAngles  = new Vector3(x, y, z);
        }
    }
}
