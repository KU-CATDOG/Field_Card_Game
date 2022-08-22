using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraDmgUp : LevelUpSkill
{
    private int count = 1;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 10;
    protected override void levelUpRoutine()
    {
        Aura.Dmg = 7;
        return;
    }
    public override string GetText()
    {
        return "AURA Damage Enhance\n 오오라의 데미지를 7로 증가시킵니다.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        if(nextSkillList == null)
        {
            nextSkillList = new();
            AuraMaximize tmp = new();
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
