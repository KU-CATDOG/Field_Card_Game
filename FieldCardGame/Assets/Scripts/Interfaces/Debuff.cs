using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuff
{
    protected Character caster;
    public int Value {get; protected set;} = 0;
    public int Priority {get; protected set;} = 0;
    public bool IsEnabled {get; protected set;} = false;
    public abstract void SetDebuff(int value);
    public virtual IEnumerator RemoveDebuff()
    {
        Value = 0;
        IsEnabled = false;
        yield return null;
    }
    public abstract IEnumerator Effect();
}