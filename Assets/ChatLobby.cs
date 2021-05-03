using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using System;
using UnityEngine.UI;
using TMPro;

public class ChatLobby : MonoBehaviourPun , IChatClientListener
{

    public ChatClient chatClient;
    public TMPro.TMP_InputField inp;
    public GameObject content, item;
    public string channel = "DEFAULT";
    public TMP_Dropdown list;

    public TMPro.TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {

        if (string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat))
        {
            return;
        }
        this.chatClient = new ChatClient(this);
        this.chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(PhotonNetwork.NickName));
        
    }

    public void Send()
    {
        if (inp.text.Replace(" ", "").Trim() != "")
        {
            string p = inp.text.Replace("~", "");
            chatClient.PublishMessage(channel, FindObjectOfType<WebData>().rank.ToString() + "~" + p);
            inp.text = "";
        }
        
    }



    // Update is called once per frame
    void Update()
    {
        text.text = "CHANNEL: " + channel;
        if (chatClient != null)
        {
            chatClient.Service();
            if (Input.GetKeyDown(KeyCode.Return) && Input.GetKey(KeyCode.LeftShift))
            {
                Send();
            }
        }
    }


    public void SetChannelChange()
    {
        SetChannel(list.options[list.value].text.ToUpper());
    }
    public void SetChannel(string ch)
    {
        if (ch != channel)
        {
            foreach (Transform item in content.transform)
            {
                Destroy(item.gameObject);
            }
            chatClient.Unsubscribe(new string[] { channel });
            channel = ch;
            chatClient.Subscribe(new string[] { channel }, 50);
        }
    }
    public void DebugReturn(DebugLevel level, string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        //throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        chatClient.Subscribe(new string[] { "DEFAULT" }, 50);

        chatClient.PublishMessage(channel, FindObjectOfType<Stats>().ranks.Count-1 + "~" + PhotonNetwork.NickName + " connected.");
    }

    public void OnDisconnected()
    {
        //throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            print(senders[i]);
            string mess = messages[i].ToString().Split('~')[1];
            int rank = int.Parse(messages[i].ToString().Split('~')[0]);

            GameObject g = Instantiate(item, content.transform);
            g.GetComponent<Text>().text = "   <color=#00C2FF>" + senders[i] + "</color>: " + mess;
            g.GetComponentInChildren<Image>().sprite = FindObjectOfType<Stats>().ranks[rank].sprite;
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        //throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
    //    throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        GameObject g = Instantiate(item, content.transform);
        g.GetComponent<Text>().text = "   <color=#00C2FF>" + channel + "</color>: +" + user;
        g.GetComponentInChildren<Image>().sprite = FindObjectOfType<Stats>().ranks[FindObjectOfType<Stats>().ranks.Count-1].sprite;
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        GameObject g = Instantiate(item, content.transform);
        g.GetComponent<Text>().text = "   <color=#00C2FF>" + channel + "</color>: -" + user;
        g.GetComponentInChildren<Image>().sprite = FindObjectOfType<Stats>().ranks[FindObjectOfType<Stats>().ranks.Count - 1].sprite;
    }
}
