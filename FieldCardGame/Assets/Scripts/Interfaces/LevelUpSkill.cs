using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelUpSkill
{
    private int count;
    public abstract int ID{ get;}
    public abstract int Count{ get; set; }
    public void LevelUpRoutine()
    {
        levelUpRoutine();
    }
    ///First => Generate new instance, else => just return the instance generated 
    public abstract List<LevelUpSkill> GetNextSkillList();
    protected abstract void levelUpRoutine();
}
