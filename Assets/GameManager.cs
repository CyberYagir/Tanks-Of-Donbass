using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks {

    public Player playerPrefab;

    public Player LocalPlayer;

    public GameObject deadTank;

    public int currMap = -1;

    public float volume = 0;

    [Space]
    public Map[] maps;
    [Space]
    public GameObject deadCam;
    public Transform[] points;
    public Timer timer;
    [System.Serializable]
    public class Map
    {
        public string name;
        public Transform[] spawnPoints;
        public Transform[] spawnBonuses;
        public GameObject map;
    }

    private void Awake()
    {
        if (Screen.fullScreen) Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        volume = PlayerPrefs.GetFloat("Vol");
        //PlayerPrefs.SetInt("UnitySelectMonitor", 0);
        //Display.displays[0].Activate();
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Start");
            return;
        }
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && Input.GetKey(KeyCode.LeftShift))
        {
            Disconnect();
        }
        if (timer.isEnd == false)
        {
            if (LocalPlayer == null)
            {
                deadCam.SetActive(true);
                StartCoroutine(Respawn());
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    Suicide();
                }
                deadCam.SetActive(false);
                StopAllCoroutines();
            }
        }
        else
        {
            deadCam.SetActive(false);
        }

    }

    public void Disconnect()
    {
        if (LocalPlayer != null)
        {
            PhotonNetwork.Destroy(LocalPlayer.gameObject);
        }
        PhotonNetwork.LeaveRoom();
        Destroy(GameObject.Find("Manager"));
        Cursor.visible = true;
        SceneManager.LoadScene("Start");
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2);
        deadCam.GetComponentInChildren<TMPro.TMP_Text>().text = "Вы мертвы";
        Player.RefreshInstance(ref LocalPlayer, playerPrefab);
    }

    void SetMap()
    {
        currMap = (int)PhotonNetwork.CurrentRoom.CustomProperties["Map"];
        for (int i = 0; i < maps.Length; i++)
        {
            if (currMap == i)
            {
                maps[i].map.SetActive(true);
                points = maps[i].spawnPoints;
                GetComponent<BonusesSpawn>().points = maps[i].spawnBonuses;
            }
            else
                maps[i].map.SetActive(false);
        }
    }

    private void Start()
    {
        SetMap();
        deadCam.GetComponentInChildren<TMPro.TMP_Text>().text = "Сервер:" + PhotonNetwork.CurrentRoom.Name + "\n" + "Игроков: " + PhotonNetwork.CurrentRoom.PlayerCount;
        //Player.RefreshInstance(ref LocalPlayer, playerPrefab);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Player.RefreshInstance(ref LocalPlayer, playerPrefab);
    }

    public void Deselect()
    {
        FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
    }

    public void Suicide()
    {
        if (LocalPlayer != null)
        {
            LocalPlayer.GetComponent<Tank>().health = 0;
        }
    }
}
