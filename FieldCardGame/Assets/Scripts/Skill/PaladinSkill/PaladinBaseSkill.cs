using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinBaseSkill : LevelUpSkill
{
    private int count = 0;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => -1;
    protected override void levelUpRoutine()
    {
        return;
    }
    public override string GetText()
    {
        return null;
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
