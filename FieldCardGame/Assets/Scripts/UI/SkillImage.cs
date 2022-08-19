using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class SkillImage : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI explainText;
    public static bool Selected;
    public TextMeshProUGUI ExplainText
    {
        get
        {
            if(explainText == null)
            {
                explainText = GetComponentInChildren<TextMeshProUGUI>();
            }
            return explainText;
        }
    }
    public LevelUpSkill ReferenceSkill;
    public virtual void OnPointerClick(PointerEventData data)
    {
        if(data.button == 0)
        {
            if(Selected)    return;
            GameManager.Instance.LvUpHandler.LevelUp(ReferenceSkill);
            Selected = true;
            PlayerUIManager.Instance.SkillPanel.RewardEnd();
        }
    }
}
