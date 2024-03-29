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
            
        IsEnabled = true;
        caster.AddForceTurnEndDebuff(ApplyEffect(), 0);
    }
    public override IEnumerator ApplyEffect()
    {
        IsEnabled = false;
        yield break;
    }
    public override void ForceRemoveEffect()
    {
        if (!IsEnabled)
            return;
        caster.RemoveForceTurnEndDebuffByIdx(FindRoutineIndex(ApplyEffect(), caster.ForceTurnEndDebuffHandler));
        IsEnabled = false;
    }
}
