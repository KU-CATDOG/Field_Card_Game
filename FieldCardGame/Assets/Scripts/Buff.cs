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

    public void Shield(int value)
    {
        if (!isShieldEnd)
        {
            shieldValue += value;
            isShieldEnd = shieldValue <= 0;
        }
        else
        {
            shieldValue = value;
            caster.TryGetDmgRoutine.Add(shield());
            caster.TurnEndBuffHandler.Add(shieldEnd());
        }
    }
    bool isShieldEnd = true;
    int shieldValue;

    public int ShieldValue => shieldValue;
    
    public IEnumerator shield()
    {
        isShieldEnd = false;
        while(!isShieldEnd)
        {
            shieldValue -= caster.Dmg;
            caster.Dmg = Mathf.Min(0, shieldValue) * (-1);
            isShieldEnd = shieldValue <= 0;
            caster.GetDmgInterrupted = caster.Dmg == 0;
            yield return null;
        }
    }

    public IEnumerator shieldEnd()
    {
        isShieldEnd = true;
        yield break;
    }

    public void Protect(int value)
    {
        if(!isProtectEnd)
        {
            protectValue += value;
        }
        else
        {
            protectValue = value;
            caster.TryGetDmgRoutine.Add(protect());
            caster.TurnEndBuffHandler.Add(protectEnd());
        }
    }
    bool isProtectEnd = true;
    int protectValue;
    public IEnumerator protect()
    {
        isProtectEnd = false;
        
        while(!isProtectEnd)
        {
            if(protectValue == 0)
                isProtectEnd = true;

            if(protectValue >= caster.Dmg)
            {
                protectValue -= caster.Dmg;
                caster.Dmg = 0;
                caster.GetDmgInterrupted = true;
            }
            else
            {
                caster.Dmg -= protectValue;
                protectValue = 0;
            }
            
            yield return null;
        }
    }
    public IEnumerator protectEnd()
    {
        isProtectEnd = true;
        yield break;
    }
}
