using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    private const int MAXHANDSIZE = 10;
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
            transform.position = new Vector3(value.X, 1, value.Y);
        }
    }
    private int sight = 10;
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


    public ICard addedCard { get; set; }
    public bool AddCardInterrupted { get; set; }
    public List<IEnumerator> AddCardTry { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> AddCardRoutine { get; private set; } = new List<IEnumerator>();

    public abstract IEnumerator AwakeTurn();
    public abstract IEnumerator AfterBuff();
    public abstract IEnumerator AfterDraw();
    public abstract IEnumerator StartTurn();
    public IEnumerator ShuffleDeck()
    {
        //need animation for player
        if (CardPile.Count != 0)
        {
            for (int i = CardPile.Count - 1; i >= 0; i--)
            {
                DiscardedPile.Add(CardPile[i]);
                CardPile.RemoveAt(i);
            }
        }
        while (DiscardedPile.Count != 0)
        {
            int rand = Random.Range(0, DiscardedPile.Count);
            CardPile.Add(DiscardedPile[rand]);
            DiscardedPile.RemoveAt(rand);
        }
        yield break;
    }
    public IEnumerator DrawCard()
    {
        if (CardPile.Count == 0)
        {
            yield return StartCoroutine(ShuffleDeck());
            if (CardPile.Count == 0)
                yield break;
        }
        if (HandCard.Count == MAXHANDSIZE)
        {
            //need animation for player
            yield break;
        }
        //need animation for player
        for (int i = DrawCardTry.Count - 1; i >= 0; i--)
        {
            IEnumerator routine = DrawCardTry[i];

            if (!routine.MoveNext())
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
        if (gameObject.tag == "Player")
        {
            yield return StartCoroutine(PlayerUIManager.Instance.DrawCard());
        }
        for (int i = DrawCardRoutine.Count - 1; i >= 0; i--)
        {
            IEnumerator routine = DrawCardRoutine[i];
            if (!routine.MoveNext())
            {
                while (NeedWait) yield return null;
                DrawCardRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
        Debug.Log("DrawCard " + drawCard + " Card Pile Count:" + CardPile.Count + ", Hand Count: " + HandCard.Count);
        yield break;
    }
    public IEnumerator DropCard(int idx)
    {
        //need animation for player
        for (int i = DropCardTry.Count - 1; i >= 0; i--)
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
        dropCard = HandCard[idx];
        DiscardedPile.Add(dropCard);
        HandCard.RemoveAt(idx);
        if (gameObject.tag == "Player")
        {
            yield return StartCoroutine(PlayerUIManager.Instance.DropCard(GameManager.Instance.CardObjectList[dropCard.GetCardID()]));
        }
        for (int i = DropCardRoutine.Count - 1; i >= 0; i--)
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
    public IEnumerator CardUse(coordinate target, int idx)
    {
        usedCard = HandCard[idx];
        yield return StartCoroutine(PayCost(usedCard.GetCost(), usedCard.GetCostType()));
        for (int i = CardUseTry.Count - 1; i >= 0; i--)
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
        yield return StartCoroutine(usedCard.CardRoutine(this, target));
        for (int i = CardUseRoutine.Count - 1; i >= 0; i--)
        {
            IEnumerator routine = CardUseRoutine[i];
            if (!routine.MoveNext())
            {
                while (NeedWait) yield return null;
                CardUseRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
        yield return StartCoroutine(DropCard(idx));
    }
    public IEnumerator AddCard()
    {
        //need Animation for Player
        for (int i = AddCardTry.Count - 1; i >= 0; i--)
        {
            IEnumerator routine = AddCardTry[i];

            if (!routine.MoveNext())
            {
                while (NeedWait) yield return null;
                AddCardTry.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
        if (AddCardInterrupted)
        {
            AddCardInterrupted = false;
            yield break;
        }
        CardPile.Add(addedCard);
        for (int i = AddCardRoutine.Count - 1; i >= 0; i--)
        {
            IEnumerator routine = AddCardRoutine[i];
            if (!routine.MoveNext())
            {
                while (NeedWait) yield return null;
                AddCardRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
        Debug.Log("AddCard," + addedCard + " Card Pile Count:" + CardPile.Count + ", Hand Count: " + HandCard.Count);
    }
    public void SightUpdate(int newSight, bool posChange = false, coordinate prevPos = null)
    {
        if (!posChange)
        {
            dfs(0, sight, position, true, false);
        }
        else
        {
            dfs(0, sight, prevPos, true, false);
        }
        dfs(0, newSight, position, true, true);
    }
    private void dfs(int level, int limit, coordinate now, bool discovered, bool onSight)
    {
        if (level > limit)
            return;
        GameManager.Instance.Map[now.X, now.Y].Discovered = discovered;
        GameManager.Instance.Map[now.X, now.Y].Onsight = onSight;
        if (now.GetDownTile() != null)
        {
            dfs(level + 1, limit, now.GetDownTile(), discovered, onSight);
        }
        if (now.GetUpTile() != null)
        {
            dfs(level + 1, limit, now.GetUpTile(), discovered, onSight);

        }
        if (now.GetLeftTile() != null)
        {
            dfs(level + 1, limit, now.GetLeftTile(), discovered, onSight);

        }
        if (now.GetRightTile() != null)
        {
            dfs(level + 1, limit, now.GetRightTile(), discovered, onSight);
        }
    }
    /// <summary>
    /// 1ĭ �̵�
    /// </summary>
    /// <param name="target"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public IEnumerator Move(coordinate target, float speed)
    {
        for (int i = TryMoveRoutine.Count - 1; i >= 0; i--)
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
        for (int i = prevTile.OnCharacterExitRoutine.Count - 1; i >= 0; i--)
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
            return time > 1f / speed;
        });
        transform.position = new Vector3(target.X, 0, target.Y);
        coordinate prevPos = position;
        position = target;
        targetTile.CharacterOnTile = this;
        SightUpdate(sight, true, prevPos);

        for (int i = targetTile.OnCharacterEnterRoutine.Count - 1; i >= 0; i--)
        {
            if (!targetTile.OnCharacterEnterRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                targetTile.OnCharacterEnterRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }

        for (int i = MoveRoutine.Count - 1; i >= 0; i--)
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
        for (int i = TryForceMoveRoutine.Count - 1; i >= 0; i--)
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
        for (int i = prevTile.OnCharacterExitRoutine.Count - 1; i >= 0; i--)
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

        for (int i = targetTile.OnCharacterEnterRoutine.Count - 1; i >= 0; i--)
        {
            if (!targetTile.OnCharacterEnterRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                targetTile.OnCharacterEnterRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }

        for (int i = ForceMoveRoutine.Count - 1; i >= 0; i--)
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
        for (int i = TryGetDmgRoutine.Count - 1; i >= 0; i--)
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

        for (int i = GetDmgRoutine.Count - 1; i >= 0; i--)
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
        for (int i = TryDieRoutine.Count - 1; i >= 0; i--)
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

        for (int i = DieRoutine.Count - 1; i >= 0; i--)
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
        for (int i = PayCostRoutine.Count - 1; i >= 0; i--)
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
