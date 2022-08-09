using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Effect 
{
    public Heal(Character caster)
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
            IsEnabled = true;
            Value = value;
            caster.AddStartBuff(ApplyEffect(), 0);
            caster.AddStartBuff(RemoveEffect(), -1);
        }
    }
    public override IEnumerator ApplyEffect()
    {
        while (IsEnabled)
        {
            caster.StartCoroutine(caster.GiveHeal(caster, Value, true));
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
        caster.RemoveStartBuffByIdx(FindRoutineIndex(ApplyEffect(), caster.StartBuffHandler));
        caster.RemoveStartBuffByIdx(FindRoutineIndex(RemoveEffect(), caster.StartBuffHandler));
        Value = 0;
        IsEnabled = false;
    }
}
