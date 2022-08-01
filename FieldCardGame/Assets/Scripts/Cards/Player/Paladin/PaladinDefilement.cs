using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinDefilement : IPlayerCard
{
    private int range = 0;
    private int cost = 0;
    private bool notRemoved = true;
    private bool interrupted;
    public bool Disposable { get; set; } = true;
    public string ExplainText
    {
        get
        {
            return $"사용 불가";
        }
    }
    public IEnumerator GetCardRoutine(Character owner)
    {
        owner.AddTurnEndDebuff(TurnEndRemove(owner), 0);
        yield break;
    }
    private IEnumerator TurnEndRemove(Character owner)
    {
        bool isCardInHand = false;
        int idx = -1;

        while(notRemoved)
        {
            for (int i = 0; i < owner.HandCard.Count; i++) {
                if (owner.HandCard[i].GetCardID() == 1023000) {
                    idx = i;
                    isCardInHand = true;
                    break;
                }
            }

            if (isCardInHand) {
                notRemoved = false;
                owner.StartCoroutine(owner.RemoveCard(idx));
            }

            yield return null;
        }
    }
    public IEnumerator RemoveCardRoutine(Character owner)
    {
        notRemoved = false;
        yield break;
    }
    public int GetRange()
    {
        //return range;
        return range;
    }
    public void SetRange(int _range)
    {
        range = _range;
    }
    public Color GetUnAvailableTileColor()
    {
        return Color.red;
    }
    public List<Coordinate> GetAvailableTile(Coordinate pos)
    {
        List<Coordinate> ret = new List<Coordinate>();

        return ret;
    }
    public Color GetAvailableTileColor()
    {
        return Color.blue;
    }
    public List<Coordinate> GetAreaofEffect(Coordinate relativePos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        
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
        return CostType.Unpayable;
    }
    public CardType GetCardType()
    {
        return CardType.Skill;
    }
    public int GetCardID()
    {
        return 1023000;
    }

}
