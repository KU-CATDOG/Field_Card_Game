using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraMaximize : LevelUpSkill
{
    private int count = 2;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 20;
    protected override void levelUpRoutine()
    {
        Aura.Range = 5;
        Aura.Maximization = true;
        return;
    }
    public override string GetText()
    {
        return "AURA Damage Enhance\n 오오라의 범위가 5로 증가하며 거리 3 이내의 적에게 2배의 피해를 줍니다.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        return null;
    }

}
