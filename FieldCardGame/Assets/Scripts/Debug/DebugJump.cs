using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugJump : IPlayerCard
{

    private int range = 20;
    private int cost = 1;
    public string ExplainText
    {
        get
        {
            return "DEBUG JUMP";
        }
    }
    public bool Disposable { get; set; }

    private bool interrupted = false;
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
                if ((tmp.X != pos.X || tmp.Y != pos.Y) && !(GameManager.Instance.Map[tmp.X, tmp.Y].CharacterOnTile is Enemy))
                    ret.Add(tmp);
                Coordinate tile;
                if ((tile = tmp.GetDownTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetLeftTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetRightTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetUpTile()) != null && !visited[tile.X, tile.Y])
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
            Coordinate tmp = queue.Dequeue();
            if (!(GameManager.Instance.Map[tmp.X, tmp.Y].CharacterOnTile is Enemy))
                ret.Add(tmp);
        }
        return ret;
    }

    public Color GetAvailableTileColor()
    {
        return Color.blue;
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
        caster.StartCoroutine(JumpRoutine(caster, target, 3));
        yield return caster.StartCoroutine(caster.Move(target, 5f));
    }
    private IEnumerator JumpRoutine(Character caster, Coordinate target, float height)
    {
        float distance = Coordinate.EuclideanDist(caster.position, target);
        float slope = 4 * (height - 1) / distance / distance;
        while(caster.position != target && !caster.MoveInterrupted)
        {
            float x = Mathf.Sqrt(Mathf.Pow(target.X - caster.transform.position.x, 2) + Mathf.Pow(target.Y - caster.transform.position.z, 2));
            caster.transform.position = new Vector3(caster.transform.position.x, height - slope * (x - distance/2) * (x - distance/2),caster.transform.position.z);
            yield return new WaitForFixedUpdate();
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
        return CostType.PaladinEnergy;
    }

    public CardType GetCardType()
    {
        return CardType.Skill;
    }

    public int GetCardID()
    {
        return 123;
    }
}
