using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinDictamnus : IPlayerCard
{
    public bool Disposable { get; set; }
    private int range = 1;
    private int cost = 0;
    private int damage = 7;
    private bool interrupted;
    public string ExplainText
    {
        get
        {
            return $"적에게 {GetDamage()}의 피해를 주고 손패로 돌아옵니다. 이번 턴 동안 이 카드의 신성력 소모량이 1 증가합니다";
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
    public Color GetUnAvailableTileColor()
    {
        return Color.red;
    }
    public List<Coordinate> GetAvailableTile(Coordinate pos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        Coordinate tile;
        if ((tile = pos.GetDownTile()) != null && GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
        {
            ret.Add(tile);
        }
        if ((tile = pos.GetLeftTile()) != null && GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
        {
            ret.Add(tile);
        }
        if ((tile = pos.GetRightTile()) != null && GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
        {
            ret.Add(tile);
        }
        if ((tile = pos.GetUpTile()) != null && GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
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
            PaladinDictamnus paladinDictamnus = new PaladinDictamnus();
            paladinDictamnus.SetCost(GetCost() + 1);
            yield return GameManager.Instance.StartCoroutine(caster.AddCard(paladinDictamnus, true));
            caster.AddTurnEndDebuff(RetrieveCost(caster, paladinDictamnus), 0);
            Disposable = true;
        }
        yield break;
    }
    private IEnumerator RetrieveCost(Character caster, ICard toRetrieve)
    {
        toRetrieve.SetCost(0);
        yield return null;
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
        return CardType.Skill;
    }
    public int GetCardID()
    {
        return 1106001;
    }
}
