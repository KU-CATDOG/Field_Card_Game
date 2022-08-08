using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinCross : IPlayerCard
{
    private int range = 0;
    private bool interrupted;
    private int cost = 0;
    public int amount = 1;
    public bool Disposable { get; set; }
    public string ExplainText
    {
        get
        {
            return $"신성력을 {GetAmount()} 얻고, 카드를 {GetAmount()}장 뽑습니다.";
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
        if (interrupted)
        {
            interrupted = false;
            yield break;
        }

        if (GameManager.Instance.Map[target.X, target.Y].CharacterOnTile is Paladin)
        {
            Paladin owner = (Paladin) GameManager.Instance.Map[target.X, target.Y].CharacterOnTile;
            owner.CrystalCount += GetAmount();
            //yield return owner.StartCoroutine(owner.DrawCard());
            yield break;
        }
        yield return caster.StartCoroutine(caster.DrawCard());
        //yield break;
    }
    public void CardRoutineInterrupt()
    {
        interrupted = true;
    }
    public int GetCost()
    {
        return cost;
    }
    public void SetCost(int value)
    {
        cost = value;
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
        return 1125001;
    }
}

