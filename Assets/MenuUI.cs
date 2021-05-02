using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class MenuUI : MonoBehaviour
{
    public PhotonLobby lobby;
    [Header("Create room")]
    public TMP_InputField roomName;
    public Slider playerCount, timeCount;
    public TMP_Text playersCountText, timeCountText;
    public Toggle open, visible;
    public List<GameObject> windows;
    public GameObject login;
    public TMP_Dropdown dropdown;

    [Header("PHP")]
    
    public TMPro.TMP_InputField LoginName;
    public TMPro.TMP_InputField LoginPass;
    [Space(20)]
    public TMPro.TMP_InputField RegName;
    public TMPro.TMP_InputField RegPass;
    public TMPro.TMP_InputField RegPass2;
    public Toggle saveInput;
    [Space]
    public WebData webData;
    [Space]
    public GameObject regB, loginB;



    [Header("UI")]
    public GameObject ddCanvas;
    public Slider weapon;
    public Slider corpus;
    public Tank previewTank;
    public TMP_Text weaponStat, corpusStat;

    public GameObject roomItem, roomsHolder;
    public Sprite fullscr, minScr;
    public Image fullScrButton;

    public Slider sliderVolume;
    [Header("Quality")]
    public TMP_Text QualityText;
    public void ChangeQuality()
    {
        int q = QualitySettings.GetQualityLevel()+1;
        if (q == 6)
        {
            q = 0;
        }
        QualityText.text = "Quality: " + q;
        PlayerPrefs.SetInt("Graphic", q);
        QualitySettings.SetQualityLevel(q);
    }

    private void Start()
    {
        lobby = PhotonLobby.lobby;
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Graphic", 5));
        QualityText.text = "Quality: " + QualitySettings.GetQualityLevel();
        weapon.maxValue = previewTank.weapons.Count - 1;
        corpus.maxValue = previewTank.corpuses.Count - 1;

        LoginName.text = PlayerPrefs.GetString("Login");
        LoginPass.text = PlayerPrefs.GetString("Password");

        if (!PlayerPrefs.HasKey("Vol"))
        {
            PlayerPrefs.SetFloat("Vol", 0.1f);
        }
        sliderVolume.value = PlayerPrefs.GetFloat("Vol", 0.1f);
        saveInput.isOn = bool.Parse(PlayerPrefs.GetString("Save") != "" ? PlayerPrefs.GetString("Save") : "False");
    }
   
    public void TankChange()
    {
        webData.weapon = (int)weapon.value;
        webData.corpus = (int)corpus.value;
        webData.SaveStart();
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void SetVolume()
    {
        PlayerPrefs.SetFloat("Vol", sliderVolume.value);
    }
    private void FixedUpdate()
    {

        FindObjectOfType<Tank>().Set();
        webData = FindObjectOfType<WebData>();
        ddCanvas = FindObjectOfType<Stats>().canvas;
        weaponStat.text = previewTank.weapons[(int)weapon.value].name + "\n" + previewTank.weapons[(int)weapon.value].damage + "\n" + previewTank.weapons[(int)weapon.value].valueAdd + "\n" + previewTank.weapons[(int)weapon.value].rotSpeed;
        corpusStat.text = previewTank.corpuses[(int)corpus.value].name + "\n" + previewTank.corpuses[(int)corpus.value].speed + "\n" + previewTank.corpuses[(int)corpus.value].maxHealth + "\n" + previewTank.corpuses[(int)corpus.value].rotSpeed;
        previewTank.corpus = (int)corpus.value;
        previewTank.weapon = (int)weapon.value;
        login.SetActive(!webData.isLogged);
        ddCanvas.SetActive(webData.isLogged);
        playersCountText.text = playerCount.value.ToString();

        TimeSpan time = TimeSpan.FromSeconds(timeCount.value);
        timeCountText.text = time.ToString(@"hh\:mm\:ss");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            for (int i = 0; i < windows.Count; i++)
            {
                windows[i].SetActive(false);
            }
        }
    }

    private void Update()
    {
        weapon.value = webData.weapon;
        corpus.value = webData.corpus;
        if (login.active == false )
        {
            if (!windows[3].active)
            {
                if (Input.GetKeyDown(KeyCode.B))
                {
                    PhotonLobby.lobby.ToBattle();
                }
                if (Input.GetKeyDown(KeyCode.G))
                {
                    OpenClose(windows[1]);
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    OpenClose(windows[0]);
                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    OpenClose(windows[2]);
                }
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    OpenClose(windows[3]);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
                {
                    OpenClose(windows[3]);
                }
            }
        }
    }

    public void CreateRoom()
    {
        PhotonLobby.lobby.CreateRoom(roomName.text, visible.isOn, open.isOn, (byte)playerCount.value, (int)timeCount.value, dropdown.value);
    }
    public void JoinRoom(TMP_Text name)
    {
        PhotonLobby.lobby.JoinRoom(name);
    }
    public void RoomsUpdate()
    {
        foreach (Transform item in roomsHolder.transform)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < lobby.rooms.Count; i++)
        {
            if (lobby.rooms[i].PlayerCount == 0 && lobby.rooms[i].MaxPlayers == 0) continue;
            GameObject g = Instantiate(roomItem, roomsHolder.transform);
            g.transform.GetChild(1).GetComponent<TMP_Text>().text = lobby.rooms[i].Name;
            g.transform.GetChild(2).GetComponent<TMP_Text>().text = lobby.rooms[i].PlayerCount + "/" + lobby.rooms[i].MaxPlayers;
            g.SetActive(true);
        }
    }

    public void Exit()
    {
        webData.Exit();
    }

    public void OpenClose(GameObject obj)
    {
        for (int i = 0; i < windows.Count; i++)
        {
            if (obj != windows[i])
                windows[i].SetActive(false);
        }
        obj.active = !obj.active;
    }

    public void Full()
    {
        if (!Screen.fullScreen)
        {
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true, 60);
            fullScrButton.sprite = minScr;
        }
        else
        {
            Screen.SetResolution(Screen.width, Screen.height, false, 60);
            fullScrButton.sprite = fullscr;
        }
    }
}
