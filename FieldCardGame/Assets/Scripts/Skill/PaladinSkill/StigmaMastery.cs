using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StigmaMastery : LevelUpSkill
{
    private int count = 2;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 22;
    protected override void levelUpRoutine()
    {
        DivineStigma.Mastery = true;
        return;
    }
    public override string GetText()
    {
        return "Stigma Mastery\n 신성낙인을 부여할 때마다 추가 신성낙인을 부여합니다.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        return null;
    }

}
