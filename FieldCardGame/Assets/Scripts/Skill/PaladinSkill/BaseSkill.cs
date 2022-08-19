using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : LevelUpSkill
{
    private int count = 0;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 0;
    protected override void levelUpRoutine()
    {
        return;
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        if(nextSkillList == null)
        {
            nextSkillList = new();
            nextSkillList.Add(new Aura());
            nextSkillList.Add(new Strengthening());
            nextSkillList.Add(new MagicReinforce());
            nextSkillList.Add(new Rise());
            nextSkillList.Add(new ProtectReinforce());
            nextSkillList.Add(new HealReinfoce());
            nextSkillList.Add(new EnLight());
        }
        return nextSkillList;
    }
    
}
