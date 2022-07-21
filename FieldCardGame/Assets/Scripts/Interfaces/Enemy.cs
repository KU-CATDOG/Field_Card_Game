using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    [SerializeField]
    private int Id;
    public int ID
    {
        get
        {
            return Id;
        }
    }
    public int GiveExp { get; set; }
    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.EnemyList.Add(this);
    }
    protected override IEnumerator dieRoutine()
    {
        if(KilledBy == GameManager.Instance.Allies[0])
        {
            yield return (KilledBy as Player).GainExp(GiveExp);
        }
    }
    public abstract IEnumerator EnemyRoutine();
}
