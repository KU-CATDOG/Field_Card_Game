using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regeneration : Effect 
{
    public Regeneration(Character caster)
    {
        this.caster = caster;
    }
    public override void SetEffect(int value)
    {
        if (IsEnabled)
            return;
            
        Value = value;
        IsEnabled = true;
        caster.AddCardUseRoutine(ApplyEffect(), 0);
        caster.AddTurnEndBuff(RemoveEffect(), 0);
    }
    public override IEnumerator ApplyEffect()
    {
        while (IsEnabled)
        {
            caster.StartCoroutine(caster.GiveHeal(caster, Value, true));
            yield return null;
        }
    }
}
