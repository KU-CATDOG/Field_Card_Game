using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragility : Effect 
{
    public Fragility(Character caster)
    {
        this.caster = caster;
    }
    public override void SetEffect(int value)
    {
        if (IsEnabled)
            return;
            
        IsEnabled = true;
        caster.AddTryGetDmgRoutine(ApplyEffect(), 5);
        caster.AddStartBuff(RemoveEffect(), 0);
    }
    public override IEnumerator ApplyEffect()
    {
        while (IsEnabled)
        {
            caster.Dmg *= 2;
            yield return null;
        }
    }
    public override void ForceRemoveEffect()
    {
        if (!IsEnabled)
            return;
        caster.RemoveTryGetDmgRoutineByIdx(FindRoutineIndex(RemoveEffect(), caster.TryGetDmgRoutine));
        caster.RemoveStartBuffByIdx(FindRoutineIndex(RemoveEffect(), caster.StartBuffHandler));
        IsEnabled = false;
    }
}
