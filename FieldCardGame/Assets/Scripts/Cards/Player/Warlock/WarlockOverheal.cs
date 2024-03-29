using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockOverheal : IPlayerCard,IHealCard
{
    private int range = 0;
    private int cost = 15;
    private int healAmount = 5;
    public List<int> HealAmounts
    {
        get
        {
            List<int> tmp = new();
            tmp.Add(healAmount);
            return tmp;
        }
    }
    public void SetHealAmount(int value)
    {
        healAmount = healAmount + value < 0 ? 0 : healAmount + value;
        HealAmounts[0] = healAmount;
    }
    private bool interrupted;
    public bool Disposable { get; set; }
    public string ExplainText
    {
        get
        {
            return $"이번 턴에 사용한 카드의 수당 {healAmount}만큼 체력을 회복합니다.";
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
    public int GetHealAmount()
    {
        return healAmount;
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
        yield return GameManager.Instance.StartCoroutine(caster.GiveHeal(caster, (caster.cardUseInTurn-1)* GetHealAmount(),true));
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
        return 3128001;
    }
}
