using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{ 
    protected void Start()
    {
        base.Start();
        Hp = MaxHp = 40;
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

        if (HandCard[0] is TrashEnemyAttack)
        {
            atkIsFst = true;
        }
        else
        {
            atkIsFst = false;
        }
        

        while (crystalCount > 0)
        { 
            var j = GameManager.Instance.CharacterSelected.position;
            int currDist = Coordinate.Distance(position, j); //need fix

            if (currDist > 3)
            {
                List<Coordinate> tiles;

                if ((tiles = HandCard[atkIsFst ? 1 : 0].GetAvailableTile(position)).Count > 0)
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

                    crystalCount -= HandCard[atkIsFst ? 1 : 0].GetCost();
                    DropInterrupted = true;
                    yield return StartCoroutine(CardUse(toGo, atkIsFst ? 1 : 0));
                    DropInterrupted = false;
                }
            }
            else if (currDist == 2)
            {

                crystalCount -= HandCard[atkIsFst ? 0 : 1].GetCost();
                yield return StartCoroutine(CardUse(j, atkIsFst ? 0 : 1));
                break;
            }
            else
            {
                crystalCount -= HandCard[atkIsFst ? 0 : 1].GetCost();
                yield return StartCoroutine(CardUse(j, atkIsFst ? 0 : 1));

                List<Coordinate> tiles;

                if ((tiles = HandCard[atkIsFst ? 1 : 0].GetAvailableTile(position)).Count > 0)
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

                    crystalCount -= HandCard[atkIsFst ? 1 : 0].GetCost();
                    DropInterrupted = true;
                    yield return StartCoroutine(CardUse(toGo, atkIsFst ? 1 : 0));
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
        TrashEnemyAttack D = new TrashEnemyAttack();
        D.SetCost(1);
        D.SetRange(2);
        D._dmg = 12;
        CardPile.Add(D);
    

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

