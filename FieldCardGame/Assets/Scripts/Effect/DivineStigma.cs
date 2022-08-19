using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivineStigma : Effect
{
    int count;
    public static int StigamDamage{get; set;}
    public DivineStigma(Character caster)
    {
        this.caster = caster;
    }
    public override void SetEffect(int value)
    {
        if (IsEnabled)
        {
            count++;
            return;
        }
        IsEnabled = true;
        caster.AddTryGetDmgRoutine(ApplyEffect(), -1);
    }
    public override IEnumerator ApplyEffect()
    {
        while (IsEnabled)
        {
            caster.Dmg += StigamDamage;
            if(--count == 0)
            {
                RemoveEffect().MoveNext();
                yield break;
            }
            yield return null;
        }
    }
    public override void ForceRemoveEffect()
    {
        if (!IsEnabled)
            return;
        caster.RemoveTryGetDmgRoutineByIdx(FindRoutineIndex(ApplyEffect(), caster.TryGetDmgRoutine));
        IsEnabled = false;
    }

}
