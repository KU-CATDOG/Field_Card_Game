using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : ICard
{
    public bool Disposable { get; set; }

    private int cost;
    private int range;
    private int minRange = 1;
    private RangeType rangeType = RangeType.Distance;
    public int _cost
    {
        get { return cost; }
        set { cost = value; }
    }
    public int _range
    {
        get { return range; }
        set { range = value; }
    }

    public RangeType _rangeType
    {
        get { return rangeType; }
        set { rangeType = value; }
    }


    private bool interrupted;
    public IEnumerator GetCardRoutine(Character owner)
    {
        yield break;
    }
    public IEnumerator RemoveCardRoutine(Character owner)
    {
        yield break;
    }

    public int GetCardID()
    {
        return 0002223;
    }
    
    public CostType GetCostType()
    {
        return CostType.MonsterCrystal;
    }
    public CardType GetCardType()
    {
        return CardType.Move;
    }
    public List<Coordinate> GetAvailableTile(Coordinate pos)
    {

        return pos.GetDistanceAvailableTile(range, rangeType, false, false, minRange);
    }   
    public bool IsAvailablePosition(Coordinate caster, Coordinate target)
    {
        List<Coordinate> availablePositions = GetAvailableTile(caster);
        if (availablePositions.Exists((i) => i.X == target.X && i.Y == target.Y))
        {
            return true;
        }
        return false;
    }
    public List<Coordinate> GetAreaofEffect(Coordinate relativePos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        ret.Add(new Coordinate(0, 0));
        return ret;
    }
    private List<Coordinate> backTracking(int limit, Coordinate center, Coordinate target)
    {
        List<Coordinate> ret = new List<Coordinate>();
        int level = 1;
        int[,] direction = new int[128, 128];
        Queue<Coordinate> queue = new Queue<Coordinate>();
        Queue<Coordinate> nextQueue = new Queue<Coordinate>();
        queue.Enqueue(center);
        while (level++ <= limit)
        {
            while (queue.Count != 0)
            {
                Coordinate tmp = queue.Dequeue();
                Coordinate tile;
                if ((tile = tmp.GetDownTile()) != null && direction[tile.X, tile.Y] == 0 && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                {
                    if (tile.X == target.X && tile.Y == target.Y)
                    {
                        ret.Add(target);
                        Coordinate i = tmp;
                        while (i.X != center.X || i.Y != center.Y)
                        {
                            ret.Add(i);
                            switch (direction[i.X, i.Y])
                            {
                                case 1:
                                    i = i.GetLeftTile();
                                    break;
                                case 2:
                                    i = i.GetDownTile();
                                    break;
                                case 3:
                                    i = i.GetRightTile();
                                    break;
                                case 4:
                                    i = i.GetUpTile();
                                    break;
                            }
                        }
                        return ret;
                    }
                    direction[tile.X, tile.Y] = 4;
                    nextQueue.Enqueue(tile);
                }
                if ((tile = tmp.GetLeftTile()) != null && direction[tile.X, tile.Y] == 0 && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                {
                    if (tile.X == target.X && tile.Y == target.Y)
                    {
                        ret.Add(target);
                        Coordinate i = tmp;
                        while (i.X != center.X || i.Y != center.Y)
                        {
                            ret.Add(i);
                            switch (direction[i.X, i.Y])
                            {
                                case 1:
                                    i = i.GetLeftTile();
                                    break;
                                case 2:
                                    i = i.GetDownTile();
                                    break;
                                case 3:
                                    i = i.GetRightTile();
                                    break;
                                case 4:
                                    i = i.GetUpTile();
                                    break;
                            }
                        }
                        return ret;
                    }
                    direction[tile.X, tile.Y] = 3;
                    nextQueue.Enqueue(tile);
                }
                if ((tile = tmp.GetRightTile()) != null && direction[tile.X, tile.Y] == 0 && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                {
                    if (tile.X == target.X && tile.Y == target.Y)
                    {
                        ret.Add(target);
                        Coordinate i = tmp;
                        while (i.X != center.X || i.Y != center.Y)
                        {
                            ret.Add(i);
                            switch (direction[i.X, i.Y])
                            {
                                case 1:
                                    i = i.GetLeftTile();
                                    break;
                                case 2:
                                    i = i.GetDownTile();
                                    break;
                                case 3:
                                    i = i.GetRightTile();
                                    break;
                                case 4:
                                    i = i.GetUpTile();
                                    break;
                            }
                        }
                        return ret;
                    }
                    direction[tile.X, tile.Y] = 1;
                    nextQueue.Enqueue(tile);
                }
                if ((tile = tmp.GetUpTile()) != null && direction[tile.X, tile.Y] == 0 && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                {
                    if (tile.X == target.X && tile.Y == target.Y)
                    {
                        ret.Add(target);
                        Coordinate i = tmp;
                        while (i.X != center.X || i.Y != center.Y)
                        {
                            ret.Add(i);
                            switch (direction[i.X, i.Y])
                            {
                                case 1:
                                    i = i.GetLeftTile();
                                    break;
                                case 2:
                                    i = i.GetDownTile();
                                    break;
                                case 3:
                                    i = i.GetRightTile();
                                    break;
                                case 4:
                                    i = i.GetUpTile();
                                    break;
                            }
                        }
                        return ret;
                    }
                    direction[tile.X, tile.Y] = 2;
                    nextQueue.Enqueue(tile);
                }
            }
            queue = new Queue<Coordinate>(nextQueue);
            nextQueue.Clear();
        }
        return null;
    }

    private List<Coordinate> FindPath(Coordinate from, Coordinate to)
    {
        List<Coordinate> ret;
        ret = backTracking(range, from, to);
        if (ret != null)
        {
            ret.Reverse();
            return ret;
        }
        return null;
    }
    public IEnumerator CardRoutine(Character caster, Coordinate center)
    {
        List<Coordinate> path;
        path = FindPath(caster.position, center);
        float speed = 5f;
        if (path == null) yield break;
        foreach (Coordinate i in path)
        {
            if (interrupted)
            {
                interrupted = false;
                yield break;
            }
            yield return GameManager.Instance.StartCoroutine(caster.Move(i, speed));
        }
    }
    public void CardRoutineInterrupt()
    {
        interrupted = true;
    }

    public void SetMinRange(int value)
    {
        minRange = value;
    }


    // not use
    public int GetRange()
    {
        return 1;
    }
    public void SetRange(int _range)
    {
        range = _range;
    }
    public int GetCost()
    {
        return cost;
    }
    public void SetCost(int _cost)
    {
        cost = _cost;
    }

}
