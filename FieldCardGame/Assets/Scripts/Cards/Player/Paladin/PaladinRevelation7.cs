using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinRevelation7 : IPlayerCard, IAttackCard, IHealCard, NotReward
{
    private int range = 3;
    private int cost = 1;
    public bool Disposable { get; set; } = true;
    private int healAmount = 7;
    private int damage = 7;
    private List<int> damages;
    public List<int> Damage
    {
        get
        {
            if (damages == null)
            {
                damages = new();
                damages.Add(damage);
            }
            return damages;
        }
    }
    public void SetDmg(int val)
    {
        damage = damage + val < 0 ? 0 : damage + val;
        Damage[0] = damage;
    }

    public List<int> HealAmounts
    {
        get
        {
            List<int> ret = new();
            ret.Add(healAmount);
            return ret;
        }
    }
    public void SetHealAmount(int val)
    {
        healAmount = healAmount + val < 0 ? 0 : healAmount + val;
    }

    private bool interrupted = false; public string ExplainText
    {
        get
        {
            return $"적에게 피해를 {damage}만큼 7번 줍니다. 내 체력을 {healAmount}만큼 7번 회복합니다. 묵시록_1.txt를 뽑을 카드 더미에 넣습니다.";
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
        Character targetEnemy = GameManager.Instance.Map[target.X, target.Y].CharacterOnTile;

        for (int i = 0; i < 7; i++)
        {
            if (interrupted)
            {
                interrupted = false;
                yield break;
            }
            yield return caster.StartCoroutine(caster.HitAttack(targetEnemy, damage));
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < 7; i++)
        {
            if (interrupted)
            {
                interrupted = false;
                yield break;
            }

            yield return caster.StartCoroutine(caster.GiveHeal(caster, healAmount));
            yield return new WaitForSeconds(0.05f);
        }

        yield return caster.StartCoroutine(caster.AddCard(new PaladinRevelation1()));
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
        return 1215011;
    }
}
