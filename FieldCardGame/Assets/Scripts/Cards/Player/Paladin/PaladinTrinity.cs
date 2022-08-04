using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinTrinity : IPlayerCard
{
    public bool Disposable { get; set; }
    private int range = 3;
    private int cost = 3;
    private int damage = 33;
    public int amount = 3;
    private bool interrupted;
    public string ExplainText
    {
        get
        {
            return $"적에게 사용하면 {GetDamage()}의 피해를 줍니다. 나에게 사용하면 신성력을 {GetAmount()} 얻고, 카드를 {GetAmount()}장 뽑습니다.";
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
    public int GetCost()
    {
        return cost;
    }
    public void SetCost(int _cost)
    {
        cost = _cost;
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
        if(GameManager.Instance.Map[pos.X, pos.Y].CharacterOnTile is Paladin)
            ret.Add(pos);
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
                if ((tmp.X != pos.X || tmp.Y != pos.Y) && GameManager.Instance.Map[tmp.X, tmp.Y].CharacterOnTile is Enemy)
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

        if (GameManager.Instance.Map[target.X, target.Y].CharacterOnTile is Enemy)
        {
            Character enemy = GameManager.Instance.Map[target.X, target.Y].CharacterOnTile;
            GameManager.Instance.StartCoroutine(caster.HitAttack(enemy, GetDamage()));
        }
        else if (GameManager.Instance.Map[target.X, target.Y].CharacterOnTile is Paladin)
        {
            Paladin owner = (Paladin) GameManager.Instance.Map[target.X, target.Y].CharacterOnTile;
            owner.CrystalCount += GetAmount();
            yield return owner.StartCoroutine(owner.DrawCard());
            yield return owner.StartCoroutine(owner.DrawCard());
            yield return owner.StartCoroutine(owner.DrawCard());
        }
        yield break;
    }
    public void CardRoutineInterrupt()
    {
        interrupted = true;
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
        return 1131001;
    }
}
