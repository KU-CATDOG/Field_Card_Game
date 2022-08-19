using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHandler
{
    private Character caster;
    private Dictionary<BuffType, Effect> buffDict = new Dictionary<BuffType, Effect>();
    public IReadOnlyDictionary<BuffType, Effect> BuffDict {get {return buffDict;}}
    public BuffHandler(Character caster)
    {
        this.caster = caster;

        Effect strengthen = new Strengthen(caster);
        Effect shield = new Shield(caster);
        Effect will = new Will(caster);
        Effect debug = new DebugEffect(caster);
        
        buffDict.Add(BuffType.Strengthen, strengthen);
        buffDict.Add(BuffType.Shield, shield);
        buffDict.Add(BuffType.Will, will);
        buffDict.Add(BuffType.Debug, debug);
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
}