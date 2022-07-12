using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    private Character caster;
    public Buff(Character caster)
    {
        this.caster = caster;
    }
    public void Strengthen(int value)
    {
        if (!isStrengthenEnd)
        {
            strengthenValue += value;
        }
        else
        {
            strengthenValue = value;
            caster.TryHitAttackRoutine.Add(strengthen());
            caster.TurnEndBuffHandler.Add(strengthenEnd());
        }
    }
    bool isStrengthenEnd = true;
    int strengthenValue;
    public IEnumerator strengthen()
    {
        isStrengthenEnd = false;
        while(!isStrengthenEnd)
        {
            caster.HitDmg += strengthenValue;
            yield return null;
        }
    }
    public IEnumerator strengthenEnd()
    {
        isStrengthenEnd = true;
        yield break;
    }
}