using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : IPlayerCard, IAttackCard
{
    //fixme
    public void SetDmg(int val)
    {
        return;
    }
    public List<int> Damage { get; }
    public bool Disposable { get; set; }
    private int cost = 1;
    private bool interrupted;
    int dmg = 10;
    public string ExplainText
    {
        get
        {
            return $"{dmg}의 피해를 줍니다.";
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
    public int GetCardID()
    {
        return 1000000;
    }
    public int GetRange()
    {
        return 1;
    }
    public void SetRange(int _range)
    {
        return;
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
    public List<Coordinate> GetAvailableTile(Coordinate pos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        Coordinate tile;
        if ((tile = pos.GetDownTile()) != null && GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile is Enemy)
        {
            ret.Add(tile);
        };
        if ((tile = pos.GetLeftTile()) != null && GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile is Enemy)
        {
            ret.Add(tile);
        };
        if ((tile = pos.GetRightTile()) != null && GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile is Enemy)
        {
            ret.Add(tile);
        };
        if ((tile = pos.GetUpTile()) != null && GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile is Enemy)
        {
            ret.Add(tile);
        }
        return ret;
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
    public List<Coordinate> GetAreaofEffect(Coordinate relativePos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        ret.Add(new Coordinate(0, 0));
        return ret;
    }
    public IEnumerator CardRoutine(Character caster, Coordinate center)
    {
        if (interrupted)
            yield break;
        GameManager.Instance.StartCoroutine(caster.HitAttack(GameManager.Instance.Map[center.X, center.Y].CharacterOnTile, dmg));
    }
    public void CardRoutineInterrupt()
    {
        interrupted = true;
    }
    public Color GetUnAvailableTileColor()
    {
        return Color.black;
    }
    public Color GetAvailableTileColor()
    {
        return Color.red;
    }
    public Color GetColorOfEffect(Coordinate pos)
    {
        return Color.white;
    }
}
