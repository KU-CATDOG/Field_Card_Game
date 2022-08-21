using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinRest : IPlayerCard, IHealCard
{
    private int range = 0;
    private bool interrupted;
    private int cost = 0;
    private int amount = 10;
    public bool Disposable { get; set; }
    public string ExplainText
    {
        get
        {
            return $"체력을 {amount} 회복합니다. 카드를 1장 버립니다.";
        }
    }
    public IEnumerator GetCardRoutine(Character owner)
    {
        yield break;
    }
    public IEnumerator RemoveCardRoutine(Character owner)
    {
        yield break;
    }
    public int GetRange()
    {
        return range;
    }
    public void SetRange(int _range)
    {
        range = _range;
    }
    public int GetAmount()
    {
        return amount;
    }
    public void SetAmount(int _amount)
    {
        amount = _amount;
    }
    public List<int> HealAmounts{get;}
    public void SetHealAmount(int val)
    {
        if (amount + val < 0)
        {
            return;
        }
        amount += val;
    }
    public Color GetUnAvailableTileColor()
    {
        return Color.red;
    }

    public List<Coordinate> GetAvailableTile(Coordinate pos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        ret.Add(pos);
        return ret;
    }

    public Color GetAvailableTileColor()
    {
        return Color.blue;
    }

    public List<Coordinate> GetAreaofEffect(Coordinate relativePos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        ret.Add(new Coordinate(0, 0));
        return ret;
    }

    public Color GetColorOfEffect(Coordinate pos)
    {
        if (pos.X == 0 && pos.Y == 0)
        {
            return Color.white;
        }
        return Color.black;
    }

    public bool IsAvailablePosition(Coordinate caster, Coordinate target)
    {
        List<Coordinate> availablePositions = GetAvailableTile(caster);
        if (availablePositions.Exists((i) => i.X == target.X && i.Y == target.Y))
        {
            return true;
        }
        return false;
    }
    public IEnumerator CardRoutine(Character caster, Coordinate target)
    {
        if (interrupted)
        {
            interrupted = false;
            yield break;
        }

        yield return GameManager.Instance.StartCoroutine(caster.GiveHeal(caster, GetAmount()));
        int i = Random.Range(0, caster.HandCard.Count);
        yield return caster.StartCoroutine(caster.DropCard(i));
    }
    public void CardRoutineInterrupt()
    {
        interrupted = true;
    }
    public int GetCost()
    {
        return cost;
    }
    public void SetCost(int value)
    {
        cost = value;
    }
    public CostType GetCostType()
    {
        return CostType.PaladinEnergy;
    }
    public CardType GetCardType()
    {
        return CardType.Skill;
    }
    public int GetCardID()
    {
        return 1028001;
    }
}

