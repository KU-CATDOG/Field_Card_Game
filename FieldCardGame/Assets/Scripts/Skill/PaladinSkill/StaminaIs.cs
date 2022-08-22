using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaIs : LevelUpSkill
{
    private int count = 2;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 18;
    protected override void levelUpRoutine()
    {
        Shield.StaminaEnabled = true;
        GameManager.Instance.CharacterSelected.AddHealRoutine(routine(), 0);
        return;
    }
    private IEnumerator routine()
    {
        while (true)
        {
            if(GameManager.Instance.CharacterSelected.HealedBy is not Shield)
            {
                Shield.StaminaEnabled = false;
                GameManager.Instance.CharacterSelected.EffectHandler.BuffDict[BuffType.Shield].SetEffect(3);
                Shield.StaminaEnabled = true;
            }
            yield return null;
        }
    }
    public override string GetText()
    {
        return "Stamina is Power \n 보호를 얻을 때마다 3의 회복을, 회복할 때마다 3의 보호를 얻습니다.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        return null;
    }
}
