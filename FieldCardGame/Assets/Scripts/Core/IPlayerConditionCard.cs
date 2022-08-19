using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerConditionCard : IPlayerCard
{
    public bool isSatisfied(Coordinate target);
    public Color SatisfiedAreaColor();
}
