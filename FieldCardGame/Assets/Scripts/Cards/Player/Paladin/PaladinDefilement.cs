using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinDefilement : IPlayerCard, NotReward, IAttackCard
{
    //fixme
    public List<int> Damage { get; }
    public void SetDmg(int val)
    {
        return;
    }

    public static bool Scintillation { get; set; } = false;
    public static bool Enlighted { get; set; } = false;
    private int range = 0;
    private int cost = -1;
    private bool notRemoved = true;
    private bool interrupted;
    public bool Disposable { get; set; }
    public string ExplainText
    {
        get
        {
            return $"사용불가, 손패에 있다면 턴이 종료될 때 사라집니다.";
        }
    }
    public IEnumerator GetCardRoutine(Character owner)
    {
        owner.AddTurnEndDebuff(TurnEndRemove(owner), 0);
        yield break;
    }
    private IEnumerator TurnEndRemove(Character owner)
    {
        bool isCardInHand = false;
        int idx = -1;

        while(notRemoved)
        {
            for (int i = 0; i < owner.HandCard.Count; i++) {
                if (owner.HandCard[i].GetCardID() == 1023000) {
                    idx = i;
                    isCardInHand = true;
                    break;
                }
            }

            if (isCardInHand) {
                notRemoved = false;
                yield return owner.StartCoroutine(owner.RemoveCard(idx));
            }

            yield return null;
        }
    }
    public IEnumerator RemoveCardRoutine(Character owner)
    {
        yield break;
    }
    public int GetRange()
    {
        if (Scintillation)
        {
            SetRange(3);
        }
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
        if (Scintillation)
        {
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
                    if (tmp.X != pos.X || tmp.Y != pos.Y)
                        ret.Add(tmp);
                    Coordinate tile;
                    if ((tile = tmp.GetDownTile()) != null && !visited[tile.X, tile.Y] )
                    {
                        visited[tile.X, tile.Y] = true;
                        nextQueue.Enqueue(tile);
                    };
                    if ((tile = tmp.GetLeftTile()) != null && !visited[tile.X, tile.Y] )
                    {
                        visited[tile.X, tile.Y] = true;
                        nextQueue.Enqueue(tile);
                    };
                    if ((tile = tmp.GetRightTile()) != null && !visited[tile.X, tile.Y] )
                    {
                        visited[tile.X, tile.Y] = true;
                        nextQueue.Enqueue(tile);
                    };
                    if ((tile = tmp.GetUpTile()) != null && !visited[tile.X, tile.Y] )
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
                ret.Add(queue.Dequeue());
            }
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
        if (Scintillation)
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
        if(Scintillation)
            yield return GameManager.Instance.StartCoroutine(caster.HitAttack(GameManager.Instance.Map[target.X, target.Y].CharacterOnTile, 3));
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
        if(!Enlighted)
            return CostType.Unpayable;
        return CostType.PaladinEnergy;
    }
    public CardType GetCardType()
    {
        return CardType.Skill;
    }
    public int GetCardID()
    {
        return 1023000;
    }

}
