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
        caster.AddTurnEndBuff(ApplyEffect(), 1);
        caster.AddStartBuff(ApplyEffect(), 0);
    }
    public override IEnumerator ApplyEffect()
    {
        if (!IsEnabled)
            yield break;

        if (token)
        {
            Value = caster.BuffHandler.BuffDict[BuffType.Shield].Value;
            Debug.Log(Value);
            token = false;
        }
        else
        {
            if (Value != 0)
                caster.BuffHandler.BuffDict[BuffType.Shield].SetEffect(Value);
            Debug.Log(Value);
            Value = 0;
            IsEnabled = false;
        }
        yield return null;
    }
}
