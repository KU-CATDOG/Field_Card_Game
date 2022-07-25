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
        caster.GiveHeal(caster, Value);
        IsEnabled = false;
        yield return null;
    }
}
