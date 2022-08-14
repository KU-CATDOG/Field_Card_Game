using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockRepay : IPlayerCard
{
    private int range = 0;
    private bool notRemoved = true;
    private int cost = 0;
    private int amount = 5;
    private bool interrupted;
    public bool Disposable { get; set; } = true;
    public string ExplainText
    {
        get
        {
            return $"이번 턴 동안 받은 피해 횟수 당 5만큼 ‘성장’을 얻습니다.";
        }
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
        caster.EffectHandler.BuffDict[BuffType.Growth].SetEffect(GetAmount() - 5);
        SetAmount(5);
        yield break;
    }
    public IEnumerator GetCardRoutine(Character owner)
    {
        owner.AddGetDmgRoutine(AddAmount(owner),0);
        yield break;
    }
    private IEnumerator AddAmount(Character owner)
    {
        while (notRemoved)
        {
            if (owner.HandCard.Contains(this))
            {
                SetAmount(GetAmount() + 5);
            }
            yield return null;
        }
    }
    public IEnumerator RemoveCardRoutine(Character owner)
    {
        notRemoved = false;
        yield break;
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
        return CardType.Attack;
    }
    public int GetCardID()
    {
        return 3024001;
    }
}
