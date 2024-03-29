using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockBloodLine : IPlayerCard
{
    private int range = 0;
    private int cost = 10;
    private bool notRemoved = true;
    private bool interrupted;
    public bool Disposable { get; set; }
    public string ExplainText
    {
        get
        {
            return $"이번 턴 동안 받은 피해 횟수 당 사거리가 1 증가합니다. 최대 {range}칸 이동합니다.";
        }
    }
    public IEnumerator RemoveCardRoutine(Character owner)
    {
        notRemoved = false;
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
    public Color GetUnAvailableTileColor()
    {
        return Color.red;
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
                if (tmp.X != pos.X || tmp.Y != pos.Y)
                    ret.Add(tmp);
                Coordinate tile;
                if ((tile = tmp.GetDownTile()) != null && !visited[tile.X, tile.Y] && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetLeftTile()) != null && !visited[tile.X, tile.Y] && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetRightTile()) != null && !visited[tile.X, tile.Y] && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetUpTile()) != null && !visited[tile.X, tile.Y] && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                }
            }
            queue = new Queue<Coordinate>(nextQueue);
            nextQueue.Clear();
        }
        while (queue.Count != 0)
        {
            ret.Add(queue.Dequeue());
        }
        return ret;
    }
    public Color GetAvailableTileColor()
    {
        return Color.blue;
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
    public Color GetColorOfEffect(Coordinate pos)
    {
        if (pos.X == 0 && pos.Y == 0)
        {
            return Color.white;
        }
        return Color.black;
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
        SetRange(0);
    }
    public IEnumerator GetCardRoutine(Character owner)
    {
        owner.AddGetDmgRoutine(AddRange(owner), 0);
        yield break;
    }
    private IEnumerator AddRange(Character owner)
    {
        while (notRemoved)
        {
            if (owner.HandCard.Contains(this))
            {
                SetRange(GetRange() + 1);
            }
            yield return null;
        }
    }
    public void CardRoutineInterrupt()
    {
        interrupted = true;
    }
    public int GetCost()
    {
        return cost;
    }
    public void SetCost(int _cost)
    {
        cost = _cost;
    }
    public CostType GetCostType()
    {
        return CostType.Hp;
    }
    public CardType GetCardType()
    {
        return CardType.Move;
    }
    public int GetCardID()
    {
        return 3143100;
    }

}
