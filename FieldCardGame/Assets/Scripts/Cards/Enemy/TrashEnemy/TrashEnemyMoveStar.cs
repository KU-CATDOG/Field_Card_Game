using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashEnemyMoveStar : ICard
{
    public bool Disposable { get; set; }

    private int cost;
    private int dmg;
    public int _dmg
    {
        get { return dmg; }
        set { dmg = value; }
    }
    
    private int range;

    private bool interrupted;

    public int GetCardID()
    {
        return 0002223;
    }
    
    public CostType GetCostType()
    {
        return CostType.MonsterCrystal;
    }
    public CardType GetCardType()
    {
        return CardType.Attack;
    }
    public List<Coordinate> GetAvailableTile(Coordinate pos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        List<Coordinate> canTile = pos.GetDistanceAvailableTile(range);
        
        foreach (var t in canTile)
        {
            if(GameManager.Instance.Map[t.X, t.Y].CharacterOnTile is Player)
            {
                ret.Add(t);
            }
        }
        //Debug.Log(ret.Count);

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
    public List<Coordinate> GetAreaofEffect(Coordinate relativePos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        ret.Add(new Coordinate(0, 0));
        return ret;
    }
    public IEnumerator CardRoutine(Character caster, Coordinate center)
    {
        if (interrupted)
            yield break;
        GameManager.Instance.StartCoroutine(caster.HitAttack(GameManager.Instance.Map[center.X, center.Y].CharacterOnTile, dmg));
    }
    public void CardRoutineInterrupt()
    {
        interrupted = true;
    }



    // not use
    public int GetRange()
    {
        return 1;
    }
    public void SetRange(int _range)
    {
        range = _range;
    }
    public int GetCost()
    {
        return cost;
    }
    public void SetCost(int _cost)
    {
        cost = _cost;
    }

}
