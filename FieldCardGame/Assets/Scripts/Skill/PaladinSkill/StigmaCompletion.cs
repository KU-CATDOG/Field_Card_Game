using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StigmaCompletion : LevelUpSkill
{
    private int count = 1;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 21;
    protected override void levelUpRoutine()
    {
        DivineStigma.Completion = true;
        return;
    }
    public override string GetText()
    {
        return "Stigma Reinforce\n 신성낙인이 부여될 때마다 대상에게 5의 피해를 입힙니다.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        return null;
    }

}
