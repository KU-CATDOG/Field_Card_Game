using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWeight : LevelUpSkill
{
    private int count = 2;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 16;
    protected override void levelUpRoutine()
    {
        GameManager.Instance.CharacterSelected.AddTryHitAttackRoutine(strengthening(), 0);
        return;
    }
    private IEnumerator strengthening()
    {
        while (true)
        {
            if(GameManager.Instance.CharacterSelected.usedCard is IAttackCard&& GameManager.Instance.CharacterSelected.usedCard.GetRange() <= 2 && GameManager.Instance.CharacterSelected.EffectHandler.BuffDict[BuffType.Shield].Value >= 1)
            {
                GameManager.Instance.CharacterSelected.HitDmg += 5;
            }
            yield return null;
        }
    }
    public override string GetText()
    {
        return "Load Weight\n 보호를 가지고 있다면 사거리가 2이하인 공격카드가 5의 추가피해를 입힙니다.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        return null;
    }
}
