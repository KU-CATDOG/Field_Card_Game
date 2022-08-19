using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffHandler
{
    private Character caster;
    private Dictionary<DebuffType, Effect> debuffDict = new Dictionary<DebuffType, Effect>();
    public IReadOnlyDictionary<DebuffType, Effect> DebuffDict {get {return debuffDict;}}
    public DebuffHandler(Character caster)
    {
        this.caster = caster;

        Effect stun = new Stun(caster);

        debuffDict.Add(DebuffType.Stun, stun);
    }

    // For Debug
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
