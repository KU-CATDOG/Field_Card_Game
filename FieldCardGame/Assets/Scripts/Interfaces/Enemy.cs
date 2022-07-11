using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.EnemyList.Add(this);
    }
    public abstract IEnumerator EnemyRoutine();
}
