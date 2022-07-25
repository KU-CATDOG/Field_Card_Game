using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viper : Enemy
{ 
    protected void Start()
    {
        base.Start();
        Hp = MaxHp = 45;
        GiveExp = 0;
        TurnStartDraw = 2;
        crystalCount = maxCrystalCount = 2;
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

        while (crystalCount > 0)
        {
            Debug.Log(crystalCount);
            if (HandCard[0] is AttackNDebuff)
            {
                atkIsFst = true;
            }
            else
            {
                atkIsFst = false;
            }

            List<Coordinate> tiles;

            if ((tiles = HandCard[atkIsFst ? 0 : 1].GetAvailableTile(position)).Count > 0)
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

                crystalCount -= HandCard[atkIsFst ? 0 : 1].GetCost();
                yield return StartCoroutine(CardUse(toATK, atkIsFst ? 0 : 1));
                break;
            }
            else if ((tiles = HandCard[atkIsFst ? 1 : 0].GetAvailableTile(position)).Count > 0)
            {
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

                crystalCount -= HandCard[atkIsFst ? 1 : 0].GetCost();
                DropInterrupted = true;
                yield return StartCoroutine(CardUse(toGo, atkIsFst ? 1 : 0));
                DropInterrupted = false;
            }
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
        AttackNDebuff a = new AttackNDebuff();
        a.SetCost(1);
        a.SetRange(1);
        a._dmg = 8;
        a._debuffType = DebuffType.Poison;
        a._debuffValue = 4;
        CardPile.Add(a);

        PaladinMove b = new PaladinMove();
        b.Disposable = false;
        b.SetCost(1);
        b.SetRange(3);
        CardPile.Add(b);
    }

    protected override IEnumerator payCost(int cost, CostType type)
    {
        yield break;
    }
}

