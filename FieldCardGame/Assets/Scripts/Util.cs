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
    Illusion,
    Regeneration,
    Growth,
    Debug,
}
public enum DebuffType
{
    Stun,
    Weakness,
    Poison,
    Fragility,
    Rooted,
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
    public int distance { get; set; }
    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }
    public Coordinate(int x, int y, int _distance)
    {
        X = x;
        Y = y;
        distance = _distance;
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

    public List<Coordinate> GetDistanceAvailableTile(int _distance)
    {
        List<Coordinate> ret = new List<Coordinate> ();
        List<string> path = new List<string>();

        ret.AddRange(findTileInRange(_distance, new Coordinate(X, Y)));

        return ret;
    }

    public List<Coordinate> findTileInRange(int _distance, Coordinate _pos)
    {
        List<Coordinate> ret = new List<Coordinate>();

        Queue<Coordinate> checkNext = new Queue<Coordinate>();
        Queue<Coordinate> checkNow = new Queue<Coordinate>();

        int[][] dirs = new int[][] 
        {
            new int[] {0, 1},
            new int[] {0, -1},
            new int[] {1, 0},
            new int[] {-1, 0},
        };

        _pos.distance = 0;
        checkNow.Enqueue(_pos);

        // 검사 시작
        while (checkNow.Count > 0)
        {
            Coordinate t = checkNow.Dequeue();
            for (int i = 0; i < 4; ++i)
            {
                Coordinate next = new Coordinate(t.X + dirs[i][0], t.Y + dirs[i][1], 0);

                if (OutRange(next) || next.distance <= t.distance + 1)
                    continue;

                // 이동 가능 거리 내에 있는 타일인지 검사.
                if (GameManager.Instance.Map[next.X, next.Y].CharacterOnTile != null)
                {
                    next.distance = t.distance + 1;
                    checkNext.Enqueue(next);
                    ret.Add(new Coordinate(next.X, next.Y));
                }
            }

            if (checkNow.Count == 0)
            {
                checkNow = checkNext;
                checkNext.Clear();
            }
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