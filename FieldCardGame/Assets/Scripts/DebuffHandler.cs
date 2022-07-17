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
    }
}
