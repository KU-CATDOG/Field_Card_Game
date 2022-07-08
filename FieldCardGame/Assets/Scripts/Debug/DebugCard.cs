using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCard : ICard
{
    public int GetRange()
    {
        return 0;
    }
    public void SetRange(int _range)
    {
        return;
    }
    public List<coordinate> GetAvailableTile()
    {
        return null;
    }
    public List<coordinate> GetAreaofEffect()
    {
        return null;
    }
    public bool IsAvailablePosition(int row, int col)
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
