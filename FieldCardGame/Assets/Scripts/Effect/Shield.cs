using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Effect 
{
    public Shield(Character caster)
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
            //fixme
            caster.AddTryGetDmgRoutine(ApplyEffect(), 0);
            caster.AddStartBuff(RemoveEffect(), 1);
            //fixme
        }
    }
    public override IEnumerator ApplyEffect()
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