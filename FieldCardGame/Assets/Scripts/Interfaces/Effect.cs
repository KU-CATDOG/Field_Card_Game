using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    protected Character caster;
    public int Value {get; protected set;} = 0;
    public int Priority {get; protected set;} = 0;
    public bool IsEnabled {get; protected set;} = false;
    public abstract void SetEffect(int value);
    public virtual IEnumerator RemoveEffect()
    {
        Value = 0;
        IsEnabled = false;
        yield return null;
    }
    public virtual void  ForceRemoveEffect()
    {
        Value = 0;
        IsEnabled = false;
    }
    protected virtual int FindRoutineIndex(IEnumerator routine, IReadOnlyList<BuffRoutine> handler)
    {
        for (int i = 0; i < handler.Count; i++)
        {
            if (handler[i].Routine.Equals(routine))
                return i;
        }
        return -1;
    }
    public abstract IEnumerator ApplyEffect();
}