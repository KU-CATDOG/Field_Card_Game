using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinLightSmite : IPlayerCard
{
    private int range = 1;
    private bool interrupted;
    private int cost = 2;
    private int damage = 10;
    public bool Disposable { get; set; }
    public string ExplainText
    {
        get
        {
            return $"적에게 {GetDamage()}의 피해를 주고, ‘기절’시킵니다.";
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
    public IEnumerator CardRoutine(Character caster, Coordinate target)
    {
        if (interrupted)
        {
             interrupted = false;
             yield break;
        }
        
        Character enemy = GameManager.Instance.Map[target.X, target.Y].CharacterOnTile;
        GameManager.Instance.StartCoroutine(caster.HitAttack(enemy, GetDamage()));

        enemy.EffectHandler.DebuffDict[DebuffType.Stun].SetEffect(1);
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
        return 1105011;
    }
}
