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
    public int Sight
    {
        get
        {
            return sight;
        }
        set
        {
            SightUpdate(value);
            sight = value;
        }
    }
    public List<ICard> HandCard { get; private set; } = new List<ICard>();
    public List<ICard> CardPile { get; private set; } = new List<ICard>();
    public List<ICard> DiscardedPile { get; private set; } = new List<ICard>();

    /// for UI
    public List<Buff> BuffList { get; set; } = new List<Buff>();
    public List<Debuff> DebuffList { get; set; } = new List<Debuff>();
    /// 

    public List<IEnumerator> BuffHandler { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> DebuffHandler { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> DrawBuffHandler { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> DrawDebuffHandler { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> TurnEndBuffHandler { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> TurnEndDebuffHandler { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> ForceTurnEndDebuffHandler { get; private set; } = new List<IEnumerator>();


    public bool MoveInterrupted { get; set; }
    public List<IEnumerator> TryMoveRoutine { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> MoveRoutine { get; private set; } = new List<IEnumerator>();


    public bool ForceMoveInterrupted { get; set; }
    public List<IEnumerator> TryForceMoveRoutine { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> ForceMoveRoutine { get; private set; } = new List<IEnumerator>();

    public bool GetDmgInterrupted { get; set; }
    public List<IEnumerator> TryGetDmgRoutine { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> GetDmgRoutine { get; private set; } = new List<IEnumerator>();

    public bool HitInterrupted { get; set; }
    public List<IEnumerator> TryHitAttackRoutine { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> HitAttackRoutine { get; private set; } = new List<IEnumerator>();
    public bool DieInterrupted { get; set; }
    public List<IEnumerator> TryDieRoutine { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> DieRoutine { get; private set; } = new List<IEnumerator>();

    public ICard drawCard { get; set; }
    public bool DrawInterrupted { get; set; }
    public List<IEnumerator> DrawCardTry { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> DrawCardRoutine { get; private set; } = new List<IEnumerator>();


    public ICard dropCard { get; set; }
    public bool DropInterrupted { get; set; }
    public List<IEnumerator> DropCardTry { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> DropCardRoutine { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> PayCostRoutine { get; private set; } = new List<IEnumerator>();

    public ICard usedCard { get; set; }
    public bool CardUseInterrupted { get; set; }
    public List<IEnumerator> CardUseTry { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> CardUseRoutine { get; private set; } = new List<IEnumerator>();

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
    public IEnumerator CardUse(coordinate center)
    {
        yield return StartCoroutine(PayCost(usedCard.GetCost(), usedCard.GetCostType()));
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
        yield return StartCoroutine(usedCard.CardRoutine(this, center));
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

    private void SightUpdate(int newSight, bool posChange = false, coordinate prevPos = null)
    {
        //implementation need
        //1. 현재 sight에 해당하는 범위의 모든 타일을 discover상태로 변경
        //2-1. posChange가 false라면 position에서 Sight에 해당하는 범위의 모든 Tile의 OnSight를 false로 한다.
        //2-2. posChange가 True라면  prevPos에서 Sight에 해당하는 범위의 모든 Tile의 OnSight를 false로 한다.
        //3.   position에서 new Sight에 해당하는 범위의 모든 타일 상태를 Discover을 True, OnSight를 True로 한다.
        //Tile은 GameManager.Instance.Map[X,Y]로 x,y좌표 타일에 접근 가능
    }

    /// <summary>
    /// 1칸 이동
    /// </summary>
    /// <param name="target"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
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
        prevTile.CharacterOnTile = null;
        Vector3 moveVector = new Vector3(target.X - position.X,0, target.Y - position.Y);
        float time = 0f;
        yield return new WaitUntil(() =>
        {
            time += Time.deltaTime;
            transform.position += moveVector * Time.deltaTime * speed;
            return time >= 1f / speed;
        });
        transform.position = new Vector3(target.X, 0, target.Y);
        coordinate prevPos = position;
        position = target;
        targetTile.CharacterOnTile = this;
        SightUpdate(sight, true, prevPos);

        for (int i = targetTile.OnCharacterEnterRoutine.Capacity-1; i >= 0; i--)
        {
            if (!targetTile.OnCharacterEnterRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                targetTile.OnCharacterEnterRoutine.RemoveAt(i);
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
        for (int i = prevTile.OnCharacterExitRoutine.Capacity - 1; i >= 0; i--)
        {
            if (!prevTile.OnCharacterExitRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                prevTile.OnCharacterExitRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
        prevTile.CharacterOnTile = null;
        Vector3 moveVector = new Vector3(target.X - position.X, 0, target.Y - position.Y);
        float time = 0f;
        yield return new WaitUntil(() =>
        {
            time += Time.deltaTime;
            transform.position += moveVector * Time.deltaTime * speed;
            return time >= 1f / speed;
        });
        transform.position = new Vector3(target.X, 0, target.Y);
        position = target;
        targetTile.CharacterOnTile = this;

        for (int i = targetTile.OnCharacterEnterRoutine.Capacity - 1; i >= 0; i--)
        {
            if (!targetTile.OnCharacterEnterRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                targetTile.OnCharacterEnterRoutine.RemoveAt(i);
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
        yield return StartCoroutine(getDmg(dmg));

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
    protected abstract IEnumerator getDmg(int dmg);
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
    public IEnumerator PayCost(int cost, CostType type)
    {
        yield return StartCoroutine(payCost(cost, type));
        for (int i = PayCostRoutine.Capacity - 1; i >= 0; i--)
        {
            if (!PayCostRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                PayCostRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
    }
    protected abstract IEnumerator payCost(int cost, CostType type);
    public abstract bool PayTest(int cost, CostType type);
}
