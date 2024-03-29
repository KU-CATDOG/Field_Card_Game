using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{ 
    protected override void Start()
    {
        base.Start();
        Hp = MaxHp = 30;
        GiveExp = 0;
        TurnStartDraw = 2;
        crystalCount = maxCrystalCount = 1;
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
        bool atkIsFst = false;

        if (HandCard[0] is TrashEnemyAttack)
        {
            atkIsFst = true;
        }
        else
        {
            atkIsFst = false;
        }

        List<Coordinate> tiles;

        if((tiles = HandCard[atkIsFst ? 0 : 1].GetAvailableTile(position)).Count > 0)
        {
            Coordinate toATK = tiles[0];
            int minDist = int.MaxValue;

            foreach (var i in tiles)
            {
                foreach (var j in GameManager.Instance.Allies)
                {
                    Coordinate pos = j.position;
                    if (i.X == pos.X && i.Y == pos.Y && minDist > Coordinate.Distance(i, pos))
                    {
                        minDist = Coordinate.Distance(i, pos);
                        toATK = i;
                    }
                }
            }

            crystalCount -= 1;
            yield return StartCoroutine(CardUse(toATK, atkIsFst ? 0 : 1));
            yield break;
        }
        else
        {
            tiles = HandCard[atkIsFst ? 1 : 0].GetAvailableTile(position);
            Coordinate toGo = tiles[0];
            int minDist = int.MaxValue;

            foreach (var i in tiles)
            {
                foreach (var j in GameManager.Instance.Allies)
                {
                    Coordinate pos = j.position;
                    if (minDist > Coordinate.Distance(i, pos))
                    {
                        minDist = Coordinate.Distance(i, pos);
                        toGo = i;
                    }
                }
            }

            crystalCount -= 1;
            yield return StartCoroutine(CardUse(toGo, atkIsFst ? 1 : 0));
            yield break;
        }
    }

    public override bool PayTest(int cost, CostType type)
    {
        return true;
    }

    protected override IEnumerator enemyDieRoutine()
    {
        yield break;
    }

    protected override IEnumerator getDmg(int dmg)
    {
        yield break;
    }

    protected override void InitializeDeck()
    {
        TrashEnemyAttack a = new TrashEnemyAttack();
        a.SetCost(1);
        a.SetRange(1);
        a._dmg = 15;
        CardPile.Add(a);

        EnemyMove m = new EnemyMove();
        m.Disposable = false;
        m.SetCost(1);
        m._range = 4;
        CardPile.Add(m);
    }

    protected override IEnumerator payCost(int cost, CostType type)
    {
        yield break;
    }
}

