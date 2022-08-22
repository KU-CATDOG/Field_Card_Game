using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ExitButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        if(data.button == 0)
        {
            Application.Quit();
        }
    }
    public void OnPointerEnter(PointerEventData data)
    {
        transform.localScale *= 1.1f;
    }
    public void OnPointerExit(PointerEventData data)
    {
        transform.localScale /= 1.1f;
    }
}
