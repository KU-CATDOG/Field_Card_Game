using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivineStigma : Effect
{
    public static int StigamDamage{get; set;} = 3;
    public DivineStigma(Character caster)
    {
        this.caster = caster;
    }
    public override void SetEffect(int value)
    {
        if (IsEnabled)
        {
            Value+=value;
            return;
        }
        IsEnabled = true;
        Value = value;
        caster.AddTryGetDmgRoutine(ApplyEffect(), -1);
    }
    public override IEnumerator ApplyEffect()
    {
        while (IsEnabled)
        {
            caster.Dmg += StigamDamage;
            if(--Value == 0)
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
