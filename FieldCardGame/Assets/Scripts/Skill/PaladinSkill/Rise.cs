using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rise : LevelUpSkill
{
    private int count = 0;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 6;
    protected override void levelUpRoutine()
    {
        IMoveCard.OnRise = true;
        return;
    }
    public override string GetText()
    {
        return "Rise\n '이동'카드의 사거리가 1 증가합니다.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        if (nextSkillList == null)
        {
            nextSkillList = new();
            Cardiovascular tmp = new();
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
