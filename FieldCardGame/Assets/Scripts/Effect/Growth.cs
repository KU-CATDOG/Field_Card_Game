using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growth : Effect 
{
    public Growth(Character caster)
    {
        this.caster = caster;
    }
    public override void SetEffect(int value)
    {
        if (IsEnabled)
            return;
            
        Value = value;
        IsEnabled = true;
        caster.AddStartBuff(RemoveEffect(), 1);
    }
    public override IEnumerator ApplyEffect()
    {
        yield return null;
    }
    public override IEnumerator RemoveEffect()
    {
        caster.StartCoroutine(caster.GiveHeal(caster, Value, true));
        IsEnabled = false;
        yield return null;
    }
    public override void ForceRemoveEffect()
    {
        if (!IsEnabled)
            return;
        caster.RemoveStartBuffByIdx(FindRoutineIndex(RemoveEffect(), caster.StartBuffHandler));
        Value = 0;
        IsEnabled = false;
    }
}
