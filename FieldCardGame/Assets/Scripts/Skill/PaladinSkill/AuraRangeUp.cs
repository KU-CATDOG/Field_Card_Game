using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraRangeUp : LevelUpSkill
{
    private int count = 1;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 9;
    protected override void levelUpRoutine()
    {
        Aura.Range = 3;
        return;
    }
    public override string GetText()
    {
        return "AURA Range Enhance\n 오오라의 범위를 3으로 증가시킵니다.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        if (nextSkillList == null)
        {
            nextSkillList = new();
            AuraMaximize tmp = new();
            if (GameManager.Instance.LvUpHandler.SkillDict.ContainsKey(tmp.ID))
            {
                nextSkillList.Add(GameManager.Instance.LvUpHandler.SkillDict[tmp.ID]);
            }
            else
            {
                nextSkillList.Add(tmp);
            }
        }
        return nextSkillList;
    }

}
