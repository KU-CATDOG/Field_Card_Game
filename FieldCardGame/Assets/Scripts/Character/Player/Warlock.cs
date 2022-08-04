using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Warlock : Player
{
    private TextMeshProUGUI bloodText;
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
        if(Hp > MaxHp)
        {
            Hp = (Hp-10) > MaxHp ? Hp - 10:MaxHp;
        }
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
        if (type == CostType.Hp)
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
        if (type == CostType.Hp)
        {
            return Hp >= cost;
        }
        else
        {
            return false;
        }
    }
    protected override void Start()
    {
        base.Start();
        bloodText = PlayerUI.GetComponentInChildren<TextMeshProUGUI>();
        TurnStartDraw = 5;
        MaxHp = Hp = 50;
    }
    protected override void Update()
    {
        base.Update();
        bloodText.text = $"{Hp}";
    }


    protected override void InitializeDeck()
    {
        CardPile.Add(new WarlockMove());
        CardPile.Add(new WarlockDebuff());
        CardPile.Add(new WarlockVampAttack());
        CardPile.Add(new WarlockVampire());
        CardPile.Add(new WarlockDebuffDraw());
        StartCoroutine(ShuffleDeck());
    }
}

