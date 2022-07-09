using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinMove : ICard
{
    private int range;
    public int GetRange()
    {
        return 10;
    }
    public void SetRange(int _range)
    {
        range = _range;
    }
    public Color GetUnAvailableTileColor()
    {
        return Color.red;
    }
    public List<coordinate> GetAvailableTile(coordinate pos)
    {
        List<coordinate> ret = new List<coordinate>();
        int level = 1;
        bool[,] visited = new bool[128, 128];
        Queue<coordinate> queue = new Queue<coordinate>();
        Queue<coordinate> nextQueue = new Queue<coordinate>();
        queue.Enqueue(pos);
        while (level++ <= GetRange())
        {
            while (queue.Count != 0)
            {
                coordinate tmp = queue.Dequeue();
                if (tmp.X != pos.X || tmp.Y != pos.Y)
                    ret.Add(tmp);
                coordinate tile;
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
            queue = new Queue<coordinate>(nextQueue);
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

    private List<coordinate> FindPath(coordinate from, coordinate to)
    {
        bool[,] visited = new bool[128, 128];
        List<coordinate> ret = new List<coordinate>();
        for (int i = 1; i <= GetRange(); i++)
        {
            if (dfs(0, i, from, ret, to, visited))
            {
                ret.RemoveAt(ret.Count - 1);
                ret.Reverse();
                ret.Add(to);
                return ret;
            }
        }
        return null;
    }

    private bool dfs(int level, int limit, coordinate now, List<coordinate> path, coordinate target, bool[,] visited)
    {
        if (level > limit)
        {
            return false;
        }
        if (target.X == now.X && target.Y == now.Y)
        {
            return true;
        }

        visited[now.X, now.Y] = true;
        coordinate tile;
        if ((tile = now.GetDownTile()) != null && !visited[tile.X, tile.Y])
        {
            if (dfs(level + 1, limit, now.GetDownTile(), path, target, visited))
            {
                path.Add(now);
                return true;
            }
        }
        if ((tile = now.GetUpTile()) != null && !visited[tile.X, tile.Y])
        {
            if (dfs(level + 1, limit, now.GetUpTile(), path, target, visited))
            {
                path.Add(now);
                return true;
            }

        }
        if ((tile = now.GetLeftTile()) != null && !visited[tile.X, tile.Y])
        {
            if (dfs(level + 1, limit, now.GetLeftTile(), path, target, visited))
            {
                path.Add(now);
                return true;
            }

        }
        if ((tile = now.GetRightTile()) != null && !visited[tile.X, tile.Y])
        {
            if (dfs(level + 1, limit, now.GetRightTile(), path, target, visited))
            {
                path.Add(now);
                return true;
            }
        }
        visited[now.X, now.Y] = false;
        return false;
    }

    public List<coordinate> GetAreaofEffect()
    {
        List<coordinate> ret = new List<coordinate>();
        ret.Add(new coordinate(0, 0));
        return ret;
    }
    public Color GetColorOfEffect(coordinate pos)
    {
        if (pos.X == 0 && pos.Y == 0)
        {
            return Color.white;
        }
        return Color.black;
    }
    public bool IsAvailablePosition(coordinate caster, coordinate target)
    {
        List<coordinate> availablePositions = GetAvailableTile(caster);
        if (availablePositions.Exists((i) => i.X == target.X && i.Y == target.Y))
        {
            return true;
        }
        return false;
    }
    public IEnumerator CardRoutine(Character caster, coordinate target)
    {
        List<coordinate> path;
        path = FindPath(caster.position, target);
        float speed = 5f;
        foreach (coordinate i in path)
        {
            yield return GameManager.Instance.StartCoroutine(caster.Move(i, speed));
        }
    }
    public int GetCost()
    {
        return 0;
    }
    public CostType GetCostType()
    {
        return CostType.PaladinEnergy;
    }
    public CardType GetCardType()
    {
        return CardType.Move;
    }
    public int GetCardID()
    {
        return 1;
    }

}
