using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockDrain : IPlayerCard
{
    private int range = 0;
    private int damage = 10;
    private int healAmount = 10;
    private bool interrupted;
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
        return ret;
    }
    public Color GetAvailableTileColor()
    {
        return Color.blue;
    }
    public List<Coordinate> GetAreaofEffect()
    {
        List<Coordinate> ret = new List<Coordinate>();
        Coordinate pos = new Coordinate(0, 0);
        Coordinate tile;
        tile = pos.GetDownTilewithoutTest();
        ret.Add(tile);
        tile = pos.GetLeftTilewithoutTest();
        ret.Add(tile);
        tile = pos.GetRightTilewithoutTest();
        ret.Add(tile);
        tile = pos.GetUpTilewithoutTest();
        ret.Add(tile);
        return ret;
    }
    public Color GetColorOfEffect(Coordinate pos)
    {
        if (pos.X == 0 && pos.Y == 0)
        {
            return Color.white;
        }
        return Color.red;
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
        List<Coordinate> attack;
        int healCount = 0;
        attack = GetAreaofEffect();
        foreach (Coordinate i in attack)
        {
            Coordinate pos = i + target;
            if (Coordinate.OutRange(pos))
            {
                continue;
            }
            healCount++;
            if (interrupted)
            {
                interrupted = false;
                yield break;
            }
            Character tmp = GameManager.Instance.Map[target.X, target.Y].CharacterOnTile;
            if (tmp)
            {
                GameManager.Instance.StartCoroutine(MultiAttack(caster, tmp));
            }
        }
        yield return GameManager.Instance.StartCoroutine(caster.GiveHeal(caster, GetHealAmount()*healCount));
    }
    private IEnumerator MultiAttack(Character caster, Character target)
    {
        caster.NeedWait++;
        yield return GameManager.Instance.StartCoroutine(caster.HitAttack(target, GetDamage()));
        caster.NeedWait--;
    }
    public void CardRoutineInterrupt()
    {
        interrupted = true;
    }
    public int GetCost()
    {
        return 20;
    }
    public CostType GetCostType()
    {
        return CostType.Hp;
    }
    public CardType GetCardType()
    {
        return CardType.Attack;
    }
    public int GetCardID()
    {
        return 3004100;
    }
}
