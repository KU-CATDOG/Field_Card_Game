using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transcendence : LevelUpSkill
{
    private int count = 1;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 23;
    protected override void levelUpRoutine()
    {
        GameManager.Instance.CharacterSelected.AddDrawCardRoutine(routine(), 0);
        return;
    }
    private IEnumerator routine()
    {
        while (true)
        {
            if (GameManager.Instance.CharacterSelected.drawCard is PaladinDefilement)
            {
                yield return GameManager.Instance.StartCoroutine(GameManager.Instance.CharacterSelected.DrawCard());
            }
            else
            {
                yield return null;
            }
        }
    }
    public override string GetText()
    {
        return "Transcendence\n 번뇌 카드를 뽑으면 카드를 한장을 뽑습니다.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        return null;
    }

}
