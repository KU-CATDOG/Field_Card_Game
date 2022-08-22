using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivineStigma : Effect
{
    public static bool Mastery { get; set; } = false;
    public static bool Completion { get; set; } = false;
    public static bool Punish { get; set; } = false;
    public static bool Cycle { get; set; } = false;
    public static int StigamDamage{get; set;} = 3;
    public DivineStigma(Character caster)
    {
        this.caster = caster;
    }
    public override void SetEffect(int value)
    {
        if (Mastery)
        {
            value++;
        }
        if (Punish)
        {
            caster.EffectHandler.DebuffDict[DebuffType.Weakness].SetEffect(1);
        }
        if (Completion)
        {
            GameManager.Instance.StartCoroutine(caster.GetDmg(GameManager.Instance.CharacterSelected, 5, false));
        }
        if (IsEnabled)
        {
            Value+=value;
            return;
        }
        IsEnabled = true;
        Value = value;
        caster.AddTryGetDmgRoutine(ApplyEffect(), -1);
    }
    public override IEnumerator ApplyEffect()
    {
        while (IsEnabled)
        {
            caster.Dmg += StigamDamage;
            if (!Cycle)
            {
                if (--Value == 0)
                {
                    RemoveEffect().MoveNext();
                    yield break;
                }
            }
            yield return null;
        }
    }
    public override void ForceRemoveEffect()
    {
        if (!IsEnabled)
            return;
        caster.RemoveTryGetDmgRoutineByIdx(FindRoutineIndex(ApplyEffect(), caster.TryGetDmgRoutine));
        IsEnabled = false;
    }

}
