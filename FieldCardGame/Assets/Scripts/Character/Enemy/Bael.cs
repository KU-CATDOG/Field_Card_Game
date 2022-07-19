using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bael : Enemy
{
    private int crystal;
    protected override void Start()
    {
        base.Start();
        Hp = MaxHp = 200;
        crystal = 4;
        GiveExp = 10;
        TurnStartDraw = 5;
    }
    protected override IEnumerator payCost(int cost, CostType type)
    {
        if(type == CostType.MonsterCrystal)
        {
            crystal -= cost;
        }
        yield break;
    }
    public override bool PayTest(int cost, CostType type)
    {
        if(type == CostType.MonsterCrystal)
        {
            return crystal >= cost;
        }
        return false;
    }

    public override IEnumerator AwakeTurn()
    {
        crystal = 4;
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
    public override IEnumerator StartTurn()
    {
        yield break;
    }
    protected override IEnumerator getDmg(int dmg)
    {
        yield break;
    }
    protected override IEnumerator dieRoutine()
    {
        yield return base.dieRoutine();
    }
    public override IEnumerator EnemyRoutine()
    {
        int moveCard;
        if (HandCard[1] is EnemyAttack)
        {
            moveCard = 0;
        }
        else
        {
            moveCard = 1;
        }

        HandCard[moveCard].SetRange(3);
        List<Coordinate> tiles = HandCard[moveCard].GetAvailableTile(position);
        Coordinate toGo = tiles[0];
        int minDist = 1000;
        foreach (var i in tiles)
        {
            foreach (var j in GameManager.Instance.Allies)
            {
                if (minDist > Coordinate.Distance(i, j.position))
                {
                    minDist = Coordinate.Distance(i, j.position);
                    toGo = i;
                }
            }
        }
        yield return StartCoroutine(CardUse(toGo, moveCard));
        tiles = HandCard[0].GetAvailableTile(position);
        if (tiles.Count != 0)
        {
            Coordinate toAttack = tiles[Random.Range(0, tiles.Count)];
            yield return StartCoroutine(CardUse(toAttack, 0));
        }
    }
    protected override void InitializeDeck()
    {
        CardPile.Add(new PaladinMove());
        CardPile.Add(new EnemyAttack());
    }
}
