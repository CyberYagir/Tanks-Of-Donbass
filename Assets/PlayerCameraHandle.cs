using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraHandle : MonoBehaviour
{
    public string name;
    public GameObject player;
    public GameObject sec, canvas;
    public AudioListener audioListener;

    private void Start()
    {
        GetComponent<Camera>().targetDisplay = 0;
        audioListener = GetComponent<AudioListener>();
    }
    private void LateUpdate()
    {
        if (player == null)
        {
            var p = FindObjectsOfType<PlayerCameraHandle>();
            for (int i = 0; i < p.Length; i++)
            {
                if (p[i].player == null)
                {
                    p[i].GetComponent<Camera>().enabled = false;
                    p[i].canvas.SetActive(false);
                    p[i].sec.SetActive(false);
                    p[i].audioListener.enabled = false;
                }
            }
            GameObject pl = GameObject.Find(name);
            if (pl != null)
            {
                if (pl.GetComponent<TankCamera>().camera == null)
                {
                    pl.GetComponent<TankCamera>().camera = this.gameObject;
                }
            }
        }
        if (player != null)
        {
            GetComponent<Camera>().enabled = true;
            canvas.SetActive(true);
            sec.SetActive(true);
            audioListener.enabled = true;
        }
    }
}
