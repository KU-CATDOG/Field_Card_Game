using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Character
{
    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.Allies.Add(this);
    }
}
