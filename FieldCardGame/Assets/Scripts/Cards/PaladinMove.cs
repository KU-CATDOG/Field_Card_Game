using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinMove : ICard
{
    private List<coordinate> AreaOfEffect;
    private int range;
    public int GetRange()
    {
        return 3;
    }
    public void SetRange(int _range)
    {
        range = _range;
    }
    public List<coordinate> GetAvailableTile()
    {
        List<coordinate> ret = new List<coordinate>();
        if (AreaOfEffect != null)
        {
            AreaOfEffect.Clear();
        }
        int level = 1;
        bool[,] visited = new bool[128, 128];
        Queue<coordinate> queue = new Queue<coordinate>();
        Queue<coordinate> nextQueue = new Queue<coordinate>();
        queue.Enqueue(GameManager.Instance.Player.position);
        while (level++ <= GetRange())
        {
            while(queue.Count != 0)
            {
                coordinate tmp = queue.Dequeue();
                coordinate tile;
                if ((tile = tmp.GetDownTile()) != null && !visited[tile.X,tile.Y] && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetDownTile()) != null && !visited[tile.X, tile.Y] && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetDownTile()) != null && !visited[tile.X, tile.Y] && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetDownTile()) != null && !visited[tile.X, tile.Y] && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                }
            }
            queue = nextQueue;
            nextQueue.Clear();
        }
        while(nextQueue.Count != 0)
        {
            ret.Add(nextQueue.Dequeue());
        }
        return ret;
    }
    
    private List<coordinate> PathFind(coordinate target)
    {
        List<coordinate> ret = new List<coordinate>();
        for(int i = 1; i <= GetRange(); i++)
        {
            if (dfs(0, i, GameManager.Instance.Player.position, ret, target))
            {
                ret.Reverse();
                return ret;
            } 
        }
        return null;
    }

    private bool dfs(int level, int limit, coordinate now, List<coordinate> path, coordinate target)
    {
        if (level > limit)
            return false;

        if (now.GetDownTile() != null)
        {
            if(dfs(level + 1, limit, now.GetDownTile(), path, target))
            {
                path.Add(now);
                return true;
            }
        }
        if (now.GetUpTile() != null)
        {
            if (dfs(level + 1, limit, now.GetUpTile(), path, target))
            {
                path.Add(now);
                return true;
            }

        }
        if (now.GetLeftTile() != null)
        {
            if (dfs(level + 1, limit, now.GetLeftTile(), path, target))
            {
                path.Add(now);
                return true;
            }

        }
        if (now.GetRightTile() != null)
        {
            if (dfs(level + 1, limit, now.GetRightTile(), path, target))
            {
                path.Add(now);
                return true;
            }
        }
        return false;
    }

    public List<coordinate> GetAreaofEffect()
    {
        List<coordinate> ret = new List<coordinate>();
        ret.Add(new coordinate(0, 0));
        return ret;
    }
    public bool IsAvailablePosition(int x, int y)
    {
        AreaOfEffect = GetAreaofEffect();
        coordinate tmp = new coordinate(x, y);
        if(AreaOfEffect.Exists((i)=>  i.X == tmp.X && i.Y == tmp.Y))
        {
            return true;
        }
        return false;
    }
    public IEnumerator CardRoutine(Character caster, coordinate center)
    {
        
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
