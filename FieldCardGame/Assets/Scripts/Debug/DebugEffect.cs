using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEffect : Effect
{
    public DebugEffect(Character caster)
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
            caster.AddTurnEndDebuff(RemoveEffect(), 0);
            //
        }
    }
    public override IEnumerator ApplyEffect()
    {
        while (IsEnabled)
        {
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