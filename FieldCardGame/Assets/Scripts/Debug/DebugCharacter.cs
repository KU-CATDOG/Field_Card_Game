using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCharacter : Character
{
    protected override IEnumerator getDmg(int dmg)
    {
        throw new System.NotImplementedException();
    }
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
    protected override IEnumerator dieRoutine()
    {
        throw new System.NotImplementedException();
    }
    protected override IEnumerator payCost(int cost, CostType type)
    {
        throw new System.NotImplementedException();
    }
    public override bool PayTest(int cost, CostType type)
    {
        throw new System.NotImplementedException();
    }
}
