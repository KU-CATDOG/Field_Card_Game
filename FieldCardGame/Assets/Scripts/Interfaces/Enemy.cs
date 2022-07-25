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
    //fixme
    protected int Gold { get; set; } = 10;
    //fixme
    protected int rewardTier { get; set; } = 1;

    protected int crystalCount;

    protected void Awake()
    {
        base.Awake();
        GameManager.Instance.EnemyList.Add(this);
    }
    protected abstract IEnumerator enemyDieRoutine();
    protected override IEnumerator dieRoutine()
    {
        yield return enemyDieRoutine();
        if(KilledBy is Player)
        {
            yield return (GameManager.Instance.CharacterSelected as Player).GainExp(GiveExp);
            DropItem();
        }
    }
    //fixme
    private void DropItem()
    {
        DropCardObject dropCardObj = Instantiate(GameManager.Instance.DropCardObject);
        dropCardObj.position = position;
        GameManager.Instance.Map[position.X, position.Y].AddOnCharacterEnterRoutine(dropCardObj.GiveReward(), 0);
    }
    public abstract IEnumerator EnemyRoutine();
}
