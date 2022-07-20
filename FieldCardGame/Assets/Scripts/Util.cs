using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BuffIcon
{
    debug,
}
public enum DebuffIcon
{
    debug,
}
public enum BuffType
{
    Strengthen,
    Shield,
    Will,
    Debug,
}
public enum DebuffType
{
    
}
public enum CostType
{
    PaladinEnergy,
    MageFire,
    MageWater,
    Hp,
    MonsterCrystal,
}
public enum CardType
{
    Move,
    Attack,
    Skill,
}
public class Coordinate
{
    public int X { get; set; }
    public int Y { get; set; }
    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }
    public static Coordinate operator*(Coordinate p1, int scalar)
    {
        return new Coordinate(p1.X * scalar, p1.Y * scalar);
    }
    public static float EuclideanDist(Coordinate p1, Coordinate p2)
    {
        return Mathf.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
    }
    public static int Distance(Coordinate p1, Coordinate p2)
    {
        return Mathf.Abs(p1.X - p2.X) + Mathf.Abs(p1.Y - p2.Y);
    }
    public static Coordinate operator+(Coordinate p1, Coordinate p2)
    {
        return new Coordinate(p1.X + p2.X, p1.Y + p2.Y);
    }
    public static Coordinate operator -(Coordinate p1, Coordinate p2)
    {
        return new Coordinate(p1.X - p2.X, p1.Y - p2.Y);
    }
    public Coordinate GetUpTile()
    {
        Coordinate ret = new Coordinate(X, Y + 1);
        if (OutRange(ret))
        {
            return null;
        }
        return ret;
    }
    public Coordinate GetDownTile()
    {
        Coordinate ret = new Coordinate(X, Y - 1);
        if (OutRange(ret))
        {
            return null;
        }
        return ret;
    }
    public Coordinate GetLeftTile()
    {
        Coordinate ret = new Coordinate(X - 1, Y);
        if (OutRange(ret))
        {
            return null;
        }
        return ret;
    }
    public Coordinate GetRightTile()
    {
        Coordinate ret = new Coordinate(X + 1, Y);
        if (OutRange(ret))
        {
            return null;
        }
        return ret;
  }
  public Coordinate GetUpTilewithoutTest()
  {
    Coordinate ret = new Coordinate(X, Y + 1);
    return ret;
  }
  public Coordinate GetDownTilewithoutTest()
  {
    Coordinate ret = new Coordinate(X, Y - 1);
    return ret;
  }
  public Coordinate GetLeftTilewithoutTest()
  {
    Coordinate ret = new Coordinate(X - 1, Y);
    return ret;
  }
  public Coordinate GetRightTilewithoutTest()
  {
    Coordinate ret = new Coordinate(X + 1, Y);
    return ret;
  }
  public static bool OutRange(Coordinate toTest)
    {
        if (toTest.X < 0 || toTest.X >= 128 || toTest.Y < 0 || toTest.Y >= 128)
        {
            return true;
        }
        return false;
    }
}