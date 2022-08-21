using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockSnatch : IPlayerCard,IAttackCard,IHealCard
{
    private int range = 1;
    private int cost = 15;
    private int damage = 15;
    private int healAmount = 20;
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
    public List<int> HealAmounts
    {
        get
        {
            List<int> tmp = new();
            tmp.Add(healAmount);
            return tmp;
        }
    }
    public void SetHealAmount(int value)
    {
        healAmount = healAmount + value < 0 ? 0 : healAmount + value;
        HealAmounts[0] = healAmount;
    }
    private bool interrupted;
    public string ExplainText
    {
        get
        {
            return $"������ {damage}�� ���ظ� �ݴϴ�. ���� {healAmount}�� ������� ȸ���մϴ�.";
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
    public int GetHealAmount()
    {
        return healAmount;
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
        Character tmp = GameManager.Instance.Map[target.X, target.Y].CharacterOnTile;
        if (tmp)
        {
            if (interrupted)
            {
                interrupted = false;
                yield break;
            }
            yield return GameManager.Instance.StartCoroutine(caster.HitAttack(tmp, GetDamage()));
            yield return GameManager.Instance.StartCoroutine(caster.GiveHeal(caster, GetHealAmount(),true));
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
        return CostType.Hp;
    }
    public CardType GetCardType()
    {
        return CardType.Attack;
    }
    public int GetCardID()
    {
        return 3002010;
    }
}
