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
    Heal,
    Debug,
    Length,
}
public enum DebuffType
{
    Stun,
    Weakness,
    Poison,
    Fragility,
    Rooted,
    Vampire,
    Length,
}
public enum CostType
{
    PaladinEnergy,
    MageFire,
    MageWater,
    Hp,
    MonsterCrystal,
    Unpayable,
}
public enum RangeType
{
   CrossWise,
   DiagonalWise,
   Distance,
   Square,
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

    public List<Coordinate> GetDistanceAvailableTile(int _distance, RangeType rangeType, bool _forATK, bool isMax = false)
    {
        List<Coordinate> ret = new List<Coordinate> ();
        
        switch (rangeType)
        {
            case RangeType.CrossWise:
                ret.AddRange(findTileInCross(_distance, new Coordinate(X, Y, 0), _forATK, isMax));
                break;
            case RangeType.DiagonalWise:

                break;
            case RangeType.Distance:
                ret.AddRange(findTileInRange(_distance, new Coordinate(X, Y, 0), _forATK, isMax));
                break;
            case RangeType.Square:

                break;
            default:

                break;
        }
        
        return ret;
    }
    public List<Coordinate> findTileInCross(int _distance, Coordinate _pos, bool forATK, bool isMax)
    {
        List<Coordinate> ret = new List<Coordinate>();



        int[][] dirs = new int[][]
        {
            new int[] {0, 1},
            new int[] {0, -1},
            new int[] {1, 0},
            new int[] {-1, 0},
        };

        for (int j = 0; j < dirs.Length; j++)
        {
            Coordinate nextPos = new Coordinate(_pos.X, _pos.Y, 0);

            for (int i = isMax ? _distance - 1 : 0; i < _distance; i++)
            {
                nextPos.X = _pos.X + dirs[j][0] * i;
                nextPos.Y = _pos.Y + dirs[j][1] * i;

                if(OutRange(nextPos))
                {
                    break;
                }

                // �̵� ���� �Ÿ� ���� �ִ� Ÿ������ �˻�.
                if (forATK)
                {
                        ret.Add(new Coordinate(nextPos.X, nextPos.Y));
                }
                else
                {
                    if (GameManager.Instance.Map[nextPos.X, nextPos.Y].CharacterOnTile == null)
                    {
                        ret.Add(new Coordinate(nextPos.X, nextPos.Y));
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        return ret;
    }
    public List<Coordinate> findTileInRange(int _distance, Coordinate _pos, bool forATK, bool isMax)
    {
        List<Coordinate> ret = new List<Coordinate>();
        List<Coordinate> useTile = new List<Coordinate>();

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
        useTile.Add(_pos);

        // �˻� ����
        while (checkNow.Count > 0)
        {
            Coordinate t = useTile[findTileIDX(useTile, checkNow.Dequeue())];

            if (t.distance == _distance)
            {
                continue;
            }

            for (int i = 0; i < 4; ++i)
            {
                Coordinate tem = new Coordinate(t.X + dirs[i][0], t.Y + dirs[i][1], int.MaxValue);

                if (OutRange(tem))
                {
                    continue;
                }

                int id = -1;

                if ((id = findTileIDX(useTile, tem)) < 0)
                {
                    useTile.Add(tem);
                    id = useTile.Count - 1;
                }

                //Debug.Log(tem.X);
                //Debug.Log(tem.Y);
                //Debug.Log(useTile.Count);

                if(useTile[id].distance <= t.distance + 1)
                    continue;

                // �̵� ���� �Ÿ� ���� �ִ� Ÿ������ �˻�.
                // need check. what is this function. just find all avaliable tile(no enemy(player))? or find only correct tile(tile on enemy)?
                if (forATK)
                {
                    useTile[id].distance = t.distance + 1;
                    checkNext.Enqueue(useTile[id]);
                    ret.Add(new Coordinate(useTile[id].X, useTile[id].Y));   
                }
                else
                {
                    if (GameManager.Instance.Map[useTile[id].X, useTile[id].Y].CharacterOnTile == null)
                    {   
                        useTile[id].distance = t.distance + 1;
                        checkNext.Enqueue(useTile[id]);
                        ret.Add(new Coordinate(useTile[id].X, useTile[id].Y));
                    }
                }
                
            }

            if (checkNow.Count == 0)
            {
                SwapReference(ref checkNow, ref checkNext);
            }
        }
        return ret;
    }
    void SwapReference(ref Queue<Coordinate> a, ref Queue<Coordinate> b)
    {
        Queue<Coordinate> temp = a;
        a = b;
        b = temp;
    }

    public int findTileIDX(List<Coordinate> useTile, Coordinate nextTile)
    {
        for (int i = 0; i < useTile.Count; i++)
        {
            if(useTile[i].X == nextTile.X && useTile[i].Y == nextTile.Y)
            {
                return i;
            }
        }
        return -1;
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
        if (toTest.X < 0 || toTest.X >= GameManager.MAPSIZE || toTest.Y < 0 || toTest.Y >= GameManager.MAPSIZE)
        {
            return true;
        }
        return false;
    }
}