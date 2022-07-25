using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockLegend : IPlayerCard
{
    private int range = 1;
    private int cost = 0;
    private int damage = 30;
    private int healAmount = 5;
    private bool interrupted;
    public bool Disposable { get; set; }
    public string ExplainText
    {
        get
        {
            return $"선택한 방향의 뒤로 3칸까지 적에게 {damage}의 피해를 줍니다. 맞은 적의 수만큼 피를 {healAmount}회복합니다. 이 카드를 사용할 때마다 해당 카드의 코스트가 2 증가합니다.";
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
        while (level < 4)
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
            GameManager.Instance.StartCoroutine(caster.HitAttack(tmp, GetDamage()));
        }
        pos = available[enemyCount - 1];
        tmp = GameManager.Instance.Map[pos.X, pos.Y].CharacterOnTile;
        yield return GameManager.Instance.StartCoroutine(caster.HitAttack(tmp, GetDamage()));
        yield return GameManager.Instance.StartCoroutine(caster.GiveHeal(caster, GetHealAmount()*enemyCount,true));
        SetCost(GetCost()+2);
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
        return 3219010;
    }
}
