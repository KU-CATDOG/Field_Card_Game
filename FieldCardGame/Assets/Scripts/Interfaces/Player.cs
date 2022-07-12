using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Character
{
    public int Level { get; set; } = 1;
    public int Exp { get; set; }
    public int Gold { get; set; } = 0;
    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.Allies.Add(this);
    }
}
