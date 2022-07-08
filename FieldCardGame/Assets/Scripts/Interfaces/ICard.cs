using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
    public int GetRange();
    public List<coordinate> GetAvailableTile();
    public List<coordinate> GetAreaofEffect();
    public bool IsAvailablePosition(int x, int y);
    public IEnumerator CardRoutine(Character caster, coordinate center);
    public int GetCost();
    public CostType GetCostType();
    public CardType GetCardType();
    public int GetCardID();
    public void SetRange(int _range);
}
