using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEnemy : Enemy
{
    private void Start()
    {
        TurnStartDraw = 1;
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
        yield break;
    }
    public override IEnumerator EnemyRoutine()
    {
        HandCard[0].SetRange(3);
        List<Coordinate> tiles = HandCard[0].GetAvailableTile(position);
        int random = Random.Range(0, tiles.Count);
        yield return StartCoroutine(CardUse(tiles[random], 0));
    }
    protected override void InitializeDeck()
    {
        CardPile.Add(new PaladinMove());
    }
}
