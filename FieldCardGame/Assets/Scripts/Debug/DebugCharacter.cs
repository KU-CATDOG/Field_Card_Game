using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCharacter : Player
{
    protected override IEnumerator getDmg(int dmg)
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
    public override IEnumerator AwakeTurn()
    {
        yield break;
    }
    public override IEnumerator StartTurn()
    {
        yield break;
    }
    protected override IEnumerator dieRoutine()
    {
        yield break;
    }
    protected override IEnumerator payCost(int cost, CostType type)
    {
        yield break;
    }
    public override bool PayTest(int cost, CostType type)
    {
        return true;
    }
    public void DebugDrawCard()
    {
        StartCoroutine(DrawCard());
    }
    private void Start()
    {
        TurnStartDraw = 5;
    }
    public void DebugAddCard()
    {/*
        if (addedCard is DebugCard)
        {
        }
        else
        {
            addedCard = new DebugCard();
        }*/
        StartCoroutine(AddCard(new PaladinMove()));
    }
    public void AddSummonCard()
    {
        StartCoroutine(AddCard(new DebugCard()));
    }
}
