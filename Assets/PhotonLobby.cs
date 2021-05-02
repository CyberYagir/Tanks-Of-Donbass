using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class PhotonLobby : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public static PhotonLobby lobby;
    public GameObject mainUI;
    public TMP_Text loadText;
    public TMP_Text errorText;
    public TMP_Text lobbyText;

    public List<RoomInfo> rooms;

    public ChatLobby chatLobby;

    private void Start()
    {
        errorText.text = "";
        lobby = this;
        PhotonNetwork.GameVersion = Application.version;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Update()
    {
        chatLobby.enabled = FindObjectOfType<WebData>().isLogged;

    }

    public override void OnConnectedToMaster()
    {
        loadText.text = "";
        mainUI.SetActive(true);
        PhotonNetwork.JoinLobby(new TypedLobby("DEFAULT", LobbyType.Default));
        lobbyText.text = PhotonNetwork.CurrentLobby.Name;
        
        base.OnConnectedToMaster();
    }


    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
    }

    public void ToBattle()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public void CreateRoom()
    {

        string name = "Room #" + Random.Range(0, 1000);
        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        h.Add("MaxTime", 300);
        h.Add("Map", Random.Range(0,2));
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10, CustomRoomProperties = h };
        PhotonNetwork.CreateRoom(name, roomOptions);
    }
    public void CreateRoom(string name, bool visible, bool open, byte players, int time, int map = 0)
    {
        if (name.Replace(" ", "") == "")
        {
            name = "Room #" + Random.Range(0, 1000);
        }

        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        if (time <= 0)
        {
            time = 9999999;
        }
        h.Add("MaxTime", time);
        h.Add("Map", map);
        RoomOptions roomOptions = new RoomOptions() { IsVisible = visible, IsOpen = open, MaxPlayers = players, CustomRoomProperties = h };
        PhotonNetwork.CreateRoom(name, roomOptions);
    }
    public void JoinRoom(TMP_Text name)
    {
        PhotonNetwork.JoinRoom(name.text);
    }
    public override void OnJoinedRoom()
    {
        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        h.Add("K", 0);
        h.Add("D", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(h);
        PhotonNetwork.LoadLevel(1);
        base.OnJoinedRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        errorText.text = "Join room Error";
        CreateRoom();
        base.OnJoinRandomFailed(returnCode, message);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        rooms = roomList;
        FindObjectOfType<MenuUI>().RoomsUpdate();
        base.OnRoomListUpdate(roomList);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Failed to create room";
        base.OnCreateRoomFailed(returnCode, message);
    }
}
