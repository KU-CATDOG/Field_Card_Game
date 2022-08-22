using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scintillation : LevelUpSkill
{
    private int count = 1;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 19;
    protected override void levelUpRoutine()
    {
        PaladinDefilement.Scintillation = true;
        return;
    }
    public override string GetText()
    {
        return "Scintillation \n 번뇌 카드가 이제 3의 사거리와 3의 데미지를 가집니다.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        if (nextSkillList == null)
        {
            nextSkillList = new();
            nextSkillList.Add(new Transcendence());
        }
        return nextSkillList;
    }
}
