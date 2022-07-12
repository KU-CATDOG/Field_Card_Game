using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockSnatch : IPlayerCard
{
    private int range;
    public int GetRange()
    {
        //return range;
        return 1;
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
    public List<Coordinate> GetAreaofEffect()
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
        //give damage to enemy which is on target position
        //get Hp
        caster.
    }
    public int GetCost()
    {
        return 15;
    }
    public CostType GetCostType()
    {
        return CostType.Hp;
    }
    public CardType GetCardType()
    {
        return CardType.Attack;
    }
    public int GetCardID()
    {
        return 3002010;
    }

}
