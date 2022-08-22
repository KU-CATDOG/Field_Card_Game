using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cardiovascular : LevelUpSkill
{
    private int count = 2;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 17;
    protected override void levelUpRoutine()
    {
        GameManager.Instance.CharacterSelected.AddMoveRoutine(routine(), 0);
        return;
    }
    private IEnumerator routine()
    {
        while (true)
        {
            GameManager.Instance.CharacterSelected.EffectHandler.BuffDict[BuffType.Shield].SetEffect(1);
            yield return null;
        }
    }
    public override string GetText()
    {
        return "Cardiovascular \n 1ĭ �̵��� ������ 1�� ��ȣ�� ����ϴ�.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        return null;
    }
}
