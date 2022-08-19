using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashEnemyHeal : ICard
{
    public bool Disposable { get; set; }

    private int cost;
    private int recoveryValue;
    private RangeType rangeType;
    
    public int _recoveryValue
    {
        get { return recoveryValue; }
        set { recoveryValue = value; }
    }
    
    public RangeType _rangeType
    {
        get { return rangeType; }
        set { rangeType = value; }
    }

    private int range;

    private bool interrupted;
    public IEnumerator GetCardRoutine(Character owner)
    {
        yield break;
    }
    public IEnumerator RemoveCardRoutine(Character owner)
    {
        yield break;
    }

    public int GetCardID()
    {
        return 0002178;
    }

    public CostType GetCostType()
    {
        return CostType.MonsterCrystal;
    }
    public CardType GetCardType()
    {
        return CardType.Skill;
    }
    public List<Coordinate> GetAvailableTile(Coordinate pos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        List<Coordinate> canTile = pos.GetDistanceAvailableTile(range, RangeType.Distance, true);

        foreach (var t in canTile)
        {
            if (GameManager.Instance.Map[t.X, t.Y].CharacterOnTile is Enemy)
            {
                ret.Add(t);
            }
        }

        return ret;
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
    public List<Coordinate> GetAreaofEffect(Coordinate relativePos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        ret.Add(new Coordinate(0, 0));
        return ret;
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

            GameManager.Instance.StartCoroutine(caster.GiveHeal(GameManager.Instance.Map[target.X, target.Y].CharacterOnTile, recoveryValue));
        }

    }
    public void CardRoutineInterrupt()
    {
        interrupted = true;
    }



    // not use
    public int GetRange()
    {
        return 1;
    }
    public void SetRange(int _range)
    {
        range = _range;
    }
    public int GetCost()
    {
        return cost;
    }
    public void SetCost(int _cost)
    {
        cost = _cost;
    }

}
