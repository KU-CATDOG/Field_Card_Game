using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockSoulBead : IPlayerCard
{
    private int range = 1;
    private int damage = 10;
    private bool interrupted;
    public bool Disposable { get; set; }
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
        if ((tile = pos.GetDownTile()) != null)
            ret.Add(tile);
        if ((tile = pos.GetUpTile()) != null)
            ret.Add(tile);
        if ((tile = pos.GetLeftTile()) != null)
            ret.Add(tile);
        if ((tile = pos.GetRightTile()) != null)
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
        int level = 1;
        Coordinate pos = new Coordinate(0, 0);
        Coordinate tile;
        ret.Add(pos);
        while (level < 3)
        {
            tile = pos + relativePos * level++;
            ret.Add(tile);
        }
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
        int enemyCount = available.Count;
        if (enemyCount == 0)
            yield break;
        for (int i = 0; i < enemyCount - 1; i++)
        {
            pos = available[i];
            tmp = GameManager.Instance.Map[pos.X, pos.Y].CharacterOnTile;
            GameManager.Instance.StartCoroutine(caster.HitAttack(tmp, GetDamage() + 5*i));
        }
        pos = available[enemyCount - 1];
        tmp = GameManager.Instance.Map[pos.X, pos.Y].CharacterOnTile;
        yield return GameManager.Instance.StartCoroutine(caster.HitAttack(tmp, GetDamage() + 5 * (enemyCount-1)));
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
        return 3106010;
    }
}
