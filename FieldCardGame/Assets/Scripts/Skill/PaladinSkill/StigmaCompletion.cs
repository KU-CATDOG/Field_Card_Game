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
        return "Stigma Reinforce\n �ż������� �ο��� ������ ��󿡰� 5�� ���ظ� �����ϴ�.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        return null;
    }

}
