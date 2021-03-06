using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Paladin : Player
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
        MaxHp = Hp = 50;
        crystalText = PlayerUI.GetComponentInChildren<TextMeshProUGUI>();
    }
    protected override void Update()
    {
        base.Update();
        crystalText.text = $"{crystalCount}";
    }


    protected override void InitializeDeck()
    {
        CardPile.Add(new PaladinMove());
        CardPile.Add(new PaladinMove());
        CardPile.Add(new Attack());
        CardPile.Add(new Attack());
        CardPile.Add(new PaladinProtect());
    }
}
