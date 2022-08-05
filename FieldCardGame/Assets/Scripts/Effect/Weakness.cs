using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakness : Effect 
{
    public Weakness(Character caster)
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
            caster.AddTryHitAttackRoutine(ApplyEffect(), 0);
            caster.AddTurnEndDebuff(RemoveEffect(), 0);
        }
    }
    public override IEnumerator ApplyEffect()
    {
        while (IsEnabled)
        {
            caster.HitDmg -= 5;
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
        caster.RemoveTryHitAttackRoutineByIdx(FindRoutineIndex(RemoveEffect(), caster.TryHitAttackRoutine));
        caster.RemoveTurnEndDebuffByIdx(FindRoutineIndex(RemoveEffect(), caster.TurnEndDebuffHandler));
        Value = 0;
        IsEnabled = false;
    }
}
