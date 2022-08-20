using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnLight : LevelUpSkill
{
    private int count = 0;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 2;
    protected override void levelUpRoutine()
    {
        return;
    }
    public override string GetText()
    {
        return "EnLight";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        return null;
    }
}
