using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAnim : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public Light light;
    public AudioSource au1, au2;
    public float wait = 0;
    GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        light.enabled = particleSystem.isPlaying;


        if (particleSystem.isPlaying)
        {
            au1.volume = 0.5f * gameManager.volume;
            au2.volume = 0.5f * gameManager.volume;
            if (au1.isPlaying == false)
            {
                au1.PlayOneShot(au1.clip);
            }
            wait += 5 * gameManager.volume * Time.deltaTime;
            if (wait > 4 * gameManager.volume)
            {
                if (au2.isPlaying == false)
                {
                    au2.PlayOneShot(au2.clip);
                }
            }
        }
        else
        {
            wait = 0;
            if (au1.volume > 0)
            {
                au1.volume -= 5 * gameManager.volume * Time.deltaTime;
                if (au1.volume < 0)
                {
                    au1.volume = 0;
                }
            }
            au2.volume = au1.volume;
        }
    }
}
