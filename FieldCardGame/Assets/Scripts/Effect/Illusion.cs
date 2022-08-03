using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Illusion : Effect 
{
    public Illusion(Character caster)
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
            caster.AddTryGetDmgRoutine(ApplyEffect(), 5);
            caster.AddStartBuff(RemoveEffect(), 0);
        }
    }
    public override IEnumerator ApplyEffect()
    {
        while (IsEnabled)
        {
            caster.StartCoroutine(caster.GiveHeal(caster, caster.Dmg, true));
            caster.Dmg = 0;
            caster.GetDmgInterrupted = true;
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
}
