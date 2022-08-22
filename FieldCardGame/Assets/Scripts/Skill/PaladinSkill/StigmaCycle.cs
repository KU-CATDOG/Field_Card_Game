using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StigmaCycle : LevelUpSkill
{
    private int count = 1;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 12;
    protected override void levelUpRoutine()
    {
        DivineStigma.Cycle = true;
        return;
    }
    public override string GetText()
    {
        return "Stigma Cycle\n 신성낙인을 소모하면 다시 부여합니다.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        if (nextSkillList == null)
        {
            nextSkillList = new();
            StigmaCompletion tmp = new();
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
