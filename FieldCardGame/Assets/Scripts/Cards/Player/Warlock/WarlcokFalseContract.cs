using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockFalseContract : IPlayerCard
{
    public bool Disposable { get; set; }
    private int range = 0;
    private int cost = 25;
    private bool interrupted;
    public int GetRange()
    {
        return range;
    }
    public void SetRange(int _range)
    {
        range = _range;
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
        List<ICard> InHand = new(caster.HandCard);
        foreach(var i in InHand)
        {
            i.SetCost(0);
        }
        caster.AddTurnEndDebuff(RetrieveCost(caster, InHand), 0);
        caster.EffectHandler.DebuffDict[DebuffType.Fragility].SetEffect(1);
        yield return null;
    }
    private IEnumerator RetrieveCost(Character caster, List<ICard> toRetrieve)
    {
        List<ICard> InHand = caster.HandCard;
        List<ICard> InDiscarded = caster.DiscardedPile;
        List<ICard> InDummy = caster.CardPile;
        foreach(var i in InHand)
        {
            if(toRetrieve.Exists(j => j==i))
            {
                i.SetCost(GameManager.Instance.CardDict[i.GetCardID()].GetCost());
            }
        }
        foreach(var i in InDiscarded)
        {
            if(toRetrieve.Exists(j => j==i))
            {
                i.SetCost(GameManager.Instance.CardDict[i.GetCardID()].GetCost());
            }
        }
        foreach(var i in InDummy)
        {
            if(toRetrieve.Exists(j => j==i))
            {
                i.SetCost(GameManager.Instance.CardDict[i.GetCardID()].GetCost());
            }
        }
        yield return null;
    }
    public void CardRoutineInterrupt()
    {
        interrupted = true;
    }
    public int GetCost()
    {
        return cost;
    }
    public void SetCost(int _cost)
    {
        cost = _cost;
    }
    public CostType GetCostType()
    {
        return CostType.Hp;
    }
    public CardType GetCardType()
    {
        return CardType.Skill;
    }
    public int GetCardID()
    {
        return 3107001;
    }

}
