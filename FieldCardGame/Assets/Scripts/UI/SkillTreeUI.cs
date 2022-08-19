using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SkillTreeUI : SkillImage
{
    [SerializeField]
    private int id;
    public int ID => id;
    private RawImage Img;
    public RawImage Image
    {
        get
        {
            if(!Img)
            {
                Img = GetComponent<RawImage>();
            }
            return Img;
        }
    }
    public override void OnPointerClick(PointerEventData data)
    {
        return;
    }
    private void Start() 
    {
        ExplainText.text = "LOCKED";
        ReferenceSkill = GameManager.Instance.LvUpHandler.SkillDict[ID];
    }
}
