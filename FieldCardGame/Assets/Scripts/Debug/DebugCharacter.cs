using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugCharacter : Player
{
    private TextMeshProUGUI crystalText;
    private int crystalCount;
    protected override IEnumerator levelUp()
    {
        yield break;
    }
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
        crystalCount = 3;
        yield break;
    }
    protected override IEnumerator dieRoutine()
    {
        yield break;
    }
    protected override IEnumerator payCost(int cost, CostType type)
    {
        if (type == CostType.PaladinEnergy)
        {
            crystalCount -= cost;
        }
        else if (type == CostType.Hp)
        {
            yield return StartCoroutine(GetDmg(this, cost, true));
        }
        else
        {
            yield break;
        }
        yield break;
    }
    public override bool PayTest(int cost, CostType type)
    {
        if (type == CostType.PaladinEnergy)
        {
            return crystalCount >= cost;
        }
        else if (type == CostType.Hp)
        {
            return Hp >= cost;
        }
        else
        {
            return false;
        }
    }
    public void DebugDrawCard()
    {
        StartCoroutine(DrawCard());
    }
    protected override void Start()
    {
        base.Start();
        TurnStartDraw = 5;
        MaxHp = Hp = 50;
        crystalText = PlayerUI.GetComponentInChildren<TextMeshProUGUI>();
    }
    protected override void Update()
    {
        base.Update();
        crystalText.text = $"{crystalCount}";
    }

    public void DebugGetDmg()
    {
        StartCoroutine(this.GetDmg(this, 10));
    }

    public void DebugGetDmg12()
    {
        StartCoroutine(this.GetDmg(this, 12));
    }

    public void DebugGetShield()
    {
        this.BuffHandler.Shield(15);
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
        StartCoroutine(AddCard(new PaladinSMA()));
    }
    public void AddSummonCard()
    {
        StartCoroutine(AddCard(new DebugCard()));
    }
    public void AddAttackCard()
    {
        StartCoroutine(AddCard(new Attack()));
    }
    public void AddWarlockMoveCard()
    {
        StartCoroutine(AddCard(new WarlockMove()));
    }
    public void AddWarlockSnatchCard()
    {
        StartCoroutine(AddCard(new WarlockSnatch()));
    }
    public void AddWarlockGatheringCard()
    {
        StartCoroutine(AddCard(new WarlockGathering()));
    }
    public void AddWarlockDrainCard()
    {
        StartCoroutine(AddCard(new WarlockDrain()));
    }
    protected override void InitializeDeck()
    {
        CardPile.Add(new PaladinMove());
        CardPile.Add(new PaladinMove());
        CardPile.Add(new PaladinMove());
        CardPile.Add(new PaladinMove());
        CardPile.Add(new PaladinProtect());
    }
}
