using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strengthen : Buff
{
    public Strengthen(Character caster)
    {
        this.caster = caster;
    }
    public override void SetBuff(int amount, int duration)
    {
        if (IsEnabled)
        {
            Value += amount;
        }
        else
        {
            Value = amount;
            MaxDuration = duration;
            Duration = 1;
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