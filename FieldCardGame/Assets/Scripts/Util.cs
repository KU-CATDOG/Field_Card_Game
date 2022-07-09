using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Buff
{
    debug,
}
public enum Debuff
{
    debug,
}
public enum BuffType
{

}
public enum CostType
{
    PaladinEnergy,
    MageFire,
    MageWater,
    Hp,
}
public enum CardType
{
    Move
}
public class coordinate
{
    public int X { get; set; }
    public int Y { get; set; }
    public coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }
    public static coordinate operator+(coordinate p1, coordinate p2)
    {
        return new coordinate(p1.X + p2.X, p1.Y + p2.Y);
    }
    public static coordinate operator -(coordinate p1, coordinate p2)
    {
        return new coordinate(p1.X - p2.X, p1.Y - p2.Y);
    }
    public coordinate GetUpTile()
    {
        coordinate ret = new coordinate(X, Y + 1);
        if (OutRange(ret))
        {
            return null;
        }
        return ret;
    }
    public coordinate GetDownTile()
    {
        coordinate ret = new coordinate(X, Y - 1);
        if (OutRange(ret))
        {
            return null;
        }
        return ret;
    }
    public coordinate GetLeftTile()
    {
        coordinate ret = new coordinate(X - 1, Y);
        if (OutRange(ret))
        {
            return null;
        }
        return ret;
    }
    public coordinate GetRightTile()
    {
        coordinate ret = new coordinate(X + 1, Y);
        if (OutRange(ret))
        {
            return null;
        }
        return ret;
    }
    private bool OutRange(coordinate toTest)
    {
        if (toTest.X < 0 || toTest.X >= 128 || toTest.Y < 0 || toTest.Y >= 128)
        {
            return true;
        }
        return false;
    }
}
public class Util
{
}