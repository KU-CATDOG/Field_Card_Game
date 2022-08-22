using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicReinforce : LevelUpSkill
{
    private int count = 0;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 4;
    protected override void levelUpRoutine()
    {
        GameManager.Instance.CharacterSelected.AddTryHitAttackRoutine(MagicReinforcement(), 0);
        return;
    }
    private IEnumerator MagicReinforcement()
    {
        while (true)
        {
            if (GameManager.Instance.CharacterSelected.usedCard is IAttackCard && GameManager.Instance.CharacterSelected.usedCard.GetRange() >= 3)
            {
                GameManager.Instance.CharacterSelected.HitDmg += 5;
            }
            yield return null;
        }
    }
    public override string GetText()
    {
        return "MagicReinforce\n 사거리가 3이상인 카드가 5의 추가피해를 입힙니다.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        if (nextSkillList == null)
        {
            nextSkillList = new();
            nextSkillList.Add(new LongDistStigma());
        }
        return nextSkillList;
    }
}
