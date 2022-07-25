using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockSoulBomb : IPlayerCard
{
    private int range = 0;
    private int cost = 20;
    private int damage = 0;
    private bool interrupted;
    public bool Disposable { get; set; }
    public string ExplainText
    {
        get
        {
            return $"패에 가지고 있는 ‘영혼’카드 개수의 5배만큼 피해를 입힙니다.";
        }
    }
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
        ret.Add(pos);
        return ret;
    }
    public Color GetAvailableTileColor()
    {
        return Color.blue;
    }
    public List<Coordinate> GetAreaofEffect(Coordinate relativePos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        Coordinate pos = new Coordinate(0,0);
        int level = 1;
        bool[,] visited = new bool[128, 128];
        Queue<Coordinate> queue = new Queue<Coordinate>();
        Queue<Coordinate> nextQueue = new Queue<Coordinate>();
        queue.Enqueue(pos);
        while (level++ <= 2)
        {
            while (queue.Count != 0)
            {
                Coordinate tmp = queue.Dequeue();
                if(tmp.X != 0 || tmp.Y != 0)
                    ret.Add(tmp);
                Coordinate tile;
                if ((tile = tmp.GetDownTilewithoutTest()) != null && !visited[tile.X +64, tile.Y +64])
                {
                    visited[tile.X+64, tile.Y+64] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetLeftTilewithoutTest()) != null && !visited[tile.X+64, tile.Y+64])
                {
                    visited[tile.X+64, tile.Y+64] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetRightTilewithoutTest()) != null && !visited[tile.X+64, tile.Y+64])
                {
                    visited[tile.X+64, tile.Y+64] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetUpTilewithoutTest()) != null && !visited[tile.X+64, tile.Y+64] )
                {
                    visited[tile.X+64, tile.Y+64] = true;
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
    public Color GetColorOfEffect(Coordinate pos)
    {
        if (pos.X == 0 && pos.Y == 0)
        {
            return Color.white;
        }
        return Color.red;
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
        List<Coordinate> attack;
        List<Coordinate> available = new List<Coordinate>();
        Character tmp;
        Coordinate pos;
        attack = GetAreaofEffect(target - caster.position);
        for (int i = 0; i<attack.Count;i++)
        {
            pos = attack[i] + target;
            if (Coordinate.OutRange(pos))
            {
                continue;
            }
            if (interrupted)
            {
                interrupted = false;
                yield break;
            }
            tmp = GameManager.Instance.Map[pos.X, pos.Y].CharacterOnTile;
            if (tmp is Player) 
            {
                continue;   
            }
            else if (tmp)
            {
                available.Add(pos);
            }
        }
        int soulCount = 0;
        List<ICard> InHand = caster.HandCard;
        List<ICard> InDiscarded = caster.DiscardedPile;
        List<ICard> InDummy = caster.CardPile;
        foreach(var i in InHand)
        {
            if(i.GetCardID() == 3026001)
            {
                soulCount++;
            }
        }
        foreach(var i in InDiscarded)
        {
            if(i.GetCardID() == 3026001)
            {
                soulCount++;
            }
        }
        foreach(var i in InDummy)
        {
            if(i.GetCardID() == 3026001)
            {
                soulCount++;
            }
        }
        int enemyCount = available.Count;
        if (enemyCount == 0)
            yield break;
        for (int i = 0; i < enemyCount - 1; i++)
        {
            pos = available[i];
            tmp = GameManager.Instance.Map[pos.X, pos.Y].CharacterOnTile;
            GameManager.Instance.StartCoroutine(caster.HitAttack(tmp, soulCount * 5));
        }
        pos = available[enemyCount - 1];
        tmp = GameManager.Instance.Map[pos.X, pos.Y].CharacterOnTile;
        yield return GameManager.Instance.StartCoroutine(caster.HitAttack(tmp, soulCount * 5));
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
        return CardType.Attack;
    }
    public int GetCardID()
    {
        return 3116010;
    }
}
