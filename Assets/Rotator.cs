using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            transform.Rotate(Vector3.up *speed * Time.deltaTime * -Input.GetAxisRaw("Mouse X"));
        }    
    }
}
