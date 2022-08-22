using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strengthening : LevelUpSkill
{
    private int count = 0;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 7;
    protected override void levelUpRoutine()
    {
        GameManager.Instance.CharacterSelected.AddTryHitAttackRoutine(strengthening(), 0);
        return;
    }
    private IEnumerator strengthening()
    {
        while (true)
        {
            if(GameManager.Instance.CharacterSelected.usedCard is IAttackCard && GameManager.Instance.CharacterSelected.usedCard.GetRange() <= 2)
            {
                GameManager.Instance.CharacterSelected.HitDmg += 5;
            }
            yield return null;
        }
    }
    public override string GetText()
    {
        return "Strengthening\n ��Ÿ��� 2������ ����ī�尡 5�� �߰����ظ� �����ϴ�.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        if (nextSkillList == null)
        {
            nextSkillList = new();
            nextSkillList.Add(new ShortDistStigma());
            LoadWeight tmp = new();
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
