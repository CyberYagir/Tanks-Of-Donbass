using System.Collections;
using UnityEngine;

public class CannonAnim : MonoBehaviour
{
    public Light light;
    public ParticleSystem particleSystem;
    public GameObject point;
    public bool started = false;
    public AudioSource audio;
    public float startVolume = 0;
    GameManager gameManager;
    private void Start()
    {

        gameManager = FindObjectOfType<GameManager>();
        startVolume = audio.volume;
    }
    public void Shoot()
    {
        audio.PlayOneShot(audio.clip);
        started = true;
    }
    private void Update()
    {
        if (started)
        {
            light.intensity = 2;
            particleSystem.transform.position = point.transform.position;
            particleSystem.Play();
            started = false;
        }
        audio.volume = startVolume * gameManager.volume;
        light.intensity -= 25 * Time.deltaTime;
    }
} 
