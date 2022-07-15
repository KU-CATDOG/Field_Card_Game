using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff
{
    private Character caster;
    public void Buff(Character caster)
    {
        this.caster = caster;
    }
    public int Value {public get; private set;} = 0;
    public int Duration {public get; private set;} = 0;
    public int MaxDuration {public get; private set;} = 0;
    public bool IsEnabled {public get; private set;} = 0;
    public void SetBuff(int amount, int duration);
    public IEnumerator Effect();
    public IEnumerator EffectEnd();
}