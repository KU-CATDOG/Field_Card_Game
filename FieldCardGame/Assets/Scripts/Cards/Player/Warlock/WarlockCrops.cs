using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class WarlockCrops : IPlayerCard,IAttackCard,IHealCard
{
    public bool Disposable { get; set; }
    private int range = 0;
    private int cost = 20;
    private int damage = 0;
    private int healAmount = 0;
    private bool interrupted;
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
            tmp.Add(healAmount);
            return tmp;
        }
    }
    public void SetHealAmount(int value)
    {
        healAmount = healAmount + value < 0 ? 0 : healAmount + value;
        HealAmounts[0] = healAmount;
    }
    public string ExplainText
    {
        get
        {
            return $"사거리 2 이내 존재하는 모든 적의 ‘흡혈’을 제거하고, 해당 수치 5배의 피해를 줍니다. 준 피해만큼 체력을 회복합니다.";
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
        Coordinate pos = new Coordinate(0, 0);
        int level = 1;
        bool[,] visited = new bool[256, 256];
        Queue<Coordinate> queue = new Queue<Coordinate>();
        Queue<Coordinate> nextQueue = new Queue<Coordinate>();
        queue.Enqueue(pos);
        while (level++ <= 2)
        {
            while (queue.Count != 0)
            {
                Coordinate tmp = queue.Dequeue();
                if ((tmp.X != pos.X || tmp.Y != pos.Y))
                    ret.Add(tmp);
                Coordinate tile;
                if ((tile = tmp.GetDownTilewithoutTest()) != null && !visited[tile.X+128, tile.Y+128])
                {
                    visited[tile.X + 128, tile.Y + 128] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetLeftTilewithoutTest()) != null && !visited[tile.X+128, tile.Y+128])
                {
                    visited[tile.X + 128, tile.Y + 128] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetRightTilewithoutTest()) != null && !visited[tile.X+128, tile.Y+128])
                {
                    visited[tile.X + 128, tile.Y + 128] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetUpTilewithoutTest()) != null && !visited[tile.X+128, tile.Y+128])
                {
                    visited[tile.X + 128, tile.Y + 128] = true;
                    nextQueue.Enqueue(tile);
                }
            }
            queue = new Queue<Coordinate>(nextQueue);
            nextQueue.Clear();
        }
        while (queue.Count != 0)
        {
            Coordinate tmp = queue.Dequeue();
            if ((tmp.X != pos.X || tmp.Y != pos.Y))
                ret.Add(tmp);
        }
        return ret;
    }
    public Color GetColorOfEffect(Coordinate pos)
    {
        if (pos.X == 0 && pos.Y == 0)
        {
            return Color.black;
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
        List<Coordinate> enemyList = GetAreaofEffect(target);
        foreach (var j in enemyList)
        {
            Character tmp = GameManager.Instance.Map[j.X+target.X, j.Y+target.Y].CharacterOnTile;
            if (tmp)
            {
                foreach (var i in tmp.EffectHandler.DebuffDict)
                {
                    if (i.Key == DebuffType.Vampire && i.Value.IsEnabled)
                    {
                        int buffCount = i.Value.Value;
                        i.Value.ForceRemoveEffect();
                        yield return GameManager.Instance.StartCoroutine(caster.HitAttack(tmp, damage+ buffCount * 5));
                        yield return GameManager.Instance.StartCoroutine(caster.GiveHeal(caster, healAmount + buffCount * 5, true));
                    }
                }
            }
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
        return CardType.Skill;
    }
    public int GetCardID()
    {
        return 3239001;
    }

}
