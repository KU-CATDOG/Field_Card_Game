using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinAdvance : IPlayerCard
{
    private int range = 3;
    private bool interrupted;
    private int cost = 0;
    public bool Disposable { get; set; }

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
        Coordinate tile = pos;
        for(int i = 1;i<4;i++)
        {
            if ((tile = tile.GetDownTile()) != null && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                ret.Add(tile);
            else
            {
                break;
            }
        }
        tile = pos;
        for (int i = 1; i < 4; i++)
        {
            if ((tile = tile.GetLeftTile()) != null && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                ret.Add(tile);
            else
            {
                break;
            }
        }
        tile = pos;
        for (int i = 1; i < 4; i++)
        {
            if ((tile = tile.GetRightTile()) != null && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                ret.Add(tile);
            else
            {
                break;
            }
        }
        tile = pos;
        for (int i = 1; i < 4; i++)
        {
            if ((tile = tile.GetUpTile()) != null && !GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                ret.Add(tile);
            else
            {
                break;
            }
        }

        return ret;
    }

    private List<Coordinate> FindPath(Coordinate from, Coordinate to)
    {
        List<Coordinate> ret = new List<Coordinate>();
        Coordinate direction = to - from;
        int limit;
        if(direction.X==0)
        {
            limit = direction.Y;
            if(limit>0)
            {
                for(int i =1;i<=limit;i++)
                {
                    ret.Add(new Coordinate(from.X, from.Y + i));
                }
            }
            else
            {
                for (int i = -1; i >= limit; i--)
                {
                    ret.Add(new Coordinate(from.X, from.Y + i));
                }
            }
        }
        else
        {
            limit = direction.X;
            if (limit > 0)
            {
                for (int i = 1; i <= limit; i++)
                {
                    ret.Add(new Coordinate(from.X+i, from.Y));
                }
            }
            else
            {
                for (int i = -1; i >= limit; i--)
                {
                    ret.Add(new Coordinate(from.X + i, from.Y));
                }
            }
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
        return cost;
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
        return 1018100;
    }
}
