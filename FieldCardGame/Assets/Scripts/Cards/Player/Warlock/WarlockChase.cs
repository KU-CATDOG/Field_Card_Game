using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockChase : IPlayerCard
{
    private int range = 0;
    private int transferRange = 8;
    private int cost = 10;
    private bool interrupted;
    public bool Disposable { get; set; }
    public string ExplainText
    {
        get
        {
            return $"플레이어 기준 {transferRange} X {transferRange} 내에 있는 가장 가까운 적 1칸 범위 내로 이동합니다.";
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
        float speed = 5f;
        Coordinate tile = GetAvailableChasingTile(target);
        Debug.Log(tile.X);
        Debug.Log(tile.Y);
        if (interrupted)
        {
            interrupted = false;
            yield break;
        }
        yield return GameManager.Instance.StartCoroutine(caster.Move(tile, speed*50));
    }
    private Coordinate GetAvailableChasingTile(Coordinate playerPos)
    {
        Coordinate ret = playerPos;
        List<Coordinate> enemy = new List<Coordinate>();
        int level = 1;
        bool[,] visited = new bool[128, 128];
        Queue<Coordinate> queue = new Queue<Coordinate>();
        Queue<Coordinate> nextQueue = new Queue<Coordinate>();
        queue.Enqueue(playerPos);
        while (level++ <= transferRange)
        {
            while (queue.Count != 0)
            {
                Coordinate tmp = queue.Dequeue();
                Coordinate tile;
                if ((tile = tmp.GetDownTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                    if(GameManager.Instance.Map[tile.X,tile.Y].CharacterOnTile is Enemy)
                        enemy.Add(tile);
                };
                if ((tile = tmp.GetLeftTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                    if(GameManager.Instance.Map[tile.X,tile.Y].CharacterOnTile is Enemy)
                        enemy.Add(tile);
                };
                if ((tile = tmp.GetRightTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                    if(GameManager.Instance.Map[tile.X,tile.Y].CharacterOnTile is Enemy)
                        enemy.Add(tile);
                };
                if ((tile = tmp.GetUpTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                    if(GameManager.Instance.Map[tile.X,tile.Y].CharacterOnTile is Enemy)
                        enemy.Add(tile);
                }
            }
            queue = new Queue<Coordinate>(nextQueue);
            nextQueue.Clear();
        }
        while (queue.Count != 0)
        {
            Coordinate tile = queue.Dequeue();
            if(GameManager.Instance.Map[tile.X,tile.Y].CharacterOnTile is Enemy)
                enemy.Add(tile);
        }
        foreach(var i in enemy)
        {
            Coordinate tile;
            if ((tile = i.GetDownTile()) != null && !GameManager.Instance.Map[tile.X,tile.Y].CharacterOnTile)
            {
                ret = tile;
                break;
            };
            if ((tile = i.GetLeftTile()) != null && !GameManager.Instance.Map[tile.X,tile.Y].CharacterOnTile)
            {
                ret = tile;
                break;
            };
            if ((tile = i.GetRightTile()) != null && !GameManager.Instance.Map[tile.X,tile.Y].CharacterOnTile)
            {
                ret = tile;
                break;
            };
            if ((tile = i.GetUpTile()) != null && !GameManager.Instance.Map[tile.X,tile.Y].CharacterOnTile)
            {
                ret = tile;
                break;        
            }
        }
        return ret;
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
        return CardType.Move;
    }
    public int GetCardID()
    {
        return 3107100;
    }

}
