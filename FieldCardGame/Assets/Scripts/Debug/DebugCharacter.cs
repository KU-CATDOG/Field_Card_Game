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
        MaxHp = Hp = 100;
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

    public void DebugGetBuffList()
    {
        foreach(var buff in this.EffectHandler.GetEnabledBuff())
        {
            Debug.Log(buff);
        }
    }

    public void DebugGetDebuffList()
    {
        foreach(var debuff in this.EffectHandler.GetEnabledDebuff())
        {
            Debug.Log(debuff);
        }
    }

    public void DebugGetShield()
    {
        this.EffectHandler.BuffDict[BuffType.Shield].SetEffect(15);
    }

    public void DebugGetWill()
    {
        this.EffectHandler.BuffDict[BuffType.Will].SetEffect(1);
    }

    public void DebugGetStun()
    {
        this.EffectHandler.DebuffDict[DebuffType.Stun].SetEffect(1);
    }

    public void DebugGetWeakness()
    {
        this.EffectHandler.DebuffDict[DebuffType.Weakness].SetEffect(1);
    }

    public void DebugGetPoison()
    {
        this.EffectHandler.DebuffDict[DebuffType.Poison].SetEffect(5);
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
        StartCoroutine(AddCard(new WarlockEarthbound()));
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
    public void AddPaladinShiningCard()
    {
        StartCoroutine(AddCard(new PaladinShining()));
    }
    public void AddWarlockSoulBeadCard()
    {
        StartCoroutine(AddCard(new WarlockSoulBead()));
    }
    public void AddPaladinRevelation()
    {
        StartCoroutine(AddCard(new PaladinRevelation1()));
    }
    public void AddPaladinDeliever()
    {
        StartCoroutine(AddCard(new PaladinDeliver()));
    }
    public void AddDebugJump()
    {
        StartCoroutine(AddCard(new DebugJump()));
    }
    public void AddWarlockGrip()
    {
        StartCoroutine(AddCard(new WarlockGrip()));
    }
    public void AddWarlockSacrifice()
    {
        StartCoroutine(AddCard(new WarlockSacrifice()));
    }
    public void AddWarlockChase()
    {
        StartCoroutine(AddCard(new WarlockChase()));
    }
    public void AddWarlockFalseContract()
    {
        StartCoroutine(AddCard(new WarlockFalseContract()));
    }
    public void AddWarlockEarthbound()
    {
        StartCoroutine(AddCard(new WarlockEarthbound()));
    }
    public void AddWarlockSmite()
    {
        StartCoroutine(AddCard(new WarlockSmite()));
    }
    public void AddWarlockBatWizard()
    {
        StartCoroutine(AddCard(new WarlockBatWizard()));
    }
    public void AddWarlockSoul()
    {
        StartCoroutine(AddCard(new WarlockSoul()));
    }
    public void AddWarlockRegeneration()
    {
        StartCoroutine(AddCard(new WarlockRegeneration()));
    }
    public void AddWarlockDmgDraw()
    {
        StartCoroutine(AddCard(new WarlockDmgDraw()));
    }
    public void AddWarlockGetSoul()
    {
        StartCoroutine(AddCard(new WarlockGetSoul()));
    }
    public void AddWarlockRanyor()
    {
        StartCoroutine(AddCard(new WarlockRanyor()));
    }
    public void AddWarlockInflation()
    {
        StartCoroutine(AddCard(new WarlockInflation()));
    }
    public void AddWarlockMemento()
    {
        StartCoroutine(AddCard(new WarlockMemento()));
    }
    public void AddWarlockHeal()
    {
        StartCoroutine(AddCard(new WarlockHeal()));
    }
    public void AddWarlockIllusion()
    {
        StartCoroutine(AddCard(new WarlockIllusion()));
    }
    public void AddWarlockLegend()
    {
        StartCoroutine(AddCard(new WarlockLegend()));
    }
    public void AddWarlockSoulBomb()
    {
        StartCoroutine(AddCard(new WarlockSoulBomb()));
    }
    public void AddWarlockLissandra()
    {
        StartCoroutine(AddCard(new WarlockLissandra()));
    }
    public void AddWarlockOverheal()
    {
        StartCoroutine(AddCard(new WarlockOvverheal()));
    }
    public void AddWarlockMosquito()
    {
        StartCoroutine(AddCard(new WarlockMosquito()));
    }
    public void AddWarlockAdvancedMove()
    {
        StartCoroutine(AddCard(new WarlockAdvancedMove()));
    }
    public void AddWarlockRecover()
    {
        StartCoroutine(AddCard(new WarlockRecover()));
    }
    public void AddWarlockCurse()
    {
        StartCoroutine(AddCard(new WarlockCurse()));
    }
    public void AddWarlockVampire()
    {
        StartCoroutine(AddCard(new WarlockVampire()));
    }
    protected override void InitializeDeck()
    {
        CardPile.Add(new PaladinMove());
        CardPile.Add(new PaladinMove());
        CardPile.Add(new PaladinMove());
        CardPile.Add(new PaladinProtect());
        CardPile.Add(new PaladinFighting());
        CardPile.Add(new PaladinJump());
        CardPile.Add(new PaladinJump());
        CardPile.Add(new PaladinMove());
    }
}
