using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pagan : Enemy
{ 
    protected void Start()
    {
        base.Start();
        Hp = MaxHp = 35;
        GiveExp = 0;
        TurnStartDraw = 2;
        crystalCount = maxCrystalCount = 3;
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
    private int[] FindAllCardIDX()
    {
        int[] ret = new int[] { FindCardIDX(typeof(EnemyMove)), FindCardIDX(typeof(TrashEnemyHeal)) };

        return ret;
    }
    public override IEnumerator EnemyRoutine()
    {
        int[] cardIDX;

        while (crystalCount > 0)
        {
            cardIDX = FindAllCardIDX();

            var j = GameManager.Instance.CharacterSelected.position;
            int currDist = Coordinate.Distance(position, j);

            List<Coordinate> tiles;

            if (currDist >= 3 && crystalCount >= 2)
            {
                if ((tiles = HandCard[cardIDX[1]].GetAvailableTile(position)).Count > 0)
                {
                    Coordinate toHeal = tiles[0];
                    int maxHurt = int.MinValue;

                    foreach (Coordinate i in tiles)
                    {
                        Character currChar = GameManager.Instance.Map[i.X, i.Y].CharacterOnTile;

                        int currHurt = currChar.MaxHp - currChar.Hp;

                        if (currHurt > maxHurt)
                        {
                            toHeal = i;
                            maxHurt = currHurt;
                        }
                    }

                    crystalCount -= HandCard[cardIDX[1]].GetCost();
                    yield return StartCoroutine(CardUse(toHeal, cardIDX[1]));
                }
                else if (Hp < MaxHp)
                {
                    crystalCount -= HandCard[cardIDX[1]].GetCost();
                    yield return StartCoroutine(CardUse(position, cardIDX[1]));
                }
                else
                {
                    TurnEnd();
                }
            }
            else if (currDist < 3)
            {
                if ((tiles = HandCard[cardIDX[0]].GetAvailableTile(position)).Count > 0)
                {
                    Coordinate toGo = tiles[0];
                    int maxDist = int.MinValue;

                    foreach (var i in tiles)
                    {
                        if (maxDist < Coordinate.Distance(i, j))
                        {
                            maxDist = Coordinate.Distance(i, j);
                            toGo = i;
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
                TurnEnd();
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
        EnemyMove a = new EnemyMove();
        a.Disposable = false;
        a.SetCost(1);
        a.SetRange(3);
        a._rangeType = RangeType.Distance;
        CardPile.Add(a);

        TrashEnemyHeal h = new TrashEnemyHeal();
        h.SetCost(2);
        h.SetRange(4);
        h._recoveryValue = 20;
        h.Disposable = false;
        CardPile.Add(h);
    }

    protected override IEnumerator payCost(int cost, CostType type)
    {
        yield break;
    }
}

