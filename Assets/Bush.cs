using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    GameManager gameManager;
    public Renderer render;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }



    private void FixedUpdate()
    {
        if (gameManager != null)
        {
            if (gameManager.LocalPlayer != null)
            {
                if (Vector3.Distance(gameManager.LocalPlayer.transform.position, transform.position) < 5)
                {
                    render.material.color = new Color(render.material.color.r, render.material.color.g, render.material.color.b, 0.5f);
                }
                else { render.material.color = new Color(render.material.color.r, render.material.color.g, render.material.color.b, 1f); }
            }
            else { render.material.color = new Color(render.material.color.r, render.material.color.g, render.material.color.b, 1f); };
        }
    }
}
