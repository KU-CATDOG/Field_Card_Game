using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
    public bool Disposable { get; set; }
    public int GetCardID();
    public int GetRange();
    public void SetRange(int _range);
    public int GetCost();
    public void SetCost(int _cost);
    public CostType GetCostType();
    public List<Coordinate> GetAvailableTile(Coordinate pos);
    public bool IsAvailablePosition(Coordinate caster, Coordinate target);
    public List<Coordinate> GetAreaofEffect(Coordinate relativePos);
    public IEnumerator CardRoutine(Character caster, Coordinate center);
    public void CardRoutineInterrupt();
    public IEnumerator GetCardRoutine(Character owner);
}
