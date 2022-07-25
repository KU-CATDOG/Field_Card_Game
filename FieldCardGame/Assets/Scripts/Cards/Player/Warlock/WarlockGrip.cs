using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockGrip : IPlayerCard
{
    private int range = 2;
    private int cost = 20;
    private int damage = 8;
    private int healAmount = 2;
    private bool interrupted;
    public bool Disposable { get; set; }
    public IEnumerator GetCardRoutine(Character owner)
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
        Coordinate tile;
        if ((tile = (pos.GetDownTile()).GetDownTile()) != null)
            ret.Add(tile);
        if ((tile = (pos.GetUpTile()).GetUpTile()) != null)
            ret.Add(tile);
        if ((tile = (pos.GetLeftTile()).GetLeftTile()) != null)
            ret.Add(tile);
        if ((tile = (pos.GetRightTile()).GetRightTile()) != null)
            ret.Add(tile);
        return ret;
    }
    public Color GetAvailableTileColor()
    {
        return Color.blue;
    }
    public List<Coordinate> GetAreaofEffect(Coordinate relativePos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        Coordinate pos = new Coordinate(0, 0);
        Coordinate tile;
        ret.Add(pos);
        tile = pos.GetDownTilewithoutTest();
        ret.Add(tile);
        tile = tile.GetRightTilewithoutTest();
        ret.Add(tile);
        tile = pos.GetUpTilewithoutTest();
        ret.Add(tile);
        tile = tile.GetLeftTilewithoutTest();
        ret.Add(tile);
        tile = pos.GetLeftTilewithoutTest();
        ret.Add(tile);
        tile = tile.GetDownTilewithoutTest();
        ret.Add(tile);
        tile = pos.GetRightTilewithoutTest();
        ret.Add(tile);
        tile = tile.GetUpTilewithoutTest();
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
        List<Coordinate> available = new List<Coordinate>();
        Character tmp;
        Coordinate pos;
        attack = GetAreaofEffect(target - caster.position);
        for (int i = 0; i<attack.Count;i++)
        {
            pos = attack[i] + target;
            if (Coordinate.OutRange(pos))
            {
                continue;
            }
            if (interrupted)
            {
                interrupted = false;
                yield break;
            }
            tmp = GameManager.Instance.Map[pos.X, pos.Y].CharacterOnTile;
            if (tmp)
            {
                available.Add(pos);
            }
        }
        if (available.Count == 0)
            yield break;
        for (int i = 0; i < available.Count - 1; i++)
        {
            pos = available[i];
            tmp = GameManager.Instance.Map[pos.X, pos.Y].CharacterOnTile;
            GameManager.Instance.StartCoroutine(caster.HitAttack(tmp, GetDamage()));
        }
        pos = available[available.Count - 1];
        tmp = GameManager.Instance.Map[pos.X, pos.Y].CharacterOnTile;
        yield return GameManager.Instance.StartCoroutine(caster.HitAttack(tmp, GetDamage()));
        yield return GameManager.Instance.StartCoroutine(caster.GiveHeal(caster, GetHealAmount() * available.Count,true));
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
        return CardType.Attack;
    }
    public int GetCardID()
    {
        return 3108010;
    }
}
