using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideBomber : Enemy
{ 
    protected void Start()
    {
        base.Start();
        Hp = MaxHp = 20;
        GiveExp = 3;
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
    private int[] findAllCardIDX()
    {
        int[] ret = new int[] {FindCardIDX(typeof(TrashEnemyAttack)), FindCardIDX(typeof(EnemyMove))};

        return ret;
    }
    public override IEnumerator EnemyRoutine()
    {
        int[] cardIDX;
        
        while (crystalCount > 0 && !IsDie)
        {
            cardIDX = findAllCardIDX();
            List<Coordinate> tiles;

            if (cardIDX[0] != -1 && (tiles = HandCard[cardIDX[0]].GetAvailableTile(position)).Count > 0)
            {
                Coordinate toATK = tiles[0];

                foreach (var i in tiles)
                {
                    foreach (var j in GameManager.Instance.Allies)
                    {
                        Coordinate pos = j.position;
                        if (i.X == pos.X && i.Y == pos.Y )
                        {
                            toATK = i;
                        }
                    }
                }

                crystalCount -= HandCard[cardIDX[0]].GetCost();
                yield return StartCoroutine(CardUse(toATK, cardIDX[0]));
                yield return GameManager.Instance.StartCoroutine(this.HitAttack(GameManager.Instance.Map[position.X, position.Y].CharacterOnTile, MaxHp));
            }
            else if (cardIDX[1] != -1 && (tiles = HandCard[cardIDX[1]].GetAvailableTile(position)).Count > 0)
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

                crystalCount -= HandCard[cardIDX[1]].GetCost();
                yield return StartCoroutine(CardUse(toGo, cardIDX[1]));
            }
            else
            {
                TurnEnd();
            }
            yield return new WaitForSeconds(0.1f);
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
        a.Disposable = false;
        a.SetCost(1);
        a.SetRange(1);
        a._dmg = 40;
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

