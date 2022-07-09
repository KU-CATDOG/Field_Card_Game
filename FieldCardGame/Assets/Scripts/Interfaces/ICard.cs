using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
    public int GetRange();
    public void SetRange(int _range);
    public Color GetUnAvailableTileColor();
    public List<coordinate> GetAvailableTile(coordinate pos);
    public Color GetAvailableTileColor();
    public List<coordinate> GetAreaofEffect();
    public Color GetColorOfEffect(coordinate pos);
    public bool IsAvailablePosition(coordinate caster, coordinate target);
    public IEnumerator CardRoutine(Character caster, coordinate center);
    public int GetCost();
    public CostType GetCostType();
    public CardType GetCardType();
    public int GetCardID();
}
