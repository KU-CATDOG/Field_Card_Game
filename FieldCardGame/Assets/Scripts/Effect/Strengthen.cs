using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strengthen : Effect 
{
    public Strengthen(Character caster)
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
            caster.AddTurnEndBuff(RemoveEffect(), 0);
        }
    }
    public override IEnumerator ApplyEffect()
    {
        while (IsEnabled)
        {
            caster.HitDmg += Value;
            yield return null;
        }
    }
    public override void ForceRemoveEffect()
    {
        if (!IsEnabled)
            return;
        caster.RemoveTryHitAttackRoutineByIdx(FindRoutineIndex(ApplyEffect(), caster.TryHitAttackRoutine));
        caster.RemoveTurnEndBuffByIdx(FindRoutineIndex(RemoveEffect(), caster.TurnEndBuffHandler));
        Value = 0;
        IsEnabled = false;
    }
}