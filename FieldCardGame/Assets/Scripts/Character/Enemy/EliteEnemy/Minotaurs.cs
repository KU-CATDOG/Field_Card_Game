using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaurs : Enemy
{
    protected override void Start()
    {
        base.Start();
        Hp = MaxHp = 150;
        GiveExp = 1;
        TurnStartDraw = 4;
        crystalCount = maxCrystalCount = 4;
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

    public int[] findAllCardIDX()
    {
        int[] ret = new int[] { -1, -1, -1, -1 };

        ret[0] = findCardIDX(typeof(EnemyMove));
        ret[1] = findCardIDX(typeof(EnemyMoveNAttack));


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
        int[] cardIDX; //Move, Charge, Chop, else

        while (crystalCount > 0)
        {
            cardIDX = findAllCardIDX();

            var j = GameManager.Instance.CharacterSelected.position;
            int currDist = Coordinate.Distance(position, j);

            if (currDist > 3 && cardIDX[1] != -1 && crystalCount >= 2)
            {
                int minDist = int.MaxValue;
                List<Coordinate> tiles = HandCard[cardIDX[1]].GetAvailableTile(position);

                Coordinate toAtk = tiles[0];
                foreach (var i in tiles)
                {
                    if (minDist > Coordinate.Distance(i, j))
                    {
                        minDist = Coordinate.Distance(i, j);
                        toAtk = i;
                    }
                }

                crystalCount -= HandCard[cardIDX[1]].GetCost();
                yield return StartCoroutine(CardUse(toAtk, cardIDX[1]));
            }
            else if ((currDist >= 2 || currDist <= 3) && cardIDX[0] != -1)
            {
                int minDist = int.MaxValue;
                List<Coordinate> tiles = HandCard[cardIDX[0]].GetAvailableTile(position);

                Coordinate toGo = tiles[0];
                foreach (var i in tiles)
                {
                    if (minDist > Coordinate.Distance(i, j))
                    {
                        minDist = Coordinate.Distance(i, j);
                        toGo = i;
                    }
                }

                crystalCount -= HandCard[cardIDX[0]].GetCost();
                yield return StartCoroutine(CardUse(toGo, cardIDX[0]));
            }
            else if (currDist == 1)
            {
                bool isready = HandCard[cardIDX[3]] is Strength ? false : true;

                if (!isready && crystalCount >= 2)
                {
                    crystalCount -= HandCard[cardIDX[3]].GetCost();
                    yield return StartCoroutine(CardUse(position, cardIDX[3]));
                }
                else if (isready && crystalCount >= 4)
                {
                    crystalCount -= HandCard[cardIDX[3]].GetCost();
                    yield return StartCoroutine(CardUse(j, cardIDX[3]));
                }
                else if (crystalCount >= 3 && cardIDX[2] != -1)
                {
                    crystalCount -= HandCard[cardIDX[2]].GetCost();
                    yield return StartCoroutine(CardUse(j, cardIDX[2]));
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
    protected override void InitializeDeck()
    {
        EnemyMove b = new EnemyMove();
        b.Disposable = false;
        b.SetCost(1);
        b.SetRange(2);
        CardPile.Add(b);

        EnemyMoveNAttack c = new EnemyMoveNAttack();
        c.Disposable = false;
        c._cost = 2;
        c._range = 4;
        c._rangeType = RangeType.CrossWise;
        c._dmg = 40;
        CardPile.Add(c);

        TrashEnemyAttack p = new TrashEnemyAttack();
        p.Disposable = false;
        p.SetCost(3);
        p.SetRange(1);
        p._dmg = 20;
        CardPile.Add(p);

        CardPile.Add(new Strength()); 
    }
}
