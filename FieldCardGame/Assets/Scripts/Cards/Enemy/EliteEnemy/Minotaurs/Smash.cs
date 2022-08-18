using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash : ICard
{
    private int range = 1;
    private int cost = 4;
    private int damage = 15;
    private DebuffType debuffType = DebuffType.Stun;
    private int debuffValue = 1;
    private bool interrupted;
    public bool Disposable = true;

    bool ICard.Disposable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
    public List<Coordinate> GetAvailableTile(Coordinate pos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        ret.Add(pos);
        return ret;
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
            yield return GameManager.Instance.StartCoroutine(caster.HitAttack(tmp, damage));
            tmp.EffectHandler.DebuffDict[debuffType].SetEffect(debuffValue);
            yield return GameManager.Instance.StartCoroutine(caster.AddCard(new EnemyMoveNAttack(), false));
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
        return CostType.MonsterCrystal;
    }
    public CardType GetCardType()
    {
        return CardType.Skill;
    }
    public int GetCardID()
    {
        return 0096010;
    }

    public List<Coordinate> GetAreaofEffect(Coordinate relativePos)
    {
        throw new System.NotImplementedException();
    }
}
