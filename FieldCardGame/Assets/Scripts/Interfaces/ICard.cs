using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
    public int GetCardID();
    public int GetRange();
    public void SetRange(int _range);
    public int GetCost();
    public CostType GetCostType();
    public CardType GetCardType();
    public List<Coordinate> GetAvailableTile(Coordinate pos);
    public bool IsAvailablePosition(Coordinate caster, Coordinate target);
    public List<Coordinate> GetAreaofEffect();
    public IEnumerator CardRoutine(Character caster, Coordinate center);
    public void CardRoutineInterrupt();
}
