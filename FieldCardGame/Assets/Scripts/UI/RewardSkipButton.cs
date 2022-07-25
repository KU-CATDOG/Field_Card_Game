using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class RewardSkipButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        if(data.button == 0)
            PlayerUIManager.Instance.CloseRewardPanel();
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
