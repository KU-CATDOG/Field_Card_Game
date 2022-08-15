using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinFairGame : IPlayerCard
{
    private int range = 1;
    private bool interrupted;
    private int cost = 2;
    private int amount = 20;
    public bool Disposable { get; set; }
    public string ExplainText
    {
        get
        {
            return $"대상을 ‘속박’합니다. ‘보호’를 20 얻습니다.";
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
        Coordinate tile;
        if ((tile = pos.GetDownTile()) != null)
        {
            ret.Add(tile);
        }
        if ((tile = pos.GetLeftTile()) != null)
        {
            ret.Add(tile);
        }
        if ((tile = pos.GetRightTile()) != null)
        {
            ret.Add(tile);
        }
        if ((tile = pos.GetUpTile()) != null)
        {
            ret.Add(tile);
        }
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
        Character tmp = GameManager.Instance.Map[target.X, target.Y].CharacterOnTile;
        if (tmp)
        {
            if (interrupted)
            {
                interrupted = false;
                yield break;
            }
            tmp.EffectHandler.DebuffDict[DebuffType.Rooted].SetEffect(1);
        }
        caster.EffectHandler.BuffDict[BuffType.Shield].SetEffect(GetAmount());
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
        return CostType.PaladinEnergy;
    }
    public CardType GetCardType()
    {
        return CardType.Skill;
    }
    public int GetCardID()
    {
        return 1035001;
    }
}

