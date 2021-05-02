using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassEnabler : MonoBehaviour
{
    public GameObject grass;
    void Update()
    {
        grass.SetActive(QualitySettings.GetQualityLevel() > 2);
    }
}
