using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : Enemy
{
    protected void Start()
    {
        base.Start();
        Hp = MaxHp = 10;
        GiveExp = 0;
        TurnStartDraw = 0;
        crystalCount = maxCrystalCount = 0;
    }

    public override IEnumerator AfterBuff()
    {
        yield break;
    }

    public override IEnumerator AfterDraw()
    {
        yield break;
    }

    public override IEnumerator AwakeTurn()
    {
        yield break;
    }

    public override IEnumerator EnemyRoutine()
    {
        yield break;
    }

    public override bool PayTest(int cost, CostType type)
    {
        return true;
    }

    protected override IEnumerator enemyDieRoutine()
    {       
        Character goblin0 = Instantiate(GameManager.Instance.EnemyDict[57]);
        goblin0.position = new Coordinate(position.X + 1, position.Y + 1);
        Character pagan0 = Instantiate(GameManager.Instance.EnemyDict[58]);
        pagan0.position = new Coordinate(position.X + 3, position.Y + 1);

        Character goblin1 = Instantiate(GameManager.Instance.EnemyDict[57]);
        goblin1.position = new Coordinate(position.X + 1, position.Y - 1);
        Character pagan1 = Instantiate(GameManager.Instance.EnemyDict[58]);
        pagan1.position = new Coordinate(position.X + 3, position.Y - 1);

        GameManager.Instance.ogerCount += 1;

        if(GameManager.Instance.ogerCount == 4)
        {
            Character oger = Instantiate(GameManager.Instance.EnemyDict[92]);
            oger.position = new Coordinate(15, 15);
        }

        yield return new WaitForSeconds(0.5f);
    }

    protected override IEnumerator getDmg(int dmg)
    {
        yield break;
    }

    protected override void InitializeDeck()
    {

    }

    protected override IEnumerator payCost(int cost, CostType type)
    {
        yield break;
    }
}
