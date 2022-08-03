using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : Effect 
{
    public Poison(Character caster)
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
            caster.AddTurnEndDebuff(ApplyEffect(), 0);
            caster.AddStartDebuff(RemoveEffect(), 0);
        }
    }
    public override IEnumerator ApplyEffect()
    {
        while (IsEnabled)
        {
            caster.StartCoroutine(caster.GetDmg(caster, Value));
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
        base.ForceRemoveEffect();
        caster.RemoveStartDebuffByIdx(FindRoutineIndex(RemoveEffect(), caster.StartDebuffHandler));
    }
}
