using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strengthen : Buff 
{
    public Strengthen(Character caster)
    {
        this.caster = caster;
    }
    public override void SetBuff(int value)
    {
        if (IsEnabled)
        {
            Value += value;
        }
        else
        {
            Value = value;
            IsEnabled = true;
            caster.TryHitAttackRoutine.Add(Effect());
            caster.TurnEndBuffHandler.Add(RemoveBuff());
        }
    }
    public override IEnumerator Effect()
    {
        while (IsEnabled)
        {
            caster.HitDmg += Value;
            yield return null;
        }
    }
}