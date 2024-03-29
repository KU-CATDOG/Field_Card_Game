using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockCycle : IPlayerCard
{
    public bool Disposable { get; set; }
    private int range = 0;
    private int cost = 0;
    private int amount = 4;
    private bool interrupted;
    public string ExplainText
    {
        get
        {
            return $"내 패를 모두 버립니다. 버린 카드 하나 당 4의 피해를 받고, 버린 카드 수 만큼 카드를 뽑습니다.";
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
        int handCount = caster.HandCard.Count;
        for (int i = handCount - 1; i >= 0; i--)
        {
            if(caster.HandCard[i] != this)
                yield return caster.DropCard(i);
        }
        for (int i = 1; i < handCount; i++)
        {
            yield return caster.GetDmg(caster, GetAmount(), true);
        }
        for (int i = 1; i < handCount; i++)
        {
            yield return caster.DrawCard();
        }
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
        return CardType.Skill;
    }
    public int GetCardID()
    {
        return 3023001;
    }

}
