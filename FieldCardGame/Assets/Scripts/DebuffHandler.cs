using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffHandler
{
    private Character caster;
    private Dictionary<DebuffType, Debuff> debuffDict = new Dictionary<DebuffType, Debuff>();
    public IReadOnlyDictionary<DebuffType, Debuff> DebuffDict {get {return debuffDict;}}
    public DebuffHandler(Character caster)
    {
        this.caster = caster;
    }
}
