using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oger : Enemy
{
    protected override void Start()
    {
        base.Start();
        Hp = MaxHp = 180;
        GiveExp = 1;
        TurnStartDraw = 4;
        crystalCount = maxCrystalCount = 3;
}
    protected override IEnumerator payCost(int cost, CostType type)
    {
        yield break;
    }
    public override bool PayTest(int cost, CostType type)
    {
        return true;
    }

    public override IEnumerator AwakeTurn()
    {
        yield break;
    }
    public override IEnumerator AfterBuff()
    {
        yield break;
    }
    public override IEnumerator AfterDraw()
    {
        yield break;
    }
    protected override IEnumerator getDmg(int dmg)
    {
        yield break;
    }
    protected override IEnumerator enemyDieRoutine()
    {
        yield break;
    }

    private bool[] calcAtkWheater(int queenAtxIdx, int bishopAtxIdx)
    {
        bool[] res = new bool[] {false, false};
        List<Coordinate> tiles;

        if (queenAtxIdx != -1)
        {
            tiles = HandCard[queenAtxIdx].GetAvailableTile(position);

            foreach (var T in tiles)
            {
                foreach (var j in GameManager.Instance.Allies)
                {
                    if (T.X == j.position.X && T.Y == j.position.Y)
                    {
                        res[0] = true;
                        break;
                    }
                }
                if (res[0])
                    break;
            }
        }
        if (bishopAtxIdx != -1)
        {
            tiles = HandCard[bishopAtxIdx].GetAreaofEffect(position);

            foreach (var T in tiles)
            {
                foreach (var j in GameManager.Instance.Allies)
                {
                    if (T.X == j.position.X && T.Y == j.position.Y)
                    {
                        res[1] = true;
                        break;
                    }
                }
                if (res[1])
                    break;
            }
        }

        return res;

    }

    private int[] RecordCardIdx()
    {
        int[] ret = { FindCardIDX(typeof(EnemyMove)), FindCardIDX(typeof(TrashEnemyAttack)), FindCardIDX(typeof(TrashEnemyDebuff)), FindCardIDX(typeof(TrashEnemyHeal)) };

        return ret;
    }

    public override IEnumerator EnemyRoutine()
    {
        int[] cardIDX;
     
        while (crystalCount > 0)
        {
            cardIDX = RecordCardIdx();

            if(cardIDX[3] != -1 &&  Hp <= 90)
            {
                crystalCount -= HandCard[cardIDX[3]].GetCost();
                yield return StartCoroutine(CardUse(position, cardIDX[3]));
            }
            else
            {
                Coordinate j = GameManager.Instance.CharacterSelected.position;
                int currDist = Coordinate.Distance(position, j);

                List<Coordinate> tiles;

                if (currDist >= 4)
                {
                    if (currDist == 4)
                    {
                        crystalCount -= HandCard[cardIDX[2]].GetCost();
                        yield return StartCoroutine(CardUse(j, cardIDX[2]));
                    }

                    if(cardIDX[0] != -1 && (tiles = HandCard[cardIDX[0]].GetAvailableTile(position)).Count > 0)
                    {
                        Coordinate toGo = tiles[0];
                        int minDist = int.MaxValue;

                        foreach (var i in tiles)
                        {
                            int d;
                            if((d = Coordinate.Distance(i, j)) < minDist)
                            {
                                minDist = d;
                                toGo = i;
                            }
                        }

                        yield return StartCoroutine(CardUse(toGo, cardIDX[0]));
                    }
                }
                else if(currDist == 2 && currDist == 3)
                {
                    if (cardIDX[0] != -1 && (tiles = HandCard[cardIDX[0]].GetAvailableTile(position)).Count > 0)
                    {
                        Coordinate toGo = tiles[0];
                        int minDist = int.MaxValue;

                        foreach (var i in tiles)
                        {
                            int d;
                            if ((d = Coordinate.Distance(i, j)) < minDist)
                            {
                                minDist = d;
                                toGo = i;
                            }
                        }

                        yield return StartCoroutine(CardUse(toGo, cardIDX[0]));
                    }

                    if(HandCard[cardIDX[1]].GetAvailableTile(position).Count > 0)
                    {
                        crystalCount -= HandCard[cardIDX[1]].GetCost();
                        yield return StartCoroutine(CardUse(j, cardIDX[1]));
                    }
                }
                else
                {
                    if (HandCard[cardIDX[1]].GetAvailableTile(position).Count > 0)
                    {
                        crystalCount -= HandCard[cardIDX[1]].GetCost();
                        yield return StartCoroutine(CardUse(j, cardIDX[1]));
                    }
                }

            }
          
        }

    }
    protected override void InitializeDeck()
    {
        EnemyMove m = new EnemyMove();
        m.SetCost(0);
        m.SetRange(3);
        m.SetMinRange(2);
        m._rangeType = RangeType.CrossWise;
        CardPile.Add(m);

        TrashEnemyHeal h = new TrashEnemyHeal();
        h.Disposable = true;
        h.SetCost(3);
        h._recoveryValue = 60;
        CardPile.Add(h);

        TrashEnemyAttack a = new TrashEnemyAttack();
        a.SetCost(3);
        a.SetRange(1);
        a._dmg = 60;
        CardPile.Add(a);

        TrashEnemyDebuff t = new TrashEnemyDebuff();
        t.SetCost(3);
        t.SetRange(4);
        t._debuffType = DebuffType.Rooted;
        t._debuffValue = 1;
        t.SetMinRange(4);
        CardPile.Add(t);
    }
}
