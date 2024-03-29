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
        PaladinDefilement.Enlighted = true;
        return;
    }
    public override string GetText()
    {
        return "EnLight\n 번뇌 카드를 사용할 수 있게 됩니다.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        if (nextSkillList == null)
        {
            nextSkillList = new();
            nextSkillList.Add(new Scintillation());
        }
        return nextSkillList;
    }
}
