using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Effect 
{
    private Character player;
    private bool isTargetAlive = false;
    public Vampire(Character caster)
    {
        this.caster = caster;
    }
    public override void SetEffect(int value)
    {
        if (IsEnabled)
        {
            Value += value;
        }
        else
        {
            Value = value;
            IsEnabled = true;
            isTargetAlive = true;
            player = GameManager.Instance.CurPlayer;
            player.AddTurnEndDebuff(ApplyEffect(), 5);
            player.AddStartDebuff(RemoveEffect(), 0);
        }
    }
    public override IEnumerator ApplyEffect()
    {
        while (IsEnabled && isTargetAlive)
        {
            caster.StartCoroutine(caster.GetDmg(caster, Value));
            if (caster.IsDie)
                isTargetAlive = false;
            player.StartCoroutine(player.GiveHeal(player, Value, true));
            yield return null;
        }
    }
    public override IEnumerator RemoveEffect()
    {
        while(true)
        {
            if ((--Value) > 0) 
                yield return null;
            else
                break;
        }
        IsEnabled = false;
        yield return null;
    }
    public override void ForceRemoveEffect()
    {
        if (!IsEnabled)
            return;
        caster.RemoveForceTurnEndDebuffByIdx(FindRoutineIndex(RemoveEffect(), caster.ForceTurnEndDebuffHandler));
        caster.RemoveStartDebuffByIdx(FindRoutineIndex(RemoveEffect(), caster.StartDebuffHandler));
        Value = 0;
        IsEnabled = false;
    }
}
