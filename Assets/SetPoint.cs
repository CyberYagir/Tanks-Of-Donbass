using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPoint : MonoBehaviour
{
    public Transform point;
    public Transform weapon;



    private void Update()
    {
        transform.position = point.position;
        transform.rotation = weapon.rotation;
    }
}
