using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SkillTreeCloseButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        if(data.button == 0)
        {
            PlayerUIManager.Instance.CloseSkillTreePanel();
        }
    }
}
