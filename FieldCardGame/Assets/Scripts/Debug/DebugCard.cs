using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCard : ICard
{
    public Color GetColorOfEffect(coordinate pos)
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
    public List<coordinate> GetAvailableTile(coordinate pos)
    {
        return null;
    }
    public List<coordinate> GetAreaofEffect()
    {
        return null;
    }
    public bool IsAvailablePosition(coordinate caster, coordinate target)
    {
        return false;
    }
    public IEnumerator CardRoutine(Character caster, coordinate center)
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
