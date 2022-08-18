using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpHandler
{
    private List<LevelUpSkill> UnAvailableSkillList = new();
    private List<LevelUpSkill> AvailableSkillList = new();
    private Dictionary<int, LevelUpSkill> skillDict = new();
    public LevelUpHandler()
    {
        AddSkillList(GameManager.Instance.BaseSkillDict[GameManager.Instance.CharacterSelected as Player]);
    }
    private void AddSkillList(LevelUpSkill skill)
    {
        if(skillDict.ContainsKey(skill.ID)) return;
        skillDict[skill.ID] = skill;
        List<LevelUpSkill> list;
        if((list = skill.GetNextSkillList()).Count != 0)
        {
            foreach(var i in list)
            {
                AddSkillList(i);
            }
        }
        if(skill.Count == 0)
        {
            AvailableSkillList.Add(skill);
        }
        else
        {
            UnAvailableSkillList.Add(skill);
        }
    }
    public List<LevelUpSkill> GetAvailableSkill(int num)
    {
        List<LevelUpSkill> ret = new();
        for(int i = 0; i<num; i++)
        {
            int rand = Random.Range(0, AvailableSkillList.Count);
            ret.Add(AvailableSkillList[rand]);
        }
        return ret;
    }
    public void LevelUp(LevelUpSkill skill)
    {
        skill.LevelUpRoutine();
        var list = skill.GetNextSkillList();
        foreach(var i in list)
        {
            if(--i.Count <= 0)
            {
                UnAvailableSkillList.Remove(i);
                AvailableSkillList.Add(i);
            }
        }
    }
}
