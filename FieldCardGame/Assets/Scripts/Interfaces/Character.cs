using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public bool NeedWait { get; set; }
    public bool ReadyToPlay { get; set; }
    private coordinate pos;
    public coordinate position 
    { 
        get 
        { 
            return pos; 
        } 
        set 
        { 
            pos = value;
            transform.position = new Vector3(value.X, 0, value.Y);
        } 
    }
    private int sight;
    public List<ICard> HandCard { get; private set; }
    public List<ICard> CardPile { get; private set; }
    public List<ICard> DiscardedPile { get; private set; }

    /// for UI
    public List<Buff> BuffList { get; set; }
    public List<Debuff> DebuffList { get; set; }
    /// 

    public List<IEnumerator> BuffHandler { get; private set; }
    public List<IEnumerator> DebuffHandler { get; private set; }
    public List<IEnumerator> DrawBuffHandler { get; private set; }
    public List<IEnumerator> DrawDebuffHandler { get; private set; }
    public List<IEnumerator> TurnEndBuffHandler { get; private set; }
    public List<IEnumerator> TurnEndDebuffHandler { get; private set; }
    public List<IEnumerator> ForceTurnEndDebuffHandler { get; private set; }


    public bool MoveInterrupted { get; set; }
    public List<IEnumerator> TryMoveRoutine { get; private set; }
    public List<IEnumerator> MoveRoutine { get; private set; }


    public bool ForceMoveInterrupted { get; set; }
    public List<IEnumerator> TryForceMoveRoutine { get; private set; }
    public List<IEnumerator> ForceMoveRoutine { get; private set; }

    public bool GetDmgInterrupted { get; set; }
    public List<IEnumerator> TryGetDmgRoutine { get; private set; }
    public List<IEnumerator> GetDmgRoutine { get; private set; }

    public bool HitInterrupted { get; set; }
    public List<IEnumerator> TryHitAttackRoutine { get; private set; }
    public List<IEnumerator> HitAttackRoutine { get; private set; }
    public bool DieInterrupted { get; set; }
    public List<IEnumerator> TryDieRoutine { get; private set; }
    public List<IEnumerator> DieRoutine { get; private set; }

    public ICard drawCard { get; set; }
    public bool DrawInterrupted { get; set; }
    public List<IEnumerator> DrawCardTry { get; private set; }
    public List<IEnumerator> DrawCardRoutine { get; private set; }


    public ICard dropCard { get; set; }
    public bool DropInterrupted { get; set; }
    public List<IEnumerator> DropCardTry { get; private set; }
    public List<IEnumerator> DropCardRoutine { get; private set; }

    public ICard usedCard { get; set; }
    public bool CardUseInterrupted { get; set; }
    public List<IEnumerator> CardUseTry { get; private set; }
    public List<IEnumerator> CardUseRoutine { get; private set; }

    public abstract IEnumerator AwakeTurn();
    public abstract IEnumerator AfterBuff();
    public abstract IEnumerator AfterDraw();
    public abstract IEnumerator StartTurn();
    public IEnumerator ShuffleDeck()
    {
        //need animation for player
        if(CardPile.Capacity != 0)
        {
            for(int i = CardPile.Capacity-1; i>=0; i--)
            {
                DiscardedPile.Add(CardPile[i]);
                CardPile.RemoveAt(i);
            }
        }
        while(DiscardedPile.Capacity != 0)
        {
            int rand = Random.Range(0, DiscardedPile.Capacity);
            CardPile.Add(DiscardedPile[rand]);
            DiscardedPile.RemoveAt(rand);
        }
        yield break;
    }
    public IEnumerator DrawCard()
    {
        //need animation for player
        for(int i = DrawCardTry.Capacity-1; i>=0; i--)
        {
            IEnumerator routine = DrawCardTry[i];
            
            if(!routine.MoveNext())
            {
                while (NeedWait) yield return null;
                DrawCardTry.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
        if (DrawInterrupted)
        {
            DrawInterrupted = false;
            yield break;
        }
        drawCard = CardPile[0];
        CardPile.RemoveAt(0);
        HandCard.Add(drawCard);
        for (int i = DrawCardRoutine.Capacity-1; i >= 0; i--)
        {
            IEnumerator routine = DrawCardRoutine[i];
            if (!routine.MoveNext())
            {
                while (NeedWait) yield return null;
                DrawCardRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
        yield break;
    }
    public IEnumerator DropCard()
    {
        //need animation for player
        for (int i = DropCardTry.Capacity - 1; i >= 0; i--)
        {
            IEnumerator routine = DropCardTry[i];
            if (!routine.MoveNext())
            {
                while (NeedWait) yield return null;
                DropCardTry.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
        if (DropInterrupted)
        {
            DrawInterrupted = false;
            yield break;
        }
        HandCard.Remove(usedCard);
        DiscardedPile.Add(usedCard);
        for (int i = DropCardRoutine.Capacity - 1; i >= 0; i--)
        {
            IEnumerator routine = DropCardRoutine[i];
            if (!routine.MoveNext())
            {
                while (NeedWait) yield return null;
                DropCardRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
        yield break;
    }
    public IEnumerator CardUse()
    {
        for (int i = CardUseTry.Capacity-1; i >= 0; i--)
        {
            IEnumerator routine = CardUseTry[i];

            if (!routine.MoveNext())
            {
                while (NeedWait) yield return null;
                CardUseTry.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
        if (CardUseInterrupted)
        {
            CardUseInterrupted = false;
            yield break;
        }
        yield return StartCoroutine(usedCard.CardRoutine(this));
        for (int i = DrawCardRoutine.Capacity-1; i >= 0; i--)
        {
            IEnumerator routine = DrawCardRoutine[i];
            if (!routine.MoveNext())
            {
                while (NeedWait) yield return null;
                DrawCardRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
        yield return StartCoroutine(DropCard());
    }

    
    public IEnumerator Move(coordinate target, int speed)
    {
        for (int i = TryMoveRoutine.Capacity-1; i >= 0; i--)
        {
            if (!TryMoveRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                TryMoveRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
        if (MoveInterrupted)
        {
            MoveInterrupted = false;
            yield break;
        }
        Tile prevTile = GameManager.Instance.Map[position.X, position.Y];
        Tile targetTile = GameManager.Instance.Map[target.X, target.Y];
        for (int i = prevTile.OnCharacterExitRoutine.Capacity-1; i >= 0; i--)
        {
            if (!prevTile.OnCharacterExitRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                prevTile.OnCharacterExitRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
        //fixme
        prevTile.CharacterOnTile = null;
        position = target;
        targetTile.CharacterOnTile = this;

        for (int i = targetTile.OnCharacterExitRoutine.Capacity-1; i >= 0; i--)
        {
            if (!targetTile.OnCharacterExitRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                targetTile.OnCharacterExitRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }

        for(int i = MoveRoutine.Capacity-1; i>=0; i--)
        {
            if (!MoveRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                MoveRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
    }

    public IEnumerator ForceMove(coordinate target, int speed)
    {
        for (int i = TryForceMoveRoutine.Capacity-1; i >= 0; i--)
        {
            if (!TryForceMoveRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                TryForceMoveRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
        if (ForceMoveInterrupted)
        {
            ForceMoveInterrupted = false;
            yield break;
        }

        Tile prevTile = GameManager.Instance.Map[position.X, position.Y];
        Tile targetTile = GameManager.Instance.Map[target.X, target.Y];
        for (int i = prevTile.OnCharacterExitRoutine.Capacity-1; i >= 0; i--)
        {
            if (!prevTile.OnCharacterExitRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                prevTile.OnCharacterExitRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
        //fixme
        prevTile.CharacterOnTile = null;
        position = target;
        targetTile.CharacterOnTile = this;

        for (int i = targetTile.OnCharacterExitRoutine.Capacity-1; i >= 0; i--)
        {
            if (!targetTile.OnCharacterExitRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                targetTile.OnCharacterExitRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }

        for (int i = ForceMoveRoutine.Capacity-1; i >= 0; i--)
        {
            if (!ForceMoveRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                ForceMoveRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
    }

    public IEnumerator GetDmg(int dmg)
    {
        for (int i = TryGetDmgRoutine.Capacity-1; i >= 0; i--)
        {
            if (!TryGetDmgRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                TryGetDmgRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
        if (GetDmgInterrupted)
        {
            GetDmgInterrupted = false;
            yield break;
        }
        ////fixme

        for (int i = GetDmgRoutine.Capacity-1; i >= 0; i--)
        {
            if (!GetDmgRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                GetDmgRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
    }
    public IEnumerator Die()
    {
        for (int i = TryDieRoutine.Capacity-1; i >= 0; i--)
        {
            if (!TryDieRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                TryDieRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
        if (DieInterrupted)
        {
            DieInterrupted = false;
            yield break;
        }
        yield return StartCoroutine(dieRoutine());

        for (int i = DieRoutine.Capacity-1; i >= 0; i--)
        {
            if (!DieRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                DieRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
    }
    protected abstract IEnumerator dieRoutine();
}
