using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealReinfoce : LevelUpSkill
{
    private int count = 0;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 3;
    protected override void levelUpRoutine()
    {
        GameManager.Instance.CharacterSelected.AddTryHealRoutine(HealReinforcement(), 0);
        return;
    }
    private IEnumerator HealReinforcement()
    {
        while (true)
        {
            GameManager.Instance.CharacterSelected.HealAmount += 3;
            yield return null;
        }
    }
    public override string GetText()
    {
        return "HealReinforce\n 회복할때마다 3만큼 추가로 회복합니다.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        if (nextSkillList == null)
        {
            nextSkillList = new();
            StaminaIs tmp = new();
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
