using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : Effect 
{
    public Stun(Character caster)
    {
        this.caster = caster;
    }
    public override void SetEffect(int value)
    {
        if (IsEnabled)
            return;
            
        Value = value;
        IsEnabled = true;
    }
    public override IEnumerator ApplyEffect()
    {
        if (!IsEnabled)
            yield break;
    }
}
