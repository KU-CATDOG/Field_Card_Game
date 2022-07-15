using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Buff
{
    public Shield(Character caster)
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
            caster.TryGetDmgRoutine.Add(Effect());
            caster.TurnEndBuffHandler.Add(RemoveBuff());
        }
    }
    public override IEnumerator Effect()
    {
        while (IsEnabled)
        {
                int tmp = Value - caster.Dmg;
                caster.Dmg = Mathf.Min(0, tmp) * (-1);
                Value = Mathf.Max(0, tmp);
                IsEnabled = Value != 0;
                caster.GetDmgInterrupted = caster.Dmg == 0;
            yield return null;
        }
    }
}