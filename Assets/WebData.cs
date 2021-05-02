using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebData : MonoBehaviour
{
    public static WebData webData;

    public bool isLogged;

    public int id;
    public string userName;
    public int exp;
    public int rank;
    public int corpus, weapon;

    public string URL;
    public List<string> data = new List<string>();

    private void Start()
    {

        if (webData == null)
        {
            webData = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        StopAllCoroutines();
        StartCoroutine(INFSave());
    }

    IEnumerator INFSave()
    {
        while (true){
            yield return new WaitForSeconds(5);
            SaveStart();
        }
    }

    public void LoginStart()
    {
        StartCoroutine(Login(FindObjectOfType<MenuUI>().LoginName.text, FindObjectOfType<MenuUI>().LoginPass.text));
    }
    public void RegStart()
    {
        StartCoroutine(Register(FindObjectOfType<MenuUI>().RegName.text, FindObjectOfType<MenuUI>().RegPass.text, FindObjectOfType<MenuUI>().RegPass2.text));
    }

    public void SaveStart()
    {
        StartCoroutine(Save());
    }

    IEnumerator Login(string name, string password)
    {
        var mn = FindObjectOfType<MenuUI>();
        if (mn.saveInput.isOn == true) {
            PlayerPrefs.SetString("Login", mn.LoginName.text);
            PlayerPrefs.SetString("Password", mn.LoginPass.text);
            PlayerPrefs.SetString("Save", "true");
        }
        else{
            PlayerPrefs.SetString("Login", "");
            PlayerPrefs.SetString("Password","");
            PlayerPrefs.SetString("Save", "false");
        }

        mn.loginB.SetActive(false);
        WWWForm form = new WWWForm();
        form.AddField("login", "");
        form.AddField("name", name.ToLower());
        form.AddField("password", password.ToLower());
        WWW www = new WWW(URL, form);
        yield return www;

        mn.loginB.SetActive(true);
        data.Clear();
        data.AddRange(www.text.Split(';'));
        if (data[0].ToLower() == "logged")
        {
            id = int.Parse(data[1]);
            userName = data[2];
            exp = int.Parse(data[3]);
            rank = int.Parse(data[4]);
            corpus = int.Parse(data[5]);
            weapon = int.Parse(data[6]);
            isLogged = true;

            FindObjectOfType<MenuUI>().weapon.value = weapon;
            FindObjectOfType<MenuUI>().corpus.value = corpus;

            Photon.Pun.PhotonNetwork.NickName = userName;

            FindObjectOfType<PhotonLobby>().errorText.text = "";
            StopAllCoroutines();
            yield break;
        }
        else
        {
            FindObjectOfType<PhotonLobby>().errorText.text = data[0];
        }

    }

    IEnumerator Save()
    {
        WWWForm form = new WWWForm();
        form.AddField("save", "");
        form.AddField("exp", exp);
        form.AddField("rank", rank);
        form.AddField("corpus", corpus);
        form.AddField("weapon", weapon);
        form.AddField("id", id);
        WWW www = new WWW(URL, form);
        yield return www;
    }

    public void Exit()
    {
        SaveStart();
        isLogged = false;
    }
    IEnumerator Register(string name, string password, string password2)
    {
        if (password.ToLower() == password2.ToLower())
        {

            FindObjectOfType<MenuUI>().regB.SetActive(false);
            WWWForm form = new WWWForm();
            form.AddField("register", "");
            form.AddField("name", name.ToLower());
            form.AddField("password", password.ToLower());
            WWW www = new WWW(URL, form);
            yield return www;

            FindObjectOfType<MenuUI>().regB.SetActive(true);
            data.Clear();
            data.AddRange(www.text.Split(';'));
            if (data[0].ToLower() == "apply")
            {
                FindObjectOfType<MenuUI>().LoginName.text = name;
                FindObjectOfType<MenuUI>().LoginName.transform.parent.gameObject.SetActive(true);
                FindObjectOfType<MenuUI>().RegName.transform.parent.gameObject.SetActive(false);
                FindObjectOfType<PhotonLobby>().errorText.text = "";
                StopAllCoroutines();
                yield break;
            }
            else
            {
                FindObjectOfType<PhotonLobby>().errorText.text = data[0];
            }
        }
        else
        {
            FindObjectOfType<PhotonLobby>().errorText.text = "Passwords";
        }
    }
}
