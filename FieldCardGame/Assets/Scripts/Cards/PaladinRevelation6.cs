using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinRevelation6 : IPlayerCard
{
    private int range = 0;

    public bool Disposable { get; set; } = true;
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
        yield return caster.StartCoroutine(caster.AddCard(new PaladinRevelation7()));
    }

    public void CardRoutineInterrupt()
    {
    }

    public int GetCost()
    {
        return 1;
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
        return 1214001;
    }
}
