using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailAnim : MonoBehaviour
{
    public Transform sphere;
    public Light light;
    public bool started = false;
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
        if (started == false)
        {
           
            audio.PlayOneShot(audio.clip);
            sphere.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            started = true;
            light.intensity = 2;
        }
    }

    void Update()
    {
        if (started == true)
        {
            sphere.parent = null;
            sphere.localScale = Vector3.Lerp(sphere.localScale, Vector3.zero, 10 * Time.deltaTime);
            light.intensity -= 10 * Time.deltaTime;
            if (Math.Round(sphere.localScale.x,2) == 0f && light.intensity <= 0) {started =false; sphere.localScale = Vector3.zero; };
            sphere.parent = transform;
        }
        audio.volume = startVolume * gameManager.volume;
    }
}
