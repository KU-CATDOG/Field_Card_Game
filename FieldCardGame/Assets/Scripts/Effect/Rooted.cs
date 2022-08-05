using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooted : Effect 
{
    public Rooted(Character caster)
    {
        this.caster = caster;
    }
    public override void SetEffect(int value)
    {
        if (IsEnabled)
            return;
            
        IsEnabled = true;

        caster.AddTryMoveRoutine(ApplyEffect(), 0);
        caster.AddTurnEndDebuff(RemoveEffect(), 0);
    }
    public override IEnumerator ApplyEffect()
    {
        while (IsEnabled)
        {
            caster.MoveInterrupted = true;
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
        caster.MoveInterrupted = false;
        yield return null;
    }
    public override void ForceRemoveEffect()
    {
        if (!IsEnabled)
            return;
        caster.RemoveTryMoveRoutineByIdx(FindRoutineIndex(RemoveEffect(), caster.TryMoveRoutine));
        caster.RemoveTurnEndDebuffByIdx(FindRoutineIndex(RemoveEffect(), caster.TurnEndDebuffHandler));
        Value = 0;
        IsEnabled = false;
    }
}
