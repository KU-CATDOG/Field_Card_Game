using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinSkysword : IPlayerCard, IAttackCard
{
    public bool Disposable { get; set; }
    private int range = 3;
    private int cost = 2;
    private int damage = 40;
    private bool interrupted;
    public string ExplainText
    {
        get
        {
            return $"기본 4방향만 사용 가능.{GetDamage()}의 피해를 줍니다";
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
    public List<int> Damage { get; }
    public void SetDmg(int value)
    {
        if ((damage + value < 0))
        {
            damage = 0;
            return;
        }
        damage += value;
    }
    public Color GetUnAvailableTileColor()
    {
        return Color.red;
    }
    public List<Coordinate> GetAvailableTile(Coordinate pos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        Coordinate tile = pos;
        for(int i=0;i<3;i++)
        {
            if (tile == null)
                break;
            if ((tile = tile.GetDownTile()) != null && GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile && i==2)
            {
                ret.Add(tile);
            }
        }
        tile = pos;
        for (int i = 0; i < 3; i++)
        {
            if (tile == null)
                break;
            if ((tile = tile.GetUpTile()) != null && GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile && i == 2)
            {
                ret.Add(tile);
            }
        }
        tile = pos;
        for (int i = 0; i < 3; i++)
        {
            if (tile == null)
                break;
            if ((tile = tile.GetRightTile()) != null && GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile && i == 2)
            {
                ret.Add(tile);
            }
        }
        tile = pos;
        for (int i = 0; i < 3; i++)
        {
            if (tile == null)
                break;
            if ((tile = tile.GetLeftTile()) != null && GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile && i == 2)
            {
                ret.Add(tile);
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
            yield return GameManager.Instance.StartCoroutine(caster.HitAttack(tmp, GetDamage()));
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
        return CostType.PaladinEnergy;
    }
    public CardType GetCardType()
    {
        return CardType.Attack;
    }
    public int GetCardID()
    {
        return 1133010;
    }
}
