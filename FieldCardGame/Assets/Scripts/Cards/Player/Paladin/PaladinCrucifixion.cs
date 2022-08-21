using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinCrucifixion : IPlayerCard
{
    private int range = 0;
    private bool interrupted;
    private int amount = 0;
    private int cost = 1;
    public bool Disposable { get; set; }
    public string ExplainText
    {
        get
        {
            return $"적에게 2의 피해를 두 번 입힙니다. 신성낙인을 부여합니다.";
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
        return 1038011;
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
        yield return GameManager.Instance.StartCoroutine(caster.HitAttack(GameManager.Instance.Map[center.X, center.Y].CharacterOnTile, 2));
        yield return GameManager.Instance.StartCoroutine(caster.HitAttack(GameManager.Instance.Map[center.X, center.Y].CharacterOnTile, 2));
        GameManager.Instance.Map[center.X, center.Y].CharacterOnTile.EffectHandler.DebuffDict[DebuffType.DivineStigma].SetEffect(1);
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
