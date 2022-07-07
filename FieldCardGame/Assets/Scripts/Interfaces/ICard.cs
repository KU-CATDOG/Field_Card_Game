using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
    public int GetRange();
    public List<coordinate> GetUnavailableTile();
    public List<coordinate> GetAreaofEffect();
    public bool IsAvailablePosition(int row, int col);
    public IEnumerator CardRoutine(Character caster, coordinate center);
    public int GetCost();
    public CostType GetCostType();
    public CardType GetCardType();
    public int GetCardID();
}
