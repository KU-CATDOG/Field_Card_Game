using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockFinalBlow : IPlayerCard,IAttackCard
{
    private int range = 1;
    private int cost = 0;
    private int damage = 0;
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
    private bool interrupted;
    public string ExplainText
    {
        get
        {
            return $"체력을 10로 만들고 적에게 감소한 생명력의 3배의 데미지를 줍니다.";
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
    public bool Disposable { get; set; } = true;
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
        int temp;
        Character tmp = GameManager.Instance.Map[target.X, target.Y].CharacterOnTile;
        if (tmp)
        {
            if (interrupted)
            {
                interrupted = false;
                yield break;
            }
            temp = caster.Hp - 10;
            yield return GameManager.Instance.StartCoroutine(caster.HitAttack(caster, temp));
            yield return GameManager.Instance.StartCoroutine(caster.HitAttack(tmp, temp + GetDamage()*3));
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
        return 3137010;
    }
}
