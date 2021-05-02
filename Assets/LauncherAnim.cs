using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherAnim : MonoBehaviour
{
    public AudioSource audio;

    float startVolume = 0;
    GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        startVolume = audio.volume;
    }
    public void Shoot()
    {
        audio.volume = startVolume * gameManager.volume;
        audio.PlayOneShot(audio.clip);

    }
}
