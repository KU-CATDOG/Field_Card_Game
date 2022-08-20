using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinAgillity : IPlayerCard
{
    public bool Disposable { get; set; }
    private int range = 2;
    private int cost = 3;
    private int damage = 10;
    private bool interrupted;
    private bool notRemoved = true;
    public string ExplainText
    {
        get
        {
            return $"피해를 {GetDamage()} 줍니다. 이번 턴에 사용한 이동 카드 하나 당 비용이 1 감소하고, 피해량이 5 증가합니다.";
        }
    }
    public int GetRange()
    {
        return range;
    }
    public void SetRange(int _range)
    {
        range = _range;
    }
    public int GetDamage()
    {
        return damage;
    }
    public void SetDamage(int _damage)
    {
        damage = _damage;
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
                if ((tmp.X != pos.X || tmp.Y != pos.Y) && GameManager.Instance.Map[tmp.X, tmp.Y].CharacterOnTile)
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
            if (GameManager.Instance.Map[tmp.X, tmp.Y].CharacterOnTile is Enemy)
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
        Character tmp = GameManager.Instance.Map[target.X, target.Y].CharacterOnTile;
        if (tmp)
        {
            if (interrupted)
            {
                interrupted = false;
                yield break;
            }
            int moveCount = caster.moveCardUseInTurn;
            yield return GameManager.Instance.StartCoroutine(caster.HitAttack(tmp, GetDamage()+5 * moveCount));
        }
        yield break;
    }
    
    public IEnumerator GetCardRoutine(Character owner)
    {
        Debug.Log(owner.moveCardUseInTurn);

        owner.AddCardUseRoutine(ReduceCost(owner), 0);
        yield break;
    }

    private IEnumerator ReduceCost(Character owner)
    {

        while (notRemoved)
        {
            if (owner.HandCard.Contains(this))
            {
                if (owner.moveCardUseInTurn <= 3)
                    SetCost(3 - owner.moveCardUseInTurn);
                else
                    SetCost(0);
            }
            yield return null;
        }
    }
    public IEnumerator RemoveCardRoutine(Character owner)
    {
        notRemoved = false;
        yield break;
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
        return CardType.Attack;
    }
    public int GetCardID()
    {
        return 1041001;
    }
}

