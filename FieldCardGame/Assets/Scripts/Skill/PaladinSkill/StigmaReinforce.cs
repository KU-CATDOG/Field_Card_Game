using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StigmaReinforce : LevelUpSkill
{
    private int count = 0;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 8;
    protected override void levelUpRoutine()
    {
        DivineStigma.StigamDamage = 5;
        return;
    }
    public override string GetText()
    {
        return "Stigma Reinforce\n 신성낙인의 추가 피해를 5로 증가시킵니다.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        if (nextSkillList == null)
        {
            nextSkillList = new();
            nextSkillList.Add(new StigmaCycle());
            nextSkillList.Add(new StigmaPunish());
            Sanctuary tmp = new();
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
