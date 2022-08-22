using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public abstract class Character : MonoBehaviour
{
    public int cardUseInTurn {get; set;}
    public int attackCardUseInTurn { get; set; }
    public int moveCardUseInTurn { get; set; }
    public int MaxHp { get; set; }
    protected int hp;
    public virtual int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
        }
    }
    public bool IsDie { get; set; }
    private CharacterUIManager effectUI;
    public CharacterUIManager EffectUI
    {
        get
        {
            if (!effectUI)
            {
                effectUI = Instantiate(GameManager.Instance.EffectUIPrefab, PlayerUIManager.Instance.EffectUIs);
                effectUI.Owner = this;
            }
            return effectUI;
        }
    }
    public GameObject HpBar { get; set; }
    private TextMeshProUGUI hpText;
    private RectTransform hpBarImg;
    private const int MAXHANDSIZE = 10;
    private MeshRenderer meshRenderer;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Animator animator;
    public EffectHandler EffectHandler { get; private set; }
    public int TurnStartDraw { get; set; }
    private Coordinate pos;
    public Coordinate PrevPos { get; set; }
    public Coordinate position
    {
        get
        {
            if (pos == null)
            {
                pos = new Coordinate((int)transform.position.x, (int)transform.position.z);
                if (GameManager.Instance.Map[pos.X, pos.Y])
                    GameManager.Instance.Map[pos.X, pos.Y].CharacterOnTile = this;
            }
            return pos;
        }
        set
        {
            PrevPos = pos;
            if (pos != null)
            {
                GameManager.Instance.Map[pos.X, pos.Y].CharacterOnTile = null;
            }
            pos = value;
            if(OnPosChanged != null)
                OnPosChanged();
            GameManager.Instance.Map[pos.X, pos.Y].CharacterOnTile = this;
            transform.position = new Vector3(value.X, transform.position.y, value.Y);
        }
    }
    protected System.Action OnPosChanged { get; set; }
    private int sight = 10;
    public int Sight
    {
        get
        {
            return sight;
        }
        set
        {
            if (this is Player)
            {
                SightUpdate(value);
            }

            sight = value;
        }
    }
    public List<ICard> HandCard { get; private set; } = new List<ICard>();
    public List<ICard> CardPile { get; private set; } = new List<ICard>();
    public List<ICard> DiscardedPile { get; private set; } = new List<ICard>();

    private List<BuffRoutine> startbuffHandler = new();
    public IReadOnlyList<BuffRoutine> StartBuffHandler
    {
        get
        {
            return startbuffHandler.AsReadOnly();
        }
    }
    public void AddStartBuff(IEnumerator routine, int priority)
    {
        startbuffHandler.Add(new BuffRoutine(routine, priority));
        startbuffHandler.Sort();
    }
    public void RemoveStartBuffByIdx(int idx)
    {
        startbuffHandler.RemoveAt(idx);
    }

    private List<BuffRoutine> startDebuffHandler = new();
    public IReadOnlyList<BuffRoutine> StartDebuffHandler
    {
        get
        {
            return startDebuffHandler.AsReadOnly();
        }
    }
    public void AddStartDebuff(IEnumerator routine, int priority)
    {
        startDebuffHandler.Add(new BuffRoutine(routine, priority));
        startDebuffHandler.Sort();
    }
    public void RemoveStartDebuffByIdx(int idx)
    {
        startDebuffHandler.RemoveAt(idx);
    }

    private List<BuffRoutine> drawBuffHandler = new();
    public IReadOnlyList<BuffRoutine> DrawBuffHandler
    {
        get
        {
            return drawBuffHandler.AsReadOnly();
        }
    }
    public void AddDrawBuff(IEnumerator routine, int priority)
    {
        drawBuffHandler.Add(new BuffRoutine(routine, priority));
        drawBuffHandler.Sort();
    }
    public void RemoveDrawBuffByIdx(int idx)
    {
        drawBuffHandler.RemoveAt(idx);
    }

    private List<BuffRoutine> drawDebuffHandler = new();
    public IReadOnlyList<BuffRoutine> DrawDebuffHandler
    {
        get
        {
            return drawDebuffHandler.AsReadOnly();
        }
    }
    public void AddDrawDebuff(IEnumerator routine, int priority)
    {
        drawDebuffHandler.Add(new BuffRoutine(routine, priority));
        drawDebuffHandler.Sort();
    }
    public void RemoveDrawDebuffByIdx(int idx)
    {
        drawDebuffHandler.RemoveAt(idx);
    }

    private List<BuffRoutine> turnEndBuffHandler = new();
    public IReadOnlyList<BuffRoutine> TurnEndBuffHandler
    {
        get
        {
            return turnEndBuffHandler.AsReadOnly();
        }
    }
    public void AddTurnEndBuff(IEnumerator routine, int priority)
    {
        turnEndBuffHandler.Add(new BuffRoutine(routine, priority));
        turnEndBuffHandler.Sort();
    }
    public void RemoveTurnEndBuffByIdx(int idx)
    {
        turnEndBuffHandler.RemoveAt(idx);
    }

    private List<BuffRoutine> turnEndDebuffHandler = new();
    public IReadOnlyList<BuffRoutine> TurnEndDebuffHandler
    {
        get
        {
            return turnEndDebuffHandler.AsReadOnly();
        }
    }
    public void AddTurnEndDebuff(IEnumerator routine, int priority)
    {
        turnEndDebuffHandler.Add(new BuffRoutine(routine, priority));
        turnEndDebuffHandler.Sort();
    }
    public void RemoveTurnEndDebuffByIdx(int idx)
    {
        turnEndDebuffHandler.RemoveAt(idx);
    }

    private List<BuffRoutine> forceTurnEndDebuffHandler = new();
    public IReadOnlyList<BuffRoutine> ForceTurnEndDebuffHandler
    {
        get
        {
            return forceTurnEndDebuffHandler.AsReadOnly();
        }
    }
    public void AddForceTurnEndDebuff(IEnumerator routine, int priority)
    {
        forceTurnEndDebuffHandler.Add(new BuffRoutine(routine, priority));
        forceTurnEndDebuffHandler.Sort();
    }
    public void RemoveForceTurnEndDebuffByIdx(int idx)
    {
        forceTurnEndDebuffHandler.RemoveAt(idx);
    }


    public bool MoveInterrupted { get; set; }

    private List<BuffRoutine> tryMoveRoutine = new();
    public IReadOnlyList<BuffRoutine> TryMoveRoutine
    {
        get
        {
            return tryMoveRoutine.AsReadOnly();
        }
    }
    public void AddTryMoveRoutine(IEnumerator routine, int priority)
    {
        tryMoveRoutine.Add(new BuffRoutine(routine, priority));
        tryMoveRoutine.Sort();
    }
    public void RemoveTryMoveRoutineByIdx(int idx)
    {
        tryMoveRoutine.RemoveAt(idx);
    }

    private List<BuffRoutine> moveRoutine = new();
    public IReadOnlyList<BuffRoutine> MoveRoutine
    {
        get
        {
            return moveRoutine.AsReadOnly();
        }
    }
    public void AddMoveRoutine(IEnumerator routine, int priority)
    {
        moveRoutine.Add(new BuffRoutine(routine, priority));
        moveRoutine.Sort();
    }
    public void RemoveMoveRoutineByIdx(int idx)
    {
        moveRoutine.RemoveAt(idx);
    }

    public Character ForceMovedBy { get; set; }
    public bool ForceMoveInterrupted { get; set; }


    private List<BuffRoutine> tryForceMoveRoutine = new();
    public IReadOnlyList<BuffRoutine> TryForceMoveRoutine
    {
        get
        {
            return tryForceMoveRoutine.AsReadOnly();
        }
    }
    public void AddTryForceMoveRoutine(IEnumerator routine, int priority)
    {
        tryForceMoveRoutine.Add(new BuffRoutine(routine, priority));
        tryForceMoveRoutine.Sort();
    }
    public void RemoveTryForceMoveRoutineByIdx(int idx)
    {
        tryForceMoveRoutine.RemoveAt(idx);
    }

    private List<BuffRoutine> forceMoveRoutine = new();
    public IReadOnlyList<BuffRoutine> ForceMoveRoutine
    {
        get
        {
            return forceMoveRoutine.AsReadOnly();
        }
    }
    public void AddForceMoveRoutine(IEnumerator routine, int priority)
    {
        forceMoveRoutine.Add(new BuffRoutine(routine, priority));
        forceMoveRoutine.Sort();
    }
    public void RemoveForceMoveRoutineByIdx(int idx)
    {
        forceMoveRoutine.RemoveAt(idx);
    }

    public Character HitTarget { get; set; }
    public Character HitBy { get; set; }
    public int Dmg { get; set; }
    public bool GetDmgInterrupted { get; set; }
    private List<BuffRoutine> tryGetDmgRoutine = new();
    public IReadOnlyList<BuffRoutine> TryGetDmgRoutine
    {
        get
        {
            return tryGetDmgRoutine.AsReadOnly();
        }
    }
    public void AddTryGetDmgRoutine(IEnumerator routine, int priority)
    {
        tryGetDmgRoutine.Add(new BuffRoutine(routine, priority));
        tryGetDmgRoutine.Sort();
    }
    public void RemoveTryGetDmgRoutineByIdx(int idx)
    {
        tryGetDmgRoutine.RemoveAt(idx);
    }

    private List<BuffRoutine> getDmgRoutine = new();
    public IReadOnlyList<BuffRoutine> GetDmgRoutine
    {
        get
        {
            return getDmgRoutine.AsReadOnly();
        }
    }
    public void AddGetDmgRoutine(IEnumerator routine, int priority)
    {
        getDmgRoutine.Add(new BuffRoutine(routine, priority));
        getDmgRoutine.Sort();
    }
    public void RemoveGetDmgRoutineByIdx(int idx)
    {
        getDmgRoutine.RemoveAt(idx);
    }

    public bool HitInterrupted { get; set; }
    public int HitDmg { get; set; }

    private List<BuffRoutine> tryHitAttackRoutine = new();
    public IReadOnlyList<BuffRoutine> TryHitAttackRoutine
    {
        get
        {
            return tryHitAttackRoutine.AsReadOnly();
        }
    }
    public void AddTryHitAttackRoutine(IEnumerator routine, int priority)
    {
        tryHitAttackRoutine.Add(new BuffRoutine(routine, priority));
        tryHitAttackRoutine.Sort();
    }
    public void RemoveTryHitAttackRoutineByIdx(int idx)
    {
        tryHitAttackRoutine.RemoveAt(idx);
    }

    private List<BuffRoutine> hitAttackRoutine = new();
    public IReadOnlyList<BuffRoutine> HitAttackRoutine
    {
        get
        {
            return hitAttackRoutine.AsReadOnly();
        }
    }
    public void AddHitAttackRoutine(IEnumerator routine, int priority)
    {
        hitAttackRoutine.Add(new BuffRoutine(routine, priority));
        hitAttackRoutine.Sort();
    }
    public void RemoveHitAttackRoutineByIdx(int idx)
    {
        hitAttackRoutine.RemoveAt(idx);
    }

    public object HealedBy;
    public bool HealInterrupted { get; set; }
    public int HealAmount { get; set; }

    private List<BuffRoutine> tryHealRoutine = new();
    public IReadOnlyList<BuffRoutine> TryHealRoutine
    {
        get
        {
            return tryHealRoutine.AsReadOnly();
        }
    }
    public void AddTryHealRoutine(IEnumerator routine, int priority)
    {
        tryHealRoutine.Add(new BuffRoutine(routine, priority));
        tryHealRoutine.Sort();
    }
    public void RemoveTryHealRoutineByIdx(int idx)
    {
        tryHealRoutine.RemoveAt(idx);
    }

    private List<BuffRoutine> healRoutine = new();
    public IReadOnlyList<BuffRoutine> HealRoutine
    {
        get
        {
            return healRoutine.AsReadOnly();
        }
    }
    public void AddHealRoutine(IEnumerator routine, int priority)
    {
        healRoutine.Add(new BuffRoutine(routine, priority));
        healRoutine.Sort();
    }
    public void RemoveHealRoutineByIdx(int idx)
    {
        healRoutine.RemoveAt(idx);
    }


    public bool GiveHealInterrupted { get; set; }
    public int GiveHealAmount { get; set; }
    private List<BuffRoutine> tryGiveHealRoutine = new();
    public IReadOnlyList<BuffRoutine> TryGiveHealRoutine
    {
        get
        {
            return tryGiveHealRoutine.AsReadOnly();
        }
    }
    public void AddTryGiveHealRoutine(IEnumerator routine, int priority)
    {
        tryGiveHealRoutine.Add(new BuffRoutine(routine, priority));
        tryGiveHealRoutine.Sort();
    }
    public void RemoveTryGiveHealRoutineByIdx(int idx)
    {
        tryGiveHealRoutine.RemoveAt(idx);
    }

    private List<BuffRoutine> giveHealRoutine = new();
    public IReadOnlyList<BuffRoutine> GiveHealRoutine
    {
        get
        {
            return giveHealRoutine.AsReadOnly();
        }
    }
    public void AddGiveHealRoutine(IEnumerator routine, int priority)
    {
        giveHealRoutine.Add(new BuffRoutine(routine, priority));
        giveHealRoutine.Sort();
    }
    public void RemoveGiveHealRoutineByIdx(int idx)
    {
        giveHealRoutine.RemoveAt(idx);
    }

    public Character KilledBy { get; set; }
    public bool DieInterrupted { get; set; }
    private List<BuffRoutine> tryDieRoutine = new();
    public IReadOnlyList<BuffRoutine> TryDieRoutine
    {
        get
        {
            return tryDieRoutine.AsReadOnly();
        }
    }
    public void AddTryDieRoutine(IEnumerator routine, int priority)
    {
        tryDieRoutine.Add(new BuffRoutine(routine, priority));
        tryDieRoutine.Sort();
    }
    public void RemoveTryDieRoutineByIdx(int idx)
    {
        tryDieRoutine.RemoveAt(idx);
    }

    private List<BuffRoutine> dieRoutineList = new();
    public IReadOnlyList<BuffRoutine> DieRoutine
    {
        get
        {
            return dieRoutineList.AsReadOnly();
        }
    }
    public void AddDieRoutine(IEnumerator routine, int priority)
    {
        dieRoutineList.Add(new BuffRoutine(routine, priority));
        dieRoutineList.Sort();
    }
    public void RemoveDieRoutineByIdx(int idx)
    {
        dieRoutineList.RemoveAt(idx);
    }

    public ICard drawCard { get; set; }
    public bool DrawInterrupted { get; set; }
    private List<BuffRoutine> drawCardTry = new();
    public IReadOnlyList<BuffRoutine> DrawCardTry
    {
        get
        {
            return drawCardTry.AsReadOnly();
        }
    }
    public void AddTryDrawRoutine(IEnumerator routine, int priority)
    {
        drawCardTry.Add(new BuffRoutine(routine, priority));
        drawCardTry.Sort();
    }
    public void RemoveTryDrawRoutineByIdx(int idx)
    {
        drawCardTry.RemoveAt(idx);
    }

    private List<BuffRoutine> drawCardRoutine = new();
    public IReadOnlyList<BuffRoutine> DrawCardRoutine
    {
        get
        {
            return drawCardRoutine.AsReadOnly();
        }
    }
    public void AddDrawCardRoutine(IEnumerator routine, int priority)
    {
        drawCardRoutine.Add(new BuffRoutine(routine, priority));
        drawCardRoutine.Sort();
    }
    public void RemoveDrawCardRoutineByIdx(int idx)
    {
        drawCardRoutine.RemoveAt(idx);
    }

    public ICard dropCard { get; set; }
    public bool DropInterrupted { get; set; }
    private List<BuffRoutine> dropCardTry = new();
    public IReadOnlyList<BuffRoutine> DropCardTry
    {
        get
        {
            return dropCardTry.AsReadOnly();
        }
    }
    public void AddTryDropCardRoutine(IEnumerator routine, int priority)
    {
        dropCardTry.Add(new BuffRoutine(routine, priority));
        dropCardTry.Sort();
    }
    public void RemoveTryDropCardRoutineByIdx(int idx)
    {
        dropCardTry.RemoveAt(idx);
    }

    private List<BuffRoutine> dropCardRoutine = new();
    public IReadOnlyList<BuffRoutine> DropCardRoutine
    {
        get
        {
            return dropCardRoutine.AsReadOnly();
        }
    }
    public void AddDropCardRoutine(IEnumerator routine, int priority)
    {
        dropCardRoutine.Add(new BuffRoutine(routine, priority));
        dropCardRoutine.Sort();
    }
    public void RemoveDropCardRoutineByIdx(int idx)
    {
        dropCardRoutine.RemoveAt(idx);
    }

    private List<BuffRoutine> payCostRoutine = new();
    public IReadOnlyList<BuffRoutine> PayCostRoutine
    {
        get
        {
            return payCostRoutine.AsReadOnly();
        }
    }
    public void AddPayCostRoutine(IEnumerator routine, int priority)
    {
        payCostRoutine.Add(new BuffRoutine(routine, priority));
        payCostRoutine.Sort();
    }
    public void RemovePayCostRoutineByIdx(int idx)
    {
        payCostRoutine.RemoveAt(idx);
    }

    public ICard usedCard { get; set; }
    public bool CardUseInterrupted { get; set; }
    private List<BuffRoutine> cardUseTry = new();
    public IReadOnlyList<BuffRoutine> CardUseTry
    {
        get
        {
            return cardUseTry.AsReadOnly();
        }
    }
    public void AddTryCardUseRoutine(IEnumerator routine, int priority)
    {
        cardUseTry.Add(new BuffRoutine(routine, priority));
        cardUseTry.Sort();
    }
    public void RemoveTryCardUseRoutineByIdx(int idx)
    {
        cardUseTry.RemoveAt(idx);
    }

    private List<BuffRoutine> cardUseRoutine = new();
    public IReadOnlyList<BuffRoutine> CardUseRoutine
    {
        get
        {
            return cardUseRoutine.AsReadOnly();
        }
    }
    public void AddCardUseRoutine(IEnumerator routine, int priority)
    {
        cardUseRoutine.Add(new BuffRoutine(routine, priority));
        cardUseRoutine.Sort();
    }
    public void RemoveCardUseRoutineByIdx(int idx)
    {
        cardUseRoutine.RemoveAt(idx);
    }

    public ICard addedCard { get; set; }
    public bool AddCardInterrupted { get; set; }

    private List<BuffRoutine> addCardTry = new();
    public IReadOnlyList<BuffRoutine> AddCardTry
    {
        get
        {
            return addCardTry.AsReadOnly();
        }
    }
    public void AddTryAddCardRoutine(IEnumerator routine, int priority)
    {
        addCardTry.Add(new BuffRoutine(routine, priority));
        addCardTry.Sort();
    }
    public void RemoveTryAddCardRoutineByIdx(int idx)
    {
        addCardTry.RemoveAt(idx);
    }

    private List<BuffRoutine> addCardRoutine = new();
    public IReadOnlyList<BuffRoutine> AddCardRoutine
    {
        get
        {
            return addCardRoutine.AsReadOnly();
        }
    }
    public void AddAddCardRoutine(IEnumerator routine, int priority)
    {
        addCardRoutine.Add(new BuffRoutine(routine, priority));
        addCardRoutine.Sort();
    }
    public void RemoveAddCardRoutineByIdx(int idx)
    {
        addCardRoutine.RemoveAt(idx);
    }

    public ICard removedCard { get; set; }
    public bool RemoveCardInterrupted { get; set; }

    private List<BuffRoutine> removeCardTry = new();
    public IReadOnlyList<BuffRoutine> RemoveCardTry
    {
        get
        {
            return removeCardTry.AsReadOnly();
        }
    }
    public void AddTryRemoveCardRoutine(IEnumerator routine, int priority)
    {
        removeCardTry.Add(new BuffRoutine(routine, priority));
        removeCardTry.Sort();
    }
    public void RemoveTryRemoveCardByIdx(int idx)
    {
        removeCardTry.RemoveAt(idx);
    }

    private List<BuffRoutine> removeCardRoutine = new();
    public IReadOnlyList<BuffRoutine> RemoveCardRoutine
    {
        get
        {
            return removeCardRoutine.AsReadOnly();
        }
    }
    public void AddRemoveCardRoutine(IEnumerator routine, int priority)
    {
        removeCardRoutine.Add(new BuffRoutine(routine, priority));
        removeCardRoutine.Sort();
    }
    public void RemoveRemoveCardRoutineByIdx(int idx)
    {
        removeCardRoutine.RemoveAt(idx);
    }

    public IEnumerator ShuffleDeck()
    {
        //need animation for player
        if (CardPile.Count != 0)
        {
            for (int i = CardPile.Count - 1; !IsDie && i >= 0; i--)
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
        DrawInterrupted = false;
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
        for (int i = DrawCardTry.Count - 1; !DrawInterrupted && !IsDie && i >= 0; i--)
        {
            IEnumerator routine = DrawCardTry[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveTryDrawRoutineByIdx(i);
            }
        }
        if (DrawInterrupted || IsDie)
        {
            DrawInterrupted = false;
            yield break;
        }
        if (CardPile.Count == 0)
        {
            StartCoroutine(ShuffleDeck());
        }
        drawCard = CardPile[0];
        CardPile.RemoveAt(0);
        HandCard.Add(drawCard);
        if (this is Player)
        {
            yield return StartCoroutine(PlayerUIManager.Instance.DrawCard());
        }
        for (int i = DrawCardRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = DrawCardRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveDrawCardRoutineByIdx(i);
            }
        }
        yield break;
    }
    /// <summary>
    /// drop from Hand
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public IEnumerator DropCard(int idx)
    {
        //DropInterrupted = false;
        //need animation for player
        for (int i = DropCardTry.Count - 1; !DropInterrupted && !IsDie && i >= 0; i--)
        {
            IEnumerator routine = DropCardTry[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveTryDropCardRoutineByIdx(i);
            }
        }
        if (DropInterrupted || IsDie)
        {
            DropInterrupted = false;
            yield break;
        }
        dropCard = HandCard[idx];
        DiscardedPile.Add(dropCard);
        HandCard.RemoveAt(idx);
        if (this is Player)
        {
            PlayerUIManager.Instance.DropCard(PlayerUIManager.Instance.CardImages[idx]);
        }
        for (int i = DropCardRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = DropCardRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveDropCardRoutineByIdx(i);
            }
        }
        yield break;
    }
    public IEnumerator CardUse(Coordinate target, int idx)
    {
        cardUseInTurn++;
        if ((HandCard[idx].GetCardID() % 100) >= 10)
            attackCardUseInTurn++;
        if ((HandCard[idx].GetCardID() % 1000) >= 100)
            moveCardUseInTurn++;
        bool disposable = HandCard[idx].Disposable;
        CardUseInterrupted = false;
        ICard card = usedCard = HandCard[idx];        
        yield return StartCoroutine(PayCost(usedCard.GetCost(), usedCard.GetCostType()));
        for (int i = CardUseTry.Count - 1; !CardUseInterrupted && !IsDie && i >= 0; i--)
        {
            IEnumerator routine = CardUseTry[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveTryCardUseRoutineByIdx(i);
            }
        }
        if (CardUseInterrupted || IsDie)
        {
            CardUseInterrupted = false;
            yield break;
        }
        yield return StartCoroutine(usedCard.CardRoutine(this, target));
        for (int i = CardUseRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = CardUseRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveCardUseRoutineByIdx(i);
            }
        }
        idx = HandCard.FindIndex((i) => i == card);
        if (!disposable)
            yield return StartCoroutine(DropCard(idx));
        else
            yield return StartCoroutine(RemoveCard(idx));
    }
    public IEnumerator AddCard(ICard toAdd, bool toHand = false)
    {
        AddCardInterrupted = false;
        //need Animation for Player
        for (int i = AddCardTry.Count - 1; !AddCardInterrupted && !IsDie && i >= 0; i--)
        {
            IEnumerator routine = AddCardTry[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveTryAddCardRoutineByIdx(i);
            }
        }
        if (AddCardInterrupted || IsDie)
        {
            AddCardInterrupted = false;
            yield break;
        }
        addedCard = toAdd;
        yield return StartCoroutine(addedCard.GetCardRoutine(this));
        if(toHand)
        {
            HandCard.Add(addedCard);
            if(this is Player)
            {
                yield return StartCoroutine(PlayerUIManager.Instance.GenerateCardToHand());
            }
        }
        else
        {
            CardPile.Add(addedCard);
        }
        for (int i = AddCardRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = AddCardRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveAddCardRoutineByIdx(i);
            }
        }
    }

    public IEnumerator RemoveCard(int idx)
    {
        RemoveCardInterrupted = false;
        //need Animation for Player
        for (int i = RemoveCardTry.Count - 1; !RemoveCardInterrupted && !IsDie && i >= 0; i--)
        {
            IEnumerator routine = RemoveCardTry[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveTryRemoveCardByIdx(i);
            }
        }
        if (RemoveCardInterrupted || IsDie)
        {
            RemoveCardInterrupted = false;
            yield break;
        }
        removedCard = HandCard[idx];
        yield return StartCoroutine(removedCard.RemoveCardRoutine(this));
        HandCard.RemoveAt(idx);
        if (this is Player)
        {
            PlayerUIManager.Instance.DropCard(PlayerUIManager.Instance.CardImages[idx]);
        }

        for (int i = RemoveCardRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = RemoveCardRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveRemoveCardRoutineByIdx(i);
            }
        }
    }
    public IEnumerator RemoveCard(ICard toRemove, bool discardedPileFirst = false)
    {
        RemoveCardInterrupted = false;
        //need Animation for Player
        for (int i = RemoveCardTry.Count - 1; !RemoveCardInterrupted && !IsDie && i >= 0; i--)
        {
            IEnumerator routine = RemoveCardTry[i].Routine;

            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveTryRemoveCardByIdx(i);
            }
        }
        if (RemoveCardInterrupted || IsDie)
        {
            RemoveCardInterrupted = false;
            yield break;
        }
        removedCard = toRemove;
        yield return StartCoroutine(removedCard.RemoveCardRoutine(this));
        if (discardedPileFirst)
        {
            ICard card = DiscardedPile.Find((x) => x.GetCardID() == removedCard.GetCardID());
            if (card == null)
            {
                card = CardPile.Find((x) => x.GetCardID() == removedCard.GetCardID());
                if (card == null)
                {
                    yield break;
                }
                else
                {
                    CardPile.Remove(card);
                }
            }
            else
            {
                DiscardedPile.Remove(card);
            }
        }
        else
        {
            ICard card = CardPile.Find((x) => x.GetCardID() == removedCard.GetCardID());
            if (card == null)
            {
                card = DiscardedPile.Find((x) => x.GetCardID() == removedCard.GetCardID());
                if (card == null)
                {
                    yield break;
                }
                else
                {
                    DiscardedPile.Remove(card);
                }
            }
            else
            {
                CardPile.Remove(card);
            }
            //CardPile.Remove();
        }

        for (int i = RemoveCardRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = RemoveCardRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveRemoveCardRoutineByIdx(i);
            }
        }
    }
    public void SightUpdate(int newSight, bool posChange = false, Coordinate prevPos = null, bool sightChange = false)
    {
        if (!posChange && sightChange)
        {
            bfs(sight, position, true);
        }
        if (posChange && !sightChange)
        {
            bfs(sight, prevPos, true);
        }
        bfs(newSight, position, true);
    }
    private void bfs(int level, Coordinate center, bool discovered)
    {
        int dist = 1;
        bool[,] visited = new bool[128, 128];
        Queue<Coordinate> queue = new Queue<Coordinate>();
        Queue<Coordinate> nextQueue = new Queue<Coordinate>();
        if (level >= 0)
            queue.Enqueue(center);
        while (dist++ <= level)
        {
            while (queue.Count != 0)
            {
                Coordinate tmp = queue.Dequeue();
                Vector3 target = new Vector3(tmp.X, transform.position.y, tmp.Y);
                if (!Physics.Raycast(transform.position, target - transform.position, Coordinate.EuclideanDist(position, tmp), LayerMask.GetMask("Wall")))
                    GameManager.Instance.Map[tmp.X, tmp.Y].Discovered = discovered;
                Coordinate tile;
                if ((tile = tmp.GetUpTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                }
                if ((tile = tmp.GetRightTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetDownTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetLeftTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
            }
            queue = new Queue<Coordinate>(nextQueue);
            nextQueue.Clear();
        }
        while (queue.Count != 0)
        {
            Coordinate tmp = queue.Dequeue();
            Vector3 target = new Vector3(tmp.X, transform.position.y, tmp.Y);
            if (!Physics.Raycast(transform.position, target - transform.position, Coordinate.EuclideanDist(position, tmp), LayerMask.GetMask("Wall")))
                GameManager.Instance.Map[tmp.X, tmp.Y].Discovered = discovered;
        }
    }
    /// <summary>
    /// 1ĭ �̵�
    /// </summary>
    /// <param name="target"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public IEnumerator Move(Coordinate target, float speed)
    {
        MoveInterrupted = false;
        for (int i = TryMoveRoutine.Count - 1; !MoveInterrupted && !IsDie && i >= 0; i--)
        {
            IEnumerator routine = TryMoveRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveTryMoveRoutineByIdx(i);
            }
        }
        if (MoveInterrupted || IsDie)
        {
            //MoveInterrupted = false;
            yield break;
        }
        Tile prevTile = GameManager.Instance.Map[position.X, position.Y];
        Tile targetTile = GameManager.Instance.Map[target.X, target.Y];
        for (int i = prevTile.OnCharacterExitRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = prevTile.OnCharacterExitRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                prevTile.RemoveOnCharacterExitRoutineByIdx(i);
            }
        }
        Vector3 moveVector = new Vector3(target.X - position.X, 0, target.Y - position.Y).normalized;
        //fixme
        if(tag != "Chess")
            transform.LookAt(transform.position + moveVector);
        //
        float time = 0f;
        if (GameManager.Instance.Map[position.X, position.Y].Discovered)
        {
            while (time <= Coordinate.EuclideanDist(target, position) / speed)
            {
                time += Time.fixedDeltaTime;
                transform.position += moveVector * Time.fixedDeltaTime * speed;
                yield return new WaitForFixedUpdate();
            }
        }
        position = target;
        if (this is Player)
        {
            SightUpdate(sight, true, PrevPos);
        }
        for (int i = targetTile.OnCharacterEnterRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = targetTile.OnCharacterEnterRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                targetTile.RemoveOnCharacterEnterRoutineByIdx(i);
            }
            if (routine.Current != null)
            {
                yield return routine.Current;
            }
        }

        for (int i = MoveRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = MoveRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveMoveRoutineByIdx(i);
            }
        }
    }

    public IEnumerator ForceMove(Character caster, Coordinate target, int speed)
    {
        ForceMoveInterrupted = false;
        ForceMovedBy = caster;
        for (int i = TryForceMoveRoutine.Count - 1; !ForceMoveInterrupted && !IsDie && i >= 0; i--)
        {
            IEnumerator routine = TryForceMoveRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveTryForceMoveRoutineByIdx(i);
            }
        }
        if (ForceMoveInterrupted || IsDie)
        {
            ForceMoveInterrupted = false;
            yield break;
        }

        Tile prevTile = GameManager.Instance.Map[position.X, position.Y];
        Tile targetTile = GameManager.Instance.Map[target.X, target.Y];
        for (int i = prevTile.OnCharacterExitRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = prevTile.OnCharacterExitRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                prevTile.RemoveOnCharacterExitRoutineByIdx(i);
            }
        }
        Vector3 moveVector = new Vector3(target.X - position.X, 0, target.Y - position.Y);
        float time = 0f;
        if (GameManager.Instance.Map[position.X, position.Y].Discovered)
        {
            while (time <= 1f / speed)
            {
                time += Time.fixedDeltaTime;
                transform.position += moveVector * Time.fixedDeltaTime * speed;
                yield return new WaitForFixedUpdate();
            }
        }
        position = target;
        if (this is Player)
        {
            SightUpdate(sight, true, PrevPos);
        }

        for (int i = targetTile.OnCharacterEnterRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = targetTile.OnCharacterEnterRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                targetTile.RemoveOnCharacterEnterRoutineByIdx(i);
            }
        }

        for (int i = ForceMoveRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = ForceMoveRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveForceMoveRoutineByIdx(i);
            }
        }
    }

    public IEnumerator GetDmg(Character caster, int dmg, bool isAbsolute = false)
    {
        GetDmgInterrupted = false;
        HitBy = caster;
        Dmg = dmg;
        if (!isAbsolute)
        {
            for (int i = TryGetDmgRoutine.Count - 1; !GetDmgInterrupted && !IsDie && i >= 0; i--)
            {
                IEnumerator routine = TryGetDmgRoutine[i].Routine;
                if (!routine.MoveNext())
                {
                    if(routine.Current != null)
                    {
                        yield return routine.Current;
                    }
                    RemoveTryGetDmgRoutineByIdx(i);
                }
            }
        }

        if (GetDmgInterrupted || IsDie)
        {
            GetDmgInterrupted = false;
            yield break;
        }
        ///fixme
        SoundManager.Instance.SFX.clip = SoundManager.Instance.SFXDict["hit_1"];
        SoundManager.Instance.SFX.Play();
        /////
        Hp -= Dmg;
        yield return StartCoroutine(getDmg(Dmg));
        if (Hp <= 0)
        {
            yield return StartCoroutine(Die(caster));
        }
        for (int i = GetDmgRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = GetDmgRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveGetDmgRoutineByIdx(i);
            }
        }
    }

    public IEnumerator HitAttack(Character target, int dmg)
    {
        HitInterrupted = false;
        HitDmg = dmg;
        HitTarget = target;
        for (int i = TryHitAttackRoutine.Count - 1; !HitInterrupted && !IsDie && i >= 0; i--)
        {
            IEnumerator routine = TryHitAttackRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveTryHitAttackRoutineByIdx(i);
            }
        }
        if (HitInterrupted || IsDie)
        {
            HitInterrupted = false;
            yield break;
        }
        if (this is Player)
        {
            StartCoroutine(MainCamera.Instance.Shake(0.2f, 0.15f, 0.07f));
        }
        yield return GameManager.Instance.StartCoroutine(target.GetDmg(this, HitDmg));

        for (int i = HitAttackRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = HitAttackRoutine[i].Routine;
            if (!HitAttackRoutine[i].Routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveHitAttackRoutineByIdx(i);
            }
        }
    }
    public IEnumerator GiveHeal(Character target, int amount, bool allowOverMaxHp = false)
    {
        GiveHealInterrupted = false;
        GiveHealAmount = amount;
        for (int i = TryGiveHealRoutine.Count - 1; !GiveHealInterrupted && !IsDie && i >= 0; i--)
        {
            IEnumerator routine = TryGiveHealRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveTryGiveHealRoutineByIdx(i);
            }
        }
        if (GiveHealInterrupted || IsDie)
        {
            GiveHealInterrupted = false;
            yield break;
        }
        yield return StartCoroutine(target.Heal(this, GiveHealAmount, allowOverMaxHp));

        for (int i = GiveHealRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = GiveHealRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveGiveHealRoutineByIdx(i);
            }
        }
    }
    public IEnumerator Heal(object caster, int amount, bool allowOverMaxHp)
    {
        HealInterrupted = false;
        HealedBy = caster;
        HealAmount = amount;
        for (int i = TryHealRoutine.Count - 1; !HealInterrupted && !IsDie && i >= 0; i--)
        {
            IEnumerator routine = TryHealRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveTryHealRoutineByIdx(i);
            }
        }
        if (HealInterrupted || IsDie)
        {
            HealInterrupted = false;
            yield break;
        }
        Hp += HealAmount;
        if (!allowOverMaxHp)
            Hp = Mathf.Clamp(Hp, 0, MaxHp);

        for (int i = HealRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = HealRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveHealRoutineByIdx(i);
            }
        }
    }

    public IEnumerator Die(Character caster)
    {
        DieInterrupted = false;
        KilledBy = caster;
        for (int i = TryDieRoutine.Count - 1; !DieInterrupted && !IsDie && i >= 0; i--)
        {
            IEnumerator routine = TryDieRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveTryDieRoutineByIdx(i);
            }
        }
        if (DieInterrupted)
        {
            DieInterrupted = false;
            yield break;
        }
        yield return StartCoroutine(dieRoutine());

        for (int i = DieRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = DieRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemoveDieRoutineByIdx(i);
            }
        }
        if (this is Enemy)
        {
            TurnManager.Instance.DieEnemyList.Add(this as Enemy);
        }
        else if (this is Player)
        {
            SightUpdate(-1, false, null, true);
            if (this == GameManager.Instance.CharacterSelected)
            {
                GameManager.Instance.GameOver = true;
            }
            TurnManager.Instance.DieAllyList.Add(this as Player);

            for (int i = HandCard.Count - 1; i >= 0; i--)
            {
                yield return StartCoroutine(DropCard(i));
            }
            (GameManager.Instance.CurPlayer as Player).PlayerUI.SetActive(false);
            PlayerUIManager.Instance.UseMode = PlayerUIManager.Instance.ReadyUseMode = PlayerUIManager.Instance.OnRoutine = false;
            TurnManager.Instance.TurnEnd = true;
        }
        GameManager.Instance.Map[position.X, position.Y].CharacterOnTile = null;
        IsDie = true;
        TurnStartDraw = 0;
        gameObject.SetActive(false);
        HpBar.SetActive(false);
    }
    public IEnumerator PayCost(int cost, CostType type)
    {
        yield return StartCoroutine(payCost(cost, type));
        for (int i = PayCostRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = PayCostRoutine[i].Routine;
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                RemovePayCostRoutineByIdx(i);
            }
        }
    }
    protected abstract IEnumerator dieRoutine();
    protected abstract IEnumerator payCost(int cost, CostType type);
    public abstract bool PayTest(int cost, CostType type);
    public abstract IEnumerator AwakeTurn();
    public abstract IEnumerator AfterBuff();
    public abstract IEnumerator AfterDraw();
    public abstract IEnumerator StartTurn();
    protected abstract IEnumerator getDmg(int dmg);
    protected abstract void InitializeDeck();
    protected virtual void Awake()
    {
        InitializeDeck();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        animator = GetComponent<Animator>();
        EffectHandler = new EffectHandler(this);
    }
    protected virtual void Start()
    {
        HpBar = Instantiate(PlayerUIManager.Instance.HpBar, PlayerUIManager.Instance.HpBars);
        HpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);
        hpBarImg = HpBar.transform.GetChild(1).GetComponent<RectTransform>();
        hpText = HpBar.GetComponentInChildren<TextMeshProUGUI>();
    }
    protected virtual void OnDestroy()
    {
        Destroy(HpBar);
        if(effectUI)
            Destroy(effectUI.gameObject);
    }
    protected virtual void Update()
    {
        if (GameManager.Instance.Map[position.X, position.Y] && !GameManager.Instance.Map[position.X, position.Y].Discovered)
        {
            if(meshRenderer)
                meshRenderer.enabled = false;
            if (skinnedMeshRenderer)
                skinnedMeshRenderer.enabled = false;
            if (animator)
                animator.enabled = false;

            HpBar.SetActive(false);
            EffectUI.gameObject.SetActive(false);
            return;
        }
        HpBar.SetActive(true);
        EffectUI.gameObject.SetActive(true);

        if (meshRenderer)
            meshRenderer.enabled = true;
        if (skinnedMeshRenderer)
            skinnedMeshRenderer.enabled = true;
        if (animator)
            animator.enabled = true;
        EffectUI.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 3f);
        HpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);
        hpText.text = $"{Hp}/{MaxHp}";
        hpBarImg.sizeDelta = new Vector2(150 * (float)Hp / MaxHp, hpBarImg.sizeDelta.y);
        hpBarImg.transform.localPosition = new Vector3(-75 + 75 * (float)Hp / MaxHp, 0);
    }
}
