using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanctuary : LevelUpSkill
{
    private int count = 2;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 11;
    protected override void levelUpRoutine()
    {
        Aura.Stigma = true;
        return;
    }
    public override string GetText()
    {
        return "Sanctuary\n ������ �ż������� �߰��� �ο��մϴ�.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        return null;
    }

}
