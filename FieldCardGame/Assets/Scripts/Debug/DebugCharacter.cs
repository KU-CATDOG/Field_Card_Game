using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCharacter : Character
{
    public override IEnumerator AfterBuff()
    {
        throw new System.NotImplementedException();
    }
    public override IEnumerator AfterDraw()
    {
        throw new System.NotImplementedException();
    }
    public override IEnumerator AwakeTurn()
    {
        throw new System.NotImplementedException();
    }
    public override bool Equals(object other)
    {
        return base.Equals(other);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public override IEnumerator StartTurn()
    {
        throw new System.NotImplementedException();
    }
    public override string ToString()
    {
        return base.ToString();
    }
}
