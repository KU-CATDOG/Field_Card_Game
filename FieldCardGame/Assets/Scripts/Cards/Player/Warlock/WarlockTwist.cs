using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class WarlockTwist : IPlayerCard
{
    public bool Disposable { get; set; }
    private int range = 0;
    private int cost = 20;
    private bool interrupted;
    public string ExplainText
    {
        get
        {
            return $"사거리 2 이내 적의 모든 버프를 랜덤한 디버프로 바꾸고, 내가 가진 모든 디버프를 랜덤한 버프로 바꿉니다.";
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
        bool[,] visited = new bool[128, 128];
        Queue<Coordinate> queue = new Queue<Coordinate>();
        Queue<Coordinate> nextQueue = new Queue<Coordinate>();
        queue.Enqueue(pos);
        while (level++ <= 2)
        {
            while (queue.Count != 0)
            {
                Coordinate tmp = queue.Dequeue();
                if ((tmp.X != pos.X || tmp.Y != pos.Y) && GameManager.Instance.Map[tmp.X,tmp.Y].CharacterOnTile)
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
            if ((tmp.X != pos.X || tmp.Y != pos.Y) && GameManager.Instance.Map[tmp.X, tmp.Y].CharacterOnTile)
                ret.Add(tmp);
        }
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
        List<Coordinate> enemyList = GetAreaofEffect(target);
        foreach (var j in enemyList)
        {
            Character tmp = GameManager.Instance.Map[j.X, j.Y].CharacterOnTile;
            if (tmp)
            {
                foreach (var i in tmp.EffectHandler.BuffDict)
                {
                    if (i.Value.IsEnabled)
                    {
                        int buffCount = i.Value.Value;
                        i.Value.ForceRemoveEffect();
                        tmp.EffectHandler.DebuffDict[(DebuffType)Random.Range(1, (int)(DebuffType.Length))].SetEffect(buffCount);
                    }
                }
            }
        }
        foreach (var i in caster.EffectHandler.DebuffDict)
        {
            if (i.Value.IsEnabled)
            {
                int buffCount = i.Value.Value;
                i.Value.ForceRemoveEffect();
                caster.EffectHandler.BuffDict[(BuffType)Random.Range(0, (int)(BuffType.Length))].SetEffect(buffCount);
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
        return 3119001;
    }

}
