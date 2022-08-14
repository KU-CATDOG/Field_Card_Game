using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinFighting : IPlayerCard
{
    private int range = 0;
    private int cost = 1;
    private bool interrupted;
    public bool Disposable { get; set; }
    public string ExplainText
    {
        get
        {
            return $"카드를 3장 뽑은 후, 손패 전체에서 공격 태그가 없는 카드들을 모두 버린다. 다음 사용하는 카드의 신성력 소모량이 1 감소한다.";
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
    public Color GetUnAvailableTileColor()
    {
        return Color.red;
    }
    public List<Coordinate> GetAvailableTile(Coordinate pos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        ret.Add(pos);
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
        yield return caster.StartCoroutine(caster.DrawCard());
        yield return caster.StartCoroutine(caster.DrawCard());
        yield return caster.StartCoroutine(caster.DrawCard());

        for (int i = caster.HandCard.Count - 1; i >= 0; i--){
            if (caster.HandCard[i].GetCardID() != 1130001 && caster.HandCard[i].GetCardID()%100 < 10)
                yield return caster.StartCoroutine(caster.DropCard(i));
        }

        List<ICard> InHand = new(caster.HandCard);
        foreach(var i in InHand)
        {
            var tmp = i.GetCost() - 1;
            i.SetCost(Mathf.Max(0, tmp));
        }
        caster.AddPayCostRoutine(RetrieveCost(caster, InHand), 0);
        caster.AddTurnEndDebuff(RetrieveCost(caster, InHand), 0);
        yield return null;
    }
    private IEnumerator RetrieveCost(Character caster, List<ICard> toRetrieve)
    {
        List<ICard> InHand = caster.HandCard;
        List<ICard> InDiscarded = caster.DiscardedPile;
        List<ICard> InDummy = caster.CardPile;
        foreach(var i in InHand)
        {
            if(toRetrieve.Exists(j => j==i))
            {
                i.SetCost(GameManager.Instance.CardDict[i.GetCardID()].GetCost());
            }
        }
        foreach(var i in InDiscarded)
        {
            if(toRetrieve.Exists(j => j==i))
            {
                i.SetCost(GameManager.Instance.CardDict[i.GetCardID()].GetCost());
            }
        }
        foreach(var i in InDummy)
        {
            if(toRetrieve.Exists(j => j==i))
            {
                i.SetCost(GameManager.Instance.CardDict[i.GetCardID()].GetCost());
            }
        }
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
        return 1130001;
    }
}
