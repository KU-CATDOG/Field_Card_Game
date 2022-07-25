using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockMosquito : IPlayerCard
{
    private int range = 1;
    private bool notRemoved = true;
    private int cost = 500;
    private int damage = 100;
    private bool interrupted;
    public bool Disposable { get; set; } = true;
    public string ExplainText
    {
        get
        {
            return $"플레이어가 데미지를 입을 때마다 비용이 10 감소한다. 사용하면 적에게 {GetDamage()}의 피해를 주고, 비용이 다시 500으로 초기화된다.";
        }
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
        }
        SetCost(500);
        yield break;
    }
    public IEnumerator GetCardRoutine(Character owner)
    {
        owner.AddGetDmgRoutine(ReduceCost(),0);
        yield break;
    }
    private IEnumerator ReduceCost()
    {
        while(notRemoved)
        {
            SetCost(GetCost() - 10);
            yield return null;
        }
    }
    public IEnumerator RemoveCardRoutine(Character owner)
    {
        notRemoved = true;
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
        return 3213010;
    }
}
