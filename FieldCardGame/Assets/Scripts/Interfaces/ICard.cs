using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
    public int GetRange();
    public List<coordinate> GetUnavailableTile();
    public List<coordinate> GetAreaofEffect();
    public bool IsAvailablePosition(int row, int col);
    public IEnumerator CardRoutine(Character caster);
    public void PayCost();
}
