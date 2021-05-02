using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color normal;
    public Color hover;

    bool over = false;
    Image button;

    private void Start()
    {
        normal = GetComponent<Image>().color;
        button = GetComponent<Image>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        over = true;
        //GetComponent<Image>().color = hover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        over = false;
        //GetComponent<Image>().color = normal;
    }


    private void Update()
    {
        if (over)
        {
            GetComponent<Image>().color = Color.Lerp(GetComponent<Image>().color, hover, 2 * Time.deltaTime);
        }
        else
        {
            GetComponent<Image>().color = Color.Lerp(GetComponent<Image>().color, normal, 2 * Time.deltaTime);
        }
    }
}
