using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectReinforce : LevelUpSkill
{
    private int count = 0;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 5;
    protected override void levelUpRoutine()
    {
        Shield.Reinforced = true;
        return;
    }
    public override string GetText()
    {
        return "ProtectReinforce\n 보호를 얻을 때마다 3의 추가 보호를 얻습니다.";
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
            LoadWeight tmp2 = new();
            if (GameManager.Instance.LvUpHandler.SkillDict.ContainsKey(tmp2.ID))
            {
                nextSkillList.Add(GameManager.Instance.LvUpHandler.SkillDict[tmp2.ID]);

            }
            else
            {
                nextSkillList.Add(tmp2);
            }
            StaminaIs tmp3 = new();
            if (GameManager.Instance.LvUpHandler.SkillDict.ContainsKey(tmp3.ID))
            {
                nextSkillList.Add(GameManager.Instance.LvUpHandler.SkillDict[tmp3.ID]);

            }
            else
            {
                nextSkillList.Add(tmp3);
            }
        }
        return nextSkillList;
    }
}
