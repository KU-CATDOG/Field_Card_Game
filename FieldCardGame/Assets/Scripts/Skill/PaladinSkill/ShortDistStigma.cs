using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortDistStigma : LevelUpSkill
{
    private int count = 1;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 14;
    protected override void levelUpRoutine()
    {
        GameManager.Instance.CharacterSelected.AddHitAttackRoutine(SStigma(), 0);
        return;
    }
    private IEnumerator SStigma()
    {
        while (true)
        {
            if( GameManager.Instance.CharacterSelected.usedCard is IAttackCard && GameManager.Instance.CharacterSelected.usedCard.GetRange() <= 2 )
            {
                GameManager.Instance.CharacterSelected.HitTarget.EffectHandler.DebuffDict[DebuffType.DivineStigma].SetEffect(1);
            }
            yield return null;
        }
    }
    public override string GetText()
    {
        return "ShortDistStigma\n ��Ÿ��� 2������ ����ī�尡 �ż������� ����ϴ�.";
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
