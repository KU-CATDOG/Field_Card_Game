using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : Enemy
{
    protected override void Start()
    {
        base.Start();
        Hp = MaxHp = 15;
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

        if (GameManager.Instance.EnemyList.Count == 1 && (tiles = HandCard[atkIsFst ? 0 : 1].GetAvailableTile(position)).Count > 0)
        {
            int minDist = int.MaxValue;
            Debug.Log(tiles.Count);
            Coordinate toATK = tiles[0];

            foreach (var i in tiles)
            {
                foreach (var j in GameManager.Instance.Allies)
                {
                    if (minDist > Coordinate.Distance(i, j.position))
                    {
                        minDist = Coordinate.Distance(i, j.position);
                        toATK = i;
                    }
                }
            }

            crystalCount -= 1;
            yield return StartCoroutine(CardUse(toATK, atkIsFst ? 0 : 1));
        }
        else
        {
            if ((tiles = HandCard[atkIsFst ? 1 : 0].GetAvailableTile(position)).Count > 0)
            {

                int minDist = int.MaxValue;
                Coordinate toATK = tiles[0];

                foreach (var i in tiles)
                {
                    foreach (var j in GameManager.Instance.Allies)
                    {
                        if (i.X == j.position.X && i.Y == j.position.Y && j.EffectHandler.DebuffDict.GetValueOrDefault(DebuffType.Weakness).IsEnabled == false && minDist > Coordinate.Distance(i, j.position))
                        {
                            minDist = Coordinate.Distance(i, j.position);
                            toATK = i;
                        }
                    }
                }

                if (minDist != int.MaxValue)
                {
                    crystalCount -= 1;
                    yield return StartCoroutine(CardUse(toATK, atkIsFst ? 1 : 0));
                    yield break;
                }
            }

            if ((tiles = HandCard[atkIsFst ? 0 : 1].GetAvailableTile(position)).Count > 0)
            {
                int minDist = int.MaxValue;
                
                Coordinate toATK = tiles[0];

                foreach (var i in tiles)
                {
                    foreach (var j in GameManager.Instance.Allies)
                    {
                        if (minDist > Coordinate.Distance(i, j.position))
                        {
                            minDist = Coordinate.Distance(i, j.position);
                            toATK = i;
                        }
                    }
                }

                crystalCount -= 1;
                yield return StartCoroutine(CardUse(toATK, atkIsFst ? 0 : 1));
            }
            
        }

        yield break;
        
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
        a.SetRange(4);
        a._dmg = 10;
        CardPile.Add(a);

        TrashEnemyWeakness b = new TrashEnemyWeakness();
        b.SetCost(1);
        b.SetRange(3);
        b._dmg = 2;
        CardPile.Add(b);
    }

    protected override IEnumerator payCost(int cost, CostType type)
    {
        yield break;
    }
}
