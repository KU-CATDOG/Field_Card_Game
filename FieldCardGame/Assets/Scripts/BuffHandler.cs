using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHandler
{
    private Character caster;
    private Dictionary<BuffType, Buff> buffDict;
    public IReadOnlyDictionary<BuffType, Buff> BuffDict {get {return buffDict;}}
    public BuffHandler(Character caster)
    {
        this.caster = caster;

        Buff buff = new Strengthen(caster);
        buffDict.Add(BuffType.strengthen, buff);
    }
}

public class Strengthen : Buff
{
    public void SetBuff(int amount, int duration)
    {
        if (IsEnabled)
        {
            Value += amount;
        }
        else
        {
            strengthenValue = value;
            MaxDuration = duration;
            Duration = 1;
            IsEnabled = true;
            caster.TryHitAttackRoutine.Add(Effect());
            caster.TurnEndBuffHandler.Add(EffectEnd());
        }
    }
    public IEnumerator Effect()
    {
        while (IsEnabled)
        {
            caster.HitDmg += Value;
            yield return null;
        }
    }
    public IEnumerator EffectEnd()
    {
        if (MaxDuration > Duration) 
        {
            Duration++;
            yield break;
        }
        IsEnabled = false;
        yield break;
    }
}
