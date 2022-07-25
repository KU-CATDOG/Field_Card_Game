using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenRMove : ICard
{
    private int range = 6;
    private int cost = 1;
    private bool interrupted;
    public bool Disposable { get; set; }
    public IEnumerator GetCardRoutine(Character owner)
    {
        yield break;
    }
    public IEnumerator RemoveCardRoutine(Character owner)
    {
        yield break;
    }
    public int GetRange()
    {
        return range;
    }
    public void SetRange(int _range)
    {
        range = _range;
    }
    public List<Coordinate> GetAvailableTile(Coordinate pos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        int level = 1;
        bool[,] visited = new bool[128, 128];
        Queue<Coordinate> queue = new Queue<Coordinate>();
        Queue<Coordinate> nextQueue = new Queue<Coordinate>();
        queue.Enqueue(pos);
        while (level++ <= GetRange())
        {
            while (queue.Count != 0)
            {
                Coordinate tmp = queue.Dequeue();
                if ((tmp.X != pos.X || tmp.Y != pos.Y) && (tmp.X == pos.X || tmp.Y == pos.Y))
                    ret.Add(tmp);
                Coordinate tile;
                if ((tile = tmp.GetDownTile()) != null && !visited[tile.X, tile.Y] && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                {
                    visited[tile.X, tile.Y] = true;
                    if(tile.X == pos.X || tile.Y == pos.Y)
                        nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetLeftTile()) != null && !visited[tile.X, tile.Y] && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile )
                {
                    visited[tile.X, tile.Y] = true;
                    if (tile.X == pos.X || tile.Y == pos.Y)
                        nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetRightTile()) != null && !visited[tile.X, tile.Y] && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile )
                {
                    visited[tile.X, tile.Y] = true;
                    if (tile.X == pos.X || tile.Y == pos.Y)
                        nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetUpTile()) != null && !visited[tile.X, tile.Y] && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile )
                {
                    visited[tile.X, tile.Y] = true;
                    if (tile.X == pos.X || tile.Y == pos.Y)
                        nextQueue.Enqueue(tile);
                }
            }
            queue = new Queue<Coordinate>(nextQueue);
            nextQueue.Clear();
        }
        while (queue.Count != 0)
        {
            Coordinate tmp = queue.Dequeue();
            if (tmp.X == pos.X || tmp.Y == pos.Y)
                ret.Add(tmp);
        }
        return ret;
    }

    private List<Coordinate> FindPath(Coordinate from, Coordinate to)
    {
        List<Coordinate> ret;
        ret = backTracking(GetRange(), from, to);
        if (ret != null)
        {
            ret.Reverse();
            return ret;
        }
        return null;
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
    public List<Coordinate> GetAreaofEffect(Coordinate relativePos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        ret.Add(new Coordinate(0, 0));
        return ret;
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
    public IEnumerator CardRoutine(Character caster, Coordinate target)
    {
        List<Coordinate> path;
        path = FindPath(caster.position, target);
        float speed = 5f;
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
    public int GetCost()
    {
        return 0;
    }
    public void SetCost(int _cost)
    {
        cost = _cost;
    }
    public CostType GetCostType()
    {
        return CostType.MonsterCrystal;
    }
    public CardType GetCardType()
    {
        return CardType.Move;
    }
    public int GetCardID()
    {
        return 0098100;
    }

}
