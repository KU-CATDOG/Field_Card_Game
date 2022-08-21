using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockSoul : IPlayerCard,IAttackCard
{
    private int range = 1;
    private int cost = 0;
    private int damage = 5;
    public List<int> Damage
    {
        get
        {
            List<int> tmp = new();
            tmp.Add(damage);
            return tmp;
        }
    }
    public void SetDmg(int value)
    {
        damage = damage + value < 0 ? 0 : damage + value;
        Damage[0] = damage;
    }
    private bool notRemoved = true;
    private bool interrupted;
    public bool Disposable { get; set; } = true;
    public string ExplainText
    {
        get
        {
            return $"{damage}의 피해를 줍니다. 턴 종료 시, {damage}의 피해를 받습니다. \n소멸.";
        }
    }
    public IEnumerator RemoveCardRoutine(Character owner)
    {
        notRemoved = false;
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
    public void SetDamge(int _damage)
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
        ret.Add(pos);
        Coordinate tile;
        if ((tile = pos.GetDownTile()) != null)
        {
            ret.Add(tile);
        };
        if ((tile = pos.GetLeftTile()) != null)
        {
            ret.Add(tile);
        };
        if ((tile = pos.GetRightTile()) != null)
        {
            ret.Add(tile);
        };
        if ((tile = pos.GetUpTile()) != null)
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
        (caster as Warlock).soulCount++;
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
    public IEnumerator GetCardRoutine(Character owner)
    {
        owner.AddTurnEndDebuff(Selfharm(owner),0);
        yield break;
    }
    private IEnumerator Selfharm(Character owner)
    {
        while (notRemoved)
        {
            if (owner.HandCard.Contains(this))
                GameManager.Instance.StartCoroutine(owner.HitAttack(owner,5));
            yield return null;
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
        return CostType.Hp;
    }
    public CardType GetCardType()
    {
        return CardType.Skill;
    }
    public int GetCardID()
    {
        return 3027001;
    }
}
