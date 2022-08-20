using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectPanel : MonoBehaviour
{
    private List<SkillImage> rewardList = new();
    private Vector3 oddPos = new Vector3(0, 0, 0);
    private Vector3 evenPos = new Vector3(150, 0, 0);
    private float distance = 300;
    public void ShowReward(List<LevelUpSkill> _rewardList)
    {
        SkillImage.Selected = false;
        gameObject.SetActive(true);
        Vector3 defaultPos;
        if (_rewardList.Count % 2 == 0)
        {
            defaultPos = evenPos;
        }
        else
        {
            defaultPos = oddPos;
        }
        for (int i = 0; i < _rewardList.Count; i++)
        {
            var skill = _rewardList[i];
            SkillImage obj = Instantiate(GameManager.Instance.SkillImage, transform);
            obj.ReferenceSkill = skill;
            obj.ExplainText.text = skill.GetText();
            obj.transform.localPosition = defaultPos + (i - _rewardList.Count / 2) * Vector3.right * distance;
            rewardList.Add(obj);
        }
    }
    public void RewardEnd()
    {
        foreach(var i in rewardList)
        {
            Destroy(i.gameObject);
        }
        rewardList.Clear();
        gameObject.SetActive(false);
        PlayerUIManager.Instance.PanelOpenned = false;
    }
}
