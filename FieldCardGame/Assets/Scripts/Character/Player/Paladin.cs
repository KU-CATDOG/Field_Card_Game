using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Paladin : Player
{
    private TextMeshProUGUI crystalText;
    private int crystalCount;
    public virtual int CrystalCount
    {
        get
        {
            return crystalCount;
        }
        set
        {
            crystalCount = value;
        }
    }
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
        else
        {
            return false;
        }
    }
    protected override void Start()
    {
        base.Start();
        TurnStartDraw = 5;
        MaxHp = Hp = 120;
        crystalText = PlayerUI.GetComponentInChildren<TextMeshProUGUI>();
    }
    protected override void Update()
    {
        base.Update();
        crystalText.text = $"{crystalCount}";
    }


    protected override void InitializeDeck()
    {
        //fixme

        CardPile.Add(new PaladinMove());
        CardPile.Add(new PaladinMove());
        CardPile.Add(new PaladinMove());
        CardPile.Add(new Attack());
        CardPile.Add(new PaladinProtect());
        CardPile.Add(new PaladinShining());
        CardPile.Add(new PaladinSMA());
        CardPile.Add(new PaladinDeliver());
        CardPile.Add(new PaladinJump());
        CardPile.Add(new PaladinFirm());
        CardPile.Add(new PaladinChecking());
        StartCoroutine(ShuffleDeck());
    }
}
