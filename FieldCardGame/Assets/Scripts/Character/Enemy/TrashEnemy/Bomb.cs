using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Enemy
{ 
    protected void Start()
    {
        base.Start();
        Hp = MaxHp = int.MaxValue;
        GiveExp = 0;
        TurnStartDraw = 1;
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
        if(Hp != MaxHp)
        {
            List<Coordinate> tiles;

            if ((tiles = HandCard[0].GetAvailableTile(position)).Count > 0)
            {
                Coordinate toATK = tiles[0];

                foreach (var i in tiles)
                {
                    foreach (var j in GameManager.Instance.Allies)
                    {
                        Coordinate pos = j.position;
                        if (i.X == pos.X && i.Y == pos.Y)
                        {
                            toATK = i;
                            break;
                        }
                    }
                }

                crystalCount -= HandCard[0].GetCost();
                yield return StartCoroutine(CardUse(toATK, 0));
            }
            yield return GameManager.Instance.StartCoroutine(this.HitAttack(GameManager.Instance.Map[position.X, position.Y].CharacterOnTile, MaxHp));
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
        a._dmg = 40;
        CardPile.Add(a);
    }

    protected override IEnumerator payCost(int cost, CostType type)
    {
        yield break;
    }
}

