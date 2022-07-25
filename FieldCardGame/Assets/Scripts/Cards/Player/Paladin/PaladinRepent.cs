using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinRepent : IPlayerConditionCard
{
    public bool Disposable { get; set; }
    private int range = 1;
    private int cost = 1;
    public int damage = 5;
    private bool interrupted;
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

        ret.Add(pos.GetUpTilewithoutTest());
        ret.Add(pos.GetDownTilewithoutTest());
        ret.Add(pos.GetLeftTilewithoutTest());
        ret.Add(pos.GetRightTilewithoutTest());

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
        if (GameManager.Instance.Map[target.X, target.Y].CharacterOnTile != null && GameManager.Instance.Map[target.X, target.Y].CharacterOnTile.Hp <= 15)
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
        if (isSatisfied(target))
        {
            damage = 15;
        }
        Character enemy = GameManager.Instance.Map[target.X, target.Y].CharacterOnTile;
        if(enemy)
            yield return GameManager.Instance.StartCoroutine(caster.HitAttack(enemy, GetDamage()));
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
        return 1022010;
    }
}
