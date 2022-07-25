using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : Enemy
{
    protected void Start()
    {
        base.Start();
        Hp = MaxHp = 15;
        GiveExp = 0;
        TurnStartDraw = 2;
        crystalCount = 1;
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
        while (crystalCount > 0)
        {

            bool atkIsFst = false;

            if (HandCard[0] is EnemyAttack)
            {
                atkIsFst = true;
            }
            else
            {
                atkIsFst = false;
            }

            int enemycount = 0;

            foreach (var j in GameManager.Instance.Allies)
            {
                if (j is Enemy)
                {
                    enemycount++;
                }
            }

            List<Coordinate> tiles;

            if (enemycount == 1 && (tiles = HandCard[atkIsFst ? 0 : 1].GetAvailableTile(position)).Count != 0)
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
            else
            {
                //공격 범위 내에 약화 상태가 아닌 사람이 있으면 약화. 없으면 가까운 사람 공격.
               /* if((tiles = HandCard[atkIsFst ? 0 : 1].GetAvailableTile(position)).Count != 0 && )
                {

                }*/

            }

            yield break;
        }
    }

    public override bool PayTest(int cost, CostType type)
    {
        return true;
    }

    public override IEnumerator StartTurn()
    {
        crystalCount = 1;
        yield break;
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
        CardPile.Add(new TrashEnemyAttack(1, 10, 4));
    }

    protected override IEnumerator payCost(int cost, CostType type)
    {
        yield break;
    }
}
