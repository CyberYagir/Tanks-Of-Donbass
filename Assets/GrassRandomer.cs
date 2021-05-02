using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassRandomer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        transform.localScale *= Random.Range(1, 1.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
