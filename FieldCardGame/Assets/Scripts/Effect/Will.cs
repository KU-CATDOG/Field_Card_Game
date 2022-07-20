using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Will : Effect
{
    public Will(Character caster)
    {
        this.caster = caster;
    }
    public override void SetEffect(int value)
    {
        if (!caster.BuffHandler.BuffDict[BuffType.Shield].IsEnabled)
            return;

        caster.TurnEndBuffHandler.Add(ApplyEffect());
        caster.StartBuffHandler.Add(ApplyEffect());
    }
    public override IEnumerator ApplyEffect()
    {
        Value = caster.BuffHandler.BuffDict[BuffType.Shield].Value;
        yield return null;
        caster.BuffHandler.BuffDict[BuffType.Shield].SetEffect(Value);
        Value = 0;
        yield return null;
    }
}
