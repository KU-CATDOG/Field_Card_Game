using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockSoulLiberation : IPlayerCard,IAttackCard,IHealCard
{
    public bool Disposable { get; set; }
    private int range = 3;
    private int cost = 30;
    private int damage = 10;
    private int amount = 10;
    public List<int> Damage
    {
        get
        {
            List<int> tmp = new();
            tmp.Add(damage);
            return tmp;
        }
    }
    public void SetDmg(int value)
    {
        damage = damage + value < 0 ? 0 : damage + value;
        Damage[0] = damage;
    }
    public List<int> HealAmounts
    {
        get
        {
            List<int> tmp = new();
            tmp.Add(amount);
            return tmp;
        }
    }
    public void SetHealAmount(int value)
    {
        amount = amount + value < 0 ? 0 : amount + value;
        HealAmounts[0] = amount;
    }
    private bool interrupted;
    public string ExplainText
    {
        get
        {
            return $"뽑을 카드 더미에 존재하는 모든 ‘영혼’을 제거하고, 그 수 만큼 피해를 {damage} 주고, 체력을 {amount} 회복하고, 카드를 뽑습니다.";
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
    public int GetAmount()
    {
        return amount;
    }
    public void SetAmount(int _amount)
    {
        amount = _amount;
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
                if(tmp.X != pos.X || tmp.Y != pos.Y)
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
            if (tmp.X != pos.X || tmp.Y != pos.Y)
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
        if (interrupted)
        {
             interrupted = false;
             yield break;
        }
        int cardCount = caster.CardPile.Count;
        int soulCount = 0;
        for (int i = cardCount - 1; i >= 0; i--)
        {
            if (caster.CardPile[i].GetCardID() == 3027001)
            {
                soulCount++;
                caster.CardPile.RemoveAt(i);
            }
        }
        Character tmp = GameManager.Instance.Map[target.X, target.Y].CharacterOnTile;
        if (tmp)
        {
            for (int i = 0; i < soulCount; i++)
            {
                yield return GameManager.Instance.StartCoroutine(caster.HitAttack(tmp, GetDamage()));
            }
        }
        for (int i = 0; i < soulCount; i++)
        {
            yield return GameManager.Instance.StartCoroutine(caster.GiveHeal(caster, GetAmount(), true));
            yield return caster.DrawCard();
        }
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
        return CostType.Hp;
    }
    public CardType GetCardType()
    {
        return CardType.Attack;
    }
    public int GetCardID()
    {
        return 3232010;
    }

}
