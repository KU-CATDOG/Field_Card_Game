using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDistStigma : LevelUpSkill
{
    private int count = 1;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 15;
    protected override void levelUpRoutine()
    {
        GameManager.Instance.CharacterSelected.AddHitAttackRoutine(LStigma(), 0);
        return;
    }
    private IEnumerator LStigma()
    {
        while (true)
        {
            if(GameManager.Instance.CharacterSelected.usedCard is IAttackCard && GameManager.Instance.CharacterSelected.usedCard.GetRange() >=3)
            {
                GameManager.Instance.CharacterSelected.HitTarget.EffectHandler.DebuffDict[DebuffType.DivineStigma].SetEffect(1);
            }
            yield return null;
        }
    }
    public override string GetText()
    {
        return "LongDistance Stigma\n 사거리가 3이상인 공격카드가 신성낙인을 남깁니다.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        if (nextSkillList == null)
        {
            nextSkillList = new();
            StigmaMastery tmp = new();
            if (GameManager.Instance.LvUpHandler.SkillDict.ContainsKey(tmp.ID))
            {
                nextSkillList.Add(GameManager.Instance.LvUpHandler.SkillDict[tmp.ID]);

            }
            else
            {
                nextSkillList.Add(tmp);
            }
        }
        return nextSkillList;
    }
}
