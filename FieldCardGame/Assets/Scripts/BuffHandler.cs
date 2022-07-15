using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHandler
{
    private Character caster;
    private Dictionary<BuffType, Buff> buffDict = new Dictionary<BuffType, Buff>();
    public IReadOnlyDictionary<BuffType, Buff> BuffDict {get {return buffDict;}}
    public BuffHandler(Character caster)
    {
        this.caster = caster;

        Buff strengthen = new Strengthen(caster);
        Buff shield = new Shield(caster);
        
        buffDict.Add(BuffType.Strengthen, strengthen);
        buffDict.Add(BuffType.Shield, shield);
    }
}