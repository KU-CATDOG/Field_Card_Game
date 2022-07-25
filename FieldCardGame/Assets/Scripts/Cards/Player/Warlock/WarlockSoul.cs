using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockSoul : IPlayerCard
{
    private int range = 1;
    private int cost = 0;
    private int healAmount = 7;
    private bool interrupted;
    public bool Disposable { get; set; } = true;
    public string ExplainText
    {
        get
        {
            return $"{healAmount}의 생명력을 회복합니다.";
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
    public int GetHealAmount()
    {
        return healAmount;
    }
    public void SetHealAmount(int _healAmount)
    {
        healAmount = _healAmount;
    }
    public Color GetUnAvailableTileColor()
    {
        return Color.red;
    }
    public List<Coordinate> GetAvailableTile(Coordinate pos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        ret.Add(pos);
        Coordinate tile;
        if ((tile = pos.GetDownTile()) != null)
        {
            ret.Add(tile);
        };
        if ((tile = pos.GetLeftTile()) != null)
        {
            ret.Add(tile);
        };
        if ((tile = pos.GetRightTile()) != null)
        {
            ret.Add(tile);
        };
        if ((tile = pos.GetUpTile()) != null)
        {
            ret.Add(tile);
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
        Character tmp = GameManager.Instance.Map[target.X, target.Y].CharacterOnTile;
        if (tmp)
        {
            if (interrupted)
            {
                interrupted = false;
                yield break;
            }
            if(tmp is Player)
                yield return GameManager.Instance.StartCoroutine(caster.GiveHeal(tmp, GetHealAmount(), true));
            else
                yield return GameManager.Instance.StartCoroutine(caster.GiveHeal(tmp, GetHealAmount(), false));
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
        return CostType.Hp;
    }
    public CardType GetCardType()
    {
        return CardType.Skill;
    }
    public int GetCardID()
    {
        return 3026001;
    }
}
