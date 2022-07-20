using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Will : Effect
{
    public Will(Character caster)
    {
        this.caster = caster;
    }

    private bool token = false;
    public override void SetEffect(int value)
    {
        if (IsEnabled)
            return;

        IsEnabled = true;
        token = true;

        // 우선순위 Shield 보다 높게 줘야함
        caster.TurnEndBuffHandler.Add(ApplyEffect());
        caster.StartBuffHandler.Add(ApplyEffect());
    }
    public override IEnumerator ApplyEffect()
    {
        if (token)
        {
            Value = caster.BuffHandler.BuffDict[BuffType.Shield].Value;
            Debug.Log(Value);
            token = false;
        }
        else
        {
            caster.BuffHandler.BuffDict[BuffType.Shield].SetEffect(Value);
            Debug.Log(Value);
            Value = 0;
            IsEnabled = false;
        }
        yield return null;
    }
}
