using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNDebuff : ICard
{
    public bool Disposable { get; set; }

    private int cost;
    private int dmg;
    private DebuffType debuffType;
    private int debuffValue;
    private RangeType rangeType;
    public int _dmg
    {
        get { return dmg; }
        set { dmg = value; }
    }
    public DebuffType _debuffType
    {
        get { return debuffType; }
        set { debuffType = value; }
    }
    public int _debuffValue
    {
        get { return debuffValue; }
        set { debuffValue = value; }
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
        return 0002124;
    }

    public CostType GetCostType()
    {
        return CostType.MonsterCrystal;
    }
    public CardType GetCardType()
    {
        return CardType.Skill; //Attack N Skill ÇÊ¿ä
    }
    public List<Coordinate> GetAvailableTile(Coordinate pos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        List<Coordinate> canTile = pos.GetDistanceAvailableTile(range, rangeType, true);

        foreach (var t in canTile)
        {
            if (GameManager.Instance.Map[t.X, t.Y].CharacterOnTile is Player)
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
    public IEnumerator CardRoutine(Character caster, Coordinate center)
    {
        Character tmp = GameManager.Instance.Map[center.X, center.Y].CharacterOnTile;

        if (tmp)
        {
            if (interrupted)
            {
                interrupted = false;
                yield break;
            }
            yield return GameManager.Instance.StartCoroutine(caster.HitAttack(tmp, dmg));
            tmp.EffectHandler.DebuffDict[debuffType].SetEffect(debuffValue);
        }
    }
    public void CardRoutineInterrupt()
    {
        interrupted = true;
    }



    // not use
    public int GetRange()
    {
        return range;
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
