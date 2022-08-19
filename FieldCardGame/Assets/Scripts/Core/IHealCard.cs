using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealCard
{
    public List<int> HealAmounts{get;}
    public void SetHealAmount(int val);
}
