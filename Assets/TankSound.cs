using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSound : MonoBehaviourPun, IPunObservable
{
    public AudioSource audioSource;
    GameManager gameManager;
   
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(audioSource.volume);
        }
        else
        {
            audioSource.volume = (float)stream.ReceiveNext();
        }
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioSource.Play();
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            audioSource.volume = ((Math.Abs(Input.GetAxis("Vertical")) + Math.Abs(Input.GetAxis("Horizontal")))/5) * gameManager.volume;
        }
    }
}
