using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCard : ICard
{
    public Color GetColorOfEffect(Coordinate pos)
    {
        return Color.black;
    }
    public Color GetUnAvailableTileColor()
    {
        return Color.black;
    }
    public Color GetAvailableTileColor()
    {
        return Color.black;
    }
    public int GetRange()
    {
        return 0;
    }
    public void SetRange(int _range)
    {
        return;
    }
    public List<Coordinate> GetAvailableTile(Coordinate pos)
    {
        return null;
    }
    public List<Coordinate> GetAreaofEffect()
    {
        return null;
    }
    public bool IsAvailablePosition(Coordinate caster, Coordinate target)
    {
        return false;
    }
    public IEnumerator CardRoutine(Character caster, Coordinate center)
    {
        yield break;
    }
    public int GetCost()
    {
        return 0;
    }
    public CostType GetCostType()
    {
        return 0;
    }
    public CardType GetCardType()
    {
        return 0;
    }
    public int GetCardID()
    {
        return 0;
    }
}
