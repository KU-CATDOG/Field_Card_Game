using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackCard
{
    public List<int> Damage{get;}
    public void SetDmg(int value);
}
