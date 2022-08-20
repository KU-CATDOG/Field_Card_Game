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
        return;
    }
    public override string GetText()
    {
        return "Rise";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        return null;
    }
}
