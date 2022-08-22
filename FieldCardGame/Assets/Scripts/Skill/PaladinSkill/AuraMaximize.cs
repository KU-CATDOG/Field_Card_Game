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
        return "AURA Damage Enhance\n �������� ������ 5�� �����ϸ� �Ÿ� 3 �̳��� ������ 2���� ���ظ� �ݴϴ�.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        return null;
    }

}
