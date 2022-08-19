using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProtectCard
{
    public List<int> ProtectAmount{ get; }
    public void SetProtectAmount(int val);
}
