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
        return;
    }
    public override string GetText()
    {
        return "ProtectReinforce";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        return null;
    }
}
