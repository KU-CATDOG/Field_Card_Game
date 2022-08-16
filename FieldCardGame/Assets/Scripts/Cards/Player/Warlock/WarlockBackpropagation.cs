using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockBackpropagation : IPlayerCard
{
    public bool Disposable { get; set; }
    private int range = 0;
    private int cost = 10;
    private int amount = 10;
    private bool interrupted;
    private bool isTurnRestart = false;
    public string ExplainText
    {
        get
        {
            return $"이 스킬을 사용한 후, 다음 턴 시작 전까지 처음으로 나에게 피해를 입힌 적에게 ‘흡혈’을 10만큼 부여합니다.";
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
        if (interrupted)
        {
             interrupted = false;
             yield break;
        }
        caster.AddGetDmgRoutine(giveVamp(caster), 0);
        (caster as Warlock).awakeRoutine.Add(turnRestart());
        yield break;
    }
    public IEnumerator giveVamp(Character owner)
    {
        Character attacker;
        bool first = true;
        while (first && !isTurnRestart)
        {
            attacker = owner.HitBy;
            if (attacker != owner)
            {
                attacker.EffectHandler.DebuffDict[DebuffType.Vampire].SetEffect(10);
                first = false;
            }
            yield return null;
        }
    }
    public IEnumerator turnRestart()
    {
        isTurnRestart = true;
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
        return 3140001;
    }

}
