using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler
{
    private Character caster;
    private Dictionary<BuffType, Effect> buffDict = new Dictionary<BuffType, Effect>();
    public IReadOnlyDictionary<BuffType, Effect> BuffDict {get {return buffDict;}}
    private Dictionary<DebuffType, Effect> debuffDict = new Dictionary<DebuffType, Effect>();
    public IReadOnlyDictionary<DebuffType, Effect> DebuffDict {get {return debuffDict;}}
    public EffectHandler(Character caster)
    {
        this.caster = caster;

        Effect strengthen = new Strengthen(caster);
        Effect shield = new Shield(caster);
        Effect will = new Will(caster);
        Effect illusion = new Illusion(caster);
        Effect regeneration = new Regeneration(caster);
        Effect growth = new Growth(caster);
        Effect heal = new Heal(caster);
        Effect debug = new DebugEffect(caster);
        
        buffDict.Add(BuffType.Strengthen, strengthen);
        buffDict.Add(BuffType.Shield, shield);
        buffDict.Add(BuffType.Will, will);
        buffDict.Add(BuffType.Illusion, illusion);
        buffDict.Add(BuffType.Regeneration, regeneration);
        buffDict.Add(BuffType.Growth, growth);
        buffDict.Add(BuffType.Heal, heal);
        buffDict.Add(BuffType.Debug, debug);

        Effect stun = new Stun(caster);
        Effect weakness = new Weakness(caster);
        Effect poison = new Poison(caster);
        Effect fragility = new Fragility(caster);
        Effect rooted = new Rooted(caster);
        Effect vampire = new Vampire(caster);

        debuffDict.Add(DebuffType.Stun, stun);
        debuffDict.Add(DebuffType.Weakness, weakness);
        debuffDict.Add(DebuffType.Poison, poison);
        debuffDict.Add(DebuffType.Fragility, fragility);
        debuffDict.Add(DebuffType.Vampire, vampire);
    }

    // For Debug
    public IReadOnlyList<BuffType> GetEnabledBuff()
    {
        List<BuffType> tmp = new List<BuffType>();
        foreach(var buff in BuffDict)
        {
            if (buff.Value.IsEnabled)
                tmp.Add(buff.Key);
        }
        return tmp;
    }

    public IReadOnlyList<DebuffType> GetEnabledDebuff()
    {
        List<DebuffType> tmp = new List<DebuffType>();
        foreach(var debuff in DebuffDict)
        {
            if (debuff.Value.IsEnabled)
                tmp.Add(debuff.Key);
        }
        return tmp;
    }
}
