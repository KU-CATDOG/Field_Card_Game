using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public Coordinate GetPosition();
    public void Interaction();
}
