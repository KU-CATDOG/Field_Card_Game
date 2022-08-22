using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveCard
{
    public static bool OnRise { get; set; } = false;
    public int Range
    {
        get
        {
            return (this as ICard).GetRange() + (OnRise ? 1 : 0);
        }
    }
}
