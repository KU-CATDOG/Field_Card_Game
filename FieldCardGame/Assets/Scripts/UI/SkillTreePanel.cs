using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTreePanel : MonoBehaviour
{
    private Dictionary<int, SkillTreeUI> UIDict = new();
    private void Awake() 
    {
        foreach(var i in GetComponentsInChildren<SkillTreeUI>())
        {
            UIDict[i.ID] = i;
        }
    }
    public void ActiveUI(int id)
    {

        UIDict[id].Image.color = Color.white;
        UIDict[id].ExplainText.color = Color.black;
        UIDict[id].ExplainText.text = UIDict[id].ReferenceSkill.GetText();
    }
}
