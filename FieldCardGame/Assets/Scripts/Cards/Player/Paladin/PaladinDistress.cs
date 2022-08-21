using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinDistress : IPlayerCard
{
    private int range = 0;
    private int cost = 0;
    private int healAmount = 5;
    private bool interrupted;
    public bool Disposable { get; set; }
    public string ExplainText
    {
        get
        {
            return $"본인을 포함한 거리 5 이내의 모두에게 ‘신성낙인’을 부여합니다. 이미 ‘신성낙인’이 부여된 대상의 ‘신성낙인’을 발동한 후 새로 부여합니다.";
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
        int level = 1;
        bool[,] visited = new bool[128, 128];
        Queue<Coordinate> queue = new Queue<Coordinate>();
        Queue<Coordinate> nextQueue = new Queue<Coordinate>();
        queue.Enqueue(new Coordinate(0,0));
        while (level++ <= 5)
        {
            while (queue.Count != 0)
            {
                Coordinate tmp = queue.Dequeue();
                if (tmp.X != 0 || tmp.Y != 0)
                    ret.Add(tmp);
                Coordinate tile;
                if ((tile = tmp.GetDownTilewithoutTest()) != null && !visited[64+tile.X, 64+tile.Y])
                {
                    visited[64+tile.X, 64+tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetLeftTilewithoutTest()) != null && !visited[64+tile.X, 64+tile.Y])
                {
                    visited[64 + tile.X, 64 + tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetRightTilewithoutTest()) != null && !visited[64 + tile.X, 64 + tile.Y])
                {
                    visited[64 + tile.X, 64 + tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetUpTilewithoutTest()) != null && !visited[64 + tile.X, 64 + tile.Y])
                {
                    visited[64+tile.X, 64+tile.Y] = true;
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
        var area = GetAreaofEffect(new Coordinate(0,0));
        foreach(var i in area)
        {
            if (Coordinate.OutRange(i + caster.position)) continue;
            if(GameManager.Instance.Map[i.X+caster.position.X, i.Y+caster.position.Y].CharacterOnTile is Enemy)
            {
                Effect stigma = GameManager.Instance.Map[i.X + caster.position.X, i.Y + caster.position.Y].CharacterOnTile.EffectHandler.DebuffDict[DebuffType.DivineStigma];
                while (stigma.IsEnabled)
                {
                    yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Map[i.X + caster.position.X, i.Y + caster.position.Y].CharacterOnTile.GetDmg(caster, 0));
                    yield return new WaitForSeconds(0.1f);
                }
                GameManager.Instance.Map[i.X + caster.position.X, i.Y + caster.position.Y].CharacterOnTile.EffectHandler.DebuffDict[DebuffType.DivineStigma].SetEffect(1);
            }
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
        return CardType.Attack;
    }
    public int GetCardID()
    {
        return 1142010;
;
    }
}
