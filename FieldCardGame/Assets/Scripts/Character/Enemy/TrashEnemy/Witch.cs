using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : Enemy
{ 
    protected void Start()
    {
        base.Start();
        Hp = MaxHp = 35;
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

    public int[] findAllCardIDX()
    {
        int[] ret = new int[] { -1, -1, -1 };

        ret[0] = findCardIDX(typeof(EnemyMove));

        for (int i = 0; i < HandCard.Count; i++)
        {
            if (i != ret[0])
            {
                TrashEnemyDebuff a = (TrashEnemyDebuff)HandCard[i];
                if (a._debuffType == DebuffType.Weakness) // need fix. not weakness 허약이어야함.
                {
                    ret[1] = i;
                }
                else if (a._debuffType == DebuffType.Weakness) 
                {
                    ret[2] = i;
                }
            }
        }

        return ret;
    }

    public override IEnumerator EnemyRoutine()
    {
        int[] cardIDX; //Move, Death, Sloth

        while (crystalCount > 0)
        {
            cardIDX = findAllCardIDX();

            var j = GameManager.Instance.CharacterSelected.position;
            int currDist = Coordinate.Distance(position, j); //need fix

            if (currDist == 2)
            {
                if (crystalCount == 2)
                {
                    crystalCount = 0;
                    yield return StartCoroutine(CardUse(j, cardIDX[1]));
                }
                else
                {
                    crystalCount = 0;
                    yield return StartCoroutine(CardUse(j, cardIDX[2]));

                }
            }
            else if (currDist > 2)
            {
                List<Coordinate> tiles;

                if ((tiles = HandCard[cardIDX[0]].GetAvailableTile(position)).Count > 0)
                {
                    Coordinate toGo = tiles[0];
                    int minDist = int.MaxValue;

                    foreach (var i in tiles)
                    {
                        int d;
                        if ((d = Coordinate.Distance(i, j)) == 2 || d < minDist)
                        {
                            minDist = d;
                            toGo = i;
                            break;
                        }
                    }

                    crystalCount -= HandCard[cardIDX[0]].GetCost();
                    DropInterrupted = true;
                    yield return StartCoroutine(CardUse(toGo, cardIDX[0]));
                    DropInterrupted = false;
                }
            }
            else
            {
                List<Coordinate> tiles;

                if ((tiles = HandCard[cardIDX[0]].GetAvailableTile(position)).Count > 0)
                {
                    Coordinate toGo = tiles[0];
                    int maxDist = int.MinValue;

                    foreach (var i in tiles)
                    {
                        int d;
                        if ((d = Coordinate.Distance(i, j)) == 2 || d > maxDist)
                        {
                            maxDist = d;
                            toGo = i;
                            break;
                        }
                    }

                    crystalCount -= HandCard[cardIDX[0]].GetCost();
                    DropInterrupted = true;
                    yield return StartCoroutine(CardUse(toGo, cardIDX[0]));
                    DropInterrupted = false;
                }
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
        TrashEnemyDebuff D = new TrashEnemyDebuff();
        D.SetCost(2);
        D.SetRange(2);
        D._debuffType = DebuffType.Weakness; // need fix
        D._debuffValue = 1;
        CardPile.Add(D);

        TrashEnemyDebuff S = new TrashEnemyDebuff();
        S.SetCost(1);
        S.SetRange(2);
        S._debuffType = DebuffType.Weakness;
        S._debuffValue = 1;
        CardPile.Add(S);

        EnemyMove m = new EnemyMove();
        m.Disposable = false;
        m.SetCost(1);
        m._range = 3;
        CardPile.Add(m);
    }

    protected override IEnumerator payCost(int cost, CostType type)
    {
        yield break;
    }
}

