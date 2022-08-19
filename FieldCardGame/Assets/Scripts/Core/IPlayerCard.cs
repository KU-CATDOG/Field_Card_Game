using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerCard : ICard
{
    public string ExplainText { get; }
    public Color GetUnAvailableTileColor();
    public Color GetAvailableTileColor();
    public Color GetColorOfEffect(Coordinate pos);
}
