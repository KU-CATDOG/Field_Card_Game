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
            caster.TryHitAttackRoutine.Add(ApplyEffect());
            caster.TurnEndBuffHandler.Add(RemoveEffect());
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
}