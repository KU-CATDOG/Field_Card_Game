using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinFullness : IPlayerCard
{
    private int range = 3;
    private int cost = 1;
    private bool notRemoved = true;
    private int damage = 0;
    private bool interrupted;
    public bool Disposable { get; set; }
    public string ExplainText
    {
        get
        {
            return $"이번 턴의 회복량({GetDamage()})만큼 피해를 줍니다. 현재 체력이 최대라면, 2배의 피해를 줍니다.";
        }
    }
    public IEnumerator GetCardRoutine(Character owner)
    {
        owner.AddTryHealRoutine(IncreaseDmg(owner), 0);
        owner.AddTurnEndDebuff(ResetDmg(owner), 0);
        yield break;
    }
    private IEnumerator IncreaseDmg(Character owner)
    {
        while(notRemoved)
        {
            SetDamage(GetDamage() + owner.HealAmount);
            if (owner.Hp == owner.MaxHp)
                SetDamage(GetDamage()*2);
            yield return null;
        }
    }
    private IEnumerator ResetDmg(Character owner)
    {
        while(notRemoved)
        {
            if (TurnManager.Instance.TurnEnd)
                SetDamage(0);
            yield return null;
        }
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
        if (interrupted)
        {
             interrupted = false;
             yield break;
        }
        
        Character enemy = GameManager.Instance.Map[target.X, target.Y].CharacterOnTile;
        GameManager.Instance.StartCoroutine(caster.HitAttack(enemy, GetDamage()));
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
        return 1226010;
    }

}
