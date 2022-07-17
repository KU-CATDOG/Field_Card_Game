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
        
        buffDict.Add(BuffType.Strengthen, strengthen);
        buffDict.Add(BuffType.Shield, shield);
    }
}