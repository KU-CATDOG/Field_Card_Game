using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinBrain : IPlayerConditionCard
{
    public bool Disposable { get; set; }
    private int range = 1;
    private int cost = 1;
    public int damage = 15;
    private bool interrupted;
    public string ExplainText
    {
        get
        {
            return $"적에게 ‘기절’해 있다면, {GetDamage()}의 피해를 줍니다. 아니라면, 적을 ‘기절’ 시킵니다.";
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
    public int GetDamage()
    {
        return damage;
    }
    public void SetDamage(int _damage)
    {
        damage = _damage;
    }
    public Color GetUnAvailableTileColor()
    {
        return Color.red;
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
    public bool isSatisfied(Coordinate target)
    {
        if (GameManager.Instance.Map[target.X, target.Y].CharacterOnTile != null && GameManager.Instance.Map[target.X, target.Y].CharacterOnTile.EffectHandler.DebuffDict[DebuffType.Stun].IsEnabled)
            return true;

        else
            return false;
    }
    public Color SatisfiedAreaColor()
    {
        return Color.yellow;
    }
    public IEnumerator CardRoutine(Character caster, Coordinate target)
    {
        if (interrupted)
        {
             interrupted = false;
             yield break;
        }
        Character enemy = GameManager.Instance.Map[target.X, target.Y].CharacterOnTile;

        if (isSatisfied(target))
        {
            yield return GameManager.Instance.StartCoroutine(caster.HitAttack(enemy, GetDamage()));
        }

        else
        {
            enemy.EffectHandler.DebuffDict[DebuffType.Stun].SetEffect(1);
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
        return 1137011;
    }
}
