using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpHandler
{
    private List<LevelUpSkill> UnAvailableSkillList = new();
    private List<LevelUpSkill> AvailableSkillList = new();
    private Dictionary<int, LevelUpSkill> skillDict = new();
    public IReadOnlyDictionary<int, LevelUpSkill> SkillDict => skillDict;
    public LevelUpHandler()
    {
        AddSkillList(GameManager.Instance.BaseSkillDict[GameManager.Instance.CharacterSelected.GetType()]);
        LevelUp(GameManager.Instance.BaseSkillDict[GameManager.Instance.CharacterSelected.GetType()]);
    }
    private void AddSkillList(LevelUpSkill skill)
    {
        if(skillDict.ContainsKey(skill.ID)) return;
        skillDict[skill.ID] = skill;
        List<LevelUpSkill> list;
        if((list = skill.GetNextSkillList()) != null && list.Count != 0)
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
        bool[] visited = new bool[AvailableSkillList.Count];
        for(int i = 0; i<num && i<AvailableSkillList.Count; i++)
        {
            int rand = Random.Range(0, AvailableSkillList.Count);
            if(visited[rand])
            {
                i--;
                continue;
            }
            visited[rand] = true;
            ret.Add(AvailableSkillList[rand]);
        }
        return ret;
    }
    public void LevelUp(LevelUpSkill skill)
    {
        skill.LevelUpRoutine();
        if(PlayerUIManager.Instance)
            (GameManager.Instance.CharacterSelected as Player).TreePanel.ActiveUI(skill.ID);
        AvailableSkillList.Remove(skill);
        var list = skill.GetNextSkillList();
        if(list == null)    return;
        foreach(var i in list)
        {
            if(--i.Count == 0)
            {
                UnAvailableSkillList.Remove(i);
                AvailableSkillList.Add(i);
            }
        }
    }
}
