using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public abstract class Character : MonoBehaviour
{
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    private GameObject hpBar;
    private TextMeshProUGUI hpText;
    private RectTransform hpBarImg;
    private const int MAXHANDSIZE = 10;
    private MeshRenderer meshRenderer;
    public Buff BuffHandler { get; private set; }
    public Debuff DebuffHandler { get; private set; } = new Debuff();
    public int TurnStartDraw { get; set; }
    public int NeedWait { get; set; }
    private Coordinate pos;
    public Coordinate PrevPos { get; set; }
    public Coordinate position
    {
        get
        {
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
            GameManager.Instance.Map[pos.X, pos.Y].CharacterOnTile = this;
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

    /// for UI
    public List<BuffType> BuffList { get; set; } = new List<BuffType>();
    public List<Debuff> DebuffList { get; set; } = new List<Debuff>();
    /// 

    public List<IEnumerator> StartBuffHandler { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> StartDebuffHandler { get; private set; } = new List<IEnumerator>();
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

    public int Dmg { get; set; }
    public bool GetDmgInterrupted { get; set; }
    public List<IEnumerator> TryGetDmgRoutine { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> GetDmgRoutine { get; private set; } = new List<IEnumerator>();

    public bool HitInterrupted { get; set; }
    public int HitDmg { get; set; }
    public List<IEnumerator> TryHitAttackRoutine { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> HitAttackRoutine { get; private set; } = new List<IEnumerator>();

    public bool HealInterrupted { get; set; }
    public int HealAmount { get; set; }
    public List<IEnumerator> TryHealRoutine { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> HealRoutine { get; private set; } = new List<IEnumerator>();
    public bool GiveHealInterrupted { get; set; }
    public int GiveHealAmount { get; set; }
    public List<IEnumerator> TryGiveHealRoutine { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> GiveHealRoutine { get; private set; } = new List<IEnumerator>();

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

    public ICard removedCard { get; set; }
    public bool RemoveCardInterrupted { get; set; }
    public List<IEnumerator> RemoveCardTry { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> RemoveCardRoutine { get; private set; } = new List<IEnumerator>();

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

            while (NeedWait != 0) yield return null;
            if (!routine.MoveNext())
            {
                DrawCardTry.RemoveAt(i);
            }
        }
        if (DrawInterrupted)
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
        if (gameObject.tag == "Player")
        {
            yield return StartCoroutine(PlayerUIManager.Instance.DrawCard());
        }
        for (int i = DrawCardRoutine.Count - 1; i >= 0; i--)
        {
            IEnumerator routine = DrawCardRoutine[i];
            while (NeedWait != 0) yield return null;
            if (!routine.MoveNext())
            {
                DrawCardRoutine.RemoveAt(i);
            }
        }
        yield break;
    }
    /// <summary>
    /// drop from deck
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public IEnumerator DropCard(ICard card)
    {
        //need animation for player
        for (int i = DropCardTry.Count - 1; i >= 0; i--)
        {
            IEnumerator routine = DropCardTry[i];
            while (NeedWait != 0) yield return null;
            if (!routine.MoveNext())
            {
                DropCardTry.RemoveAt(i);
            }
        }
        if (DropInterrupted)
        {
            DrawInterrupted = false;
            yield break;
        }
        dropCard = CardPile.Find((x) => x.GetCardID() == card.GetCardID());
        if (dropCard == null)
        {
            Debug.LogError("Wrong DropCard Operation");
            yield break;
        }
        DiscardedPile.Add(dropCard);
        CardPile.Remove(dropCard);
        for (int i = DropCardRoutine.Count - 1; i >= 0; i--)
        {
            IEnumerator routine = DropCardRoutine[i];
            while (NeedWait != 0) yield return null;
            if (!routine.MoveNext())
            {
                DropCardRoutine.RemoveAt(i);
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
        //need animation for player
        for (int i = DropCardTry.Count - 1; i >= 0; i--)
        {
            IEnumerator routine = DropCardTry[i];
            while (NeedWait != 0) yield return null;
            if (!routine.MoveNext())
            {
                DropCardTry.RemoveAt(i);
            }
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
            PlayerUIManager.Instance.DropCard(PlayerUIManager.Instance.CardImages[idx]);
        }
        for (int i = DropCardRoutine.Count - 1; i >= 0; i--)
        {
            IEnumerator routine = DropCardRoutine[i];
            while (NeedWait != 0) yield return null;
            if (!routine.MoveNext())
            {
                DropCardRoutine.RemoveAt(i);
            }
        }
        yield break;
    }
    public IEnumerator CardUse(Coordinate target, int idx)
    {
        usedCard = HandCard[idx];
        yield return StartCoroutine(PayCost(usedCard.GetCost(), usedCard.GetCostType()));
        for (int i = CardUseTry.Count - 1; i >= 0; i--)
        {
            IEnumerator routine = CardUseTry[i];
            while (NeedWait != 0) yield return null;
            if (!routine.MoveNext())
            {
                CardUseTry.RemoveAt(i);
            }
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
            while (NeedWait != 0) yield return null;
            if (!routine.MoveNext())
            {
                CardUseRoutine.RemoveAt(i);
            }
        }
        yield return StartCoroutine(DropCard(idx));
    }
    public IEnumerator AddCard(ICard toAdd)
    {
        //need Animation for Player
        for (int i = AddCardTry.Count - 1; i >= 0; i--)
        {
            IEnumerator routine = AddCardTry[i];
            while (NeedWait != 0) yield return null;

            if (!routine.MoveNext())
            {
                AddCardTry.RemoveAt(i);
            }
        }
        if (AddCardInterrupted)
        {
            AddCardInterrupted = false;
            yield break;
        }
        addedCard = toAdd;
        CardPile.Add(addedCard);
        for (int i = AddCardRoutine.Count - 1; i >= 0; i--)
        {
            IEnumerator routine = AddCardRoutine[i];
            while (NeedWait != 0) yield return null;
            if (!routine.MoveNext())
            {
                AddCardRoutine.RemoveAt(i);
            }
        }
    }
    public IEnumerator RemoveCard(ICard toRemove, bool discardedPileFirst)
    {
        //need Animation for Player
        for (int i = RemoveCardTry.Count - 1; i >= 0; i--)
        {
            IEnumerator routine = RemoveCardTry[i];
            while (NeedWait != 0) yield return null;

            if (!routine.MoveNext())
            {
                RemoveCardTry.RemoveAt(i);
            }
        }
        if (RemoveCardInterrupted)
        {
            RemoveCardInterrupted = false;
            yield break;
        }
        removedCard = toRemove;
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

        for (int i = RemoveCardRoutine.Count - 1; i >= 0; i--)
        {
            IEnumerator routine = RemoveCardRoutine[i];
            while (NeedWait != 0) yield return null;
            if (!routine.MoveNext())
            {
                RemoveCardRoutine.RemoveAt(i);
            }
        }
    }
    public void SightUpdate(int newSight, bool posChange = false, Coordinate prevPos = null, bool sightChange = false)
    {
        if (!posChange && sightChange)
        {
            bfs(sight, position, true, -1);
        }
        if (posChange && !sightChange)
        {
            bfs(sight, prevPos, true, -1);
        }
        bfs(newSight, position, true, 1);
    }
    private void bfs(int level, Coordinate center, bool discovered, int onSight)
    {
        int dist = 1;
        bool[,] visited = new bool[128, 128];
        Queue<Coordinate> queue = new Queue<Coordinate>();
        Queue<Coordinate> nextQueue = new Queue<Coordinate>();
        queue.Enqueue(center);
        while (dist++ <= level)
        {
            while (queue.Count != 0)
            {
                Coordinate tmp = queue.Dequeue();
                GameManager.Instance.Map[tmp.X, tmp.Y].Discovered = discovered;
                GameManager.Instance.Map[tmp.X, tmp.Y].Onsight += onSight;
                if (GameManager.Instance.Map[tmp.X, tmp.Y].Onsight == 0)
                {
                    if (GameManager.Instance.Map[tmp.X, tmp.Y].CharacterOnTile)
                    {
                    }
                }
                else
                {
                    if (GameManager.Instance.Map[tmp.X, tmp.Y].CharacterOnTile)
                    {
                    }
                }
                Coordinate tile;
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
                if ((tile = tmp.GetRightTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetUpTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                }
            }
            queue = new Queue<Coordinate>(nextQueue);
            nextQueue.Clear();
        }
        while (queue.Count != 0)
        {
            Coordinate tmp = queue.Dequeue();
            GameManager.Instance.Map[tmp.X, tmp.Y].Discovered = discovered;
            GameManager.Instance.Map[tmp.X, tmp.Y].Onsight += onSight;
        }
    }
    /// <summary>
    /// 1Ä­ ÀÌµ¿
    /// </summary>
    /// <param name="target"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public IEnumerator Move(Coordinate target, float speed)
    {

        for (int i = TryMoveRoutine.Count - 1; i >= 0; i--)
        {
            while (NeedWait != 0) yield return null;
            if (!TryMoveRoutine[i].MoveNext())
            {
                TryMoveRoutine.RemoveAt(i);
            }
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
            while (NeedWait != 0) yield return null;
            if (!prevTile.OnCharacterExitRoutine[i].MoveNext())
            {
                prevTile.OnCharacterExitRoutine.RemoveAt(i);
            }
        }
        Vector3 moveVector = new Vector3(target.X - position.X, 0, target.Y - position.Y);
        float time = 0f;
        if (GameManager.Instance.Map[position.X, position.Y].Onsight != 0)
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
        for (int i = targetTile.OnCharacterEnterRoutine.Count - 1; i >= 0; i--)
        {
            while (NeedWait != 0) yield return null;
            if (!targetTile.OnCharacterEnterRoutine[i].MoveNext())
            {
                targetTile.OnCharacterEnterRoutine.RemoveAt(i);
            }
        }

        for (int i = MoveRoutine.Count - 1; i >= 0; i--)
        {
            if (!MoveRoutine[i].MoveNext())
            {
                while (NeedWait != 0) yield return null;
                MoveRoutine.RemoveAt(i);
            }
        }
    }

    public IEnumerator ForceMove(Coordinate target, int speed)
    {
        for (int i = TryForceMoveRoutine.Count - 1; i >= 0; i--)
        {
            while (NeedWait != 0) yield return null;
            if (!TryForceMoveRoutine[i].MoveNext())
            {
                TryForceMoveRoutine.RemoveAt(i);
            }
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
            while (NeedWait != 0) yield return null;
            if (!prevTile.OnCharacterExitRoutine[i].MoveNext())
            {
                prevTile.OnCharacterExitRoutine.RemoveAt(i);
            }
        }
        Vector3 moveVector = new Vector3(target.X - position.X, 0, target.Y - position.Y);
        float time = 0f;
        if (GameManager.Instance.Map[position.X, position.Y].Onsight != 0)
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

        for (int i = targetTile.OnCharacterEnterRoutine.Count - 1; i >= 0; i--)
        {
            while (NeedWait != 0) yield return null;
            if (!targetTile.OnCharacterEnterRoutine[i].MoveNext())
            {
                targetTile.OnCharacterEnterRoutine.RemoveAt(i);
            }
        }

        for (int i = ForceMoveRoutine.Count - 1; i >= 0; i--)
        {
            while (NeedWait != 0) yield return null;
            if (!ForceMoveRoutine[i].MoveNext())
            {
                ForceMoveRoutine.RemoveAt(i);
            }
        }
    }

    public IEnumerator GetDmg(int dmg)
    {
        Dmg = dmg;
        for (int i = TryGetDmgRoutine.Count - 1; i >= 0; i--)
        {
            while (NeedWait != 0) yield return null;
            if (!TryGetDmgRoutine[i].MoveNext())
            {
                TryGetDmgRoutine.RemoveAt(i);
            }
        }
        if (GetDmgInterrupted)
        {
            GetDmgInterrupted = false;
            yield break;
        }
        Hp -= Dmg;
        yield return StartCoroutine(getDmg(Dmg));
        if (Hp <= 0)
        {
            StartCoroutine(Die());
            if (!gameObject)
                yield break;
        }
        for (int i = GetDmgRoutine.Count - 1; i >= 0; i--)
        {
            while (NeedWait != 0) yield return null;
            if (!GetDmgRoutine[i].MoveNext())
            {
                GetDmgRoutine.RemoveAt(i);
            }
        }
    }

    public IEnumerator HitAttack(Character target, int dmg)
    {
        HitDmg = dmg;
        for (int i = TryHitAttackRoutine.Count - 1; i >= 0; i--)
        {
            while (NeedWait != 0) yield return null;
            if (!TryHitAttackRoutine[i].MoveNext())
            {
                TryHitAttackRoutine.RemoveAt(i);
            }
        }
        if (HitInterrupted)
        {
            HitInterrupted = false;
            yield break;
        }
        yield return StartCoroutine(target.GetDmg(HitDmg));

        for (int i = HitAttackRoutine.Count - 1; i >= 0; i--)
        {
            while (NeedWait != 0) yield return null;
            if (!HitAttackRoutine[i].MoveNext())
            {
                HitAttackRoutine.RemoveAt(i);
            }
        }
    }
    public IEnumerator GiveHeal(Character target, int amount)
    {
        GiveHealAmount = amount;
        for (int i = TryGiveHealRoutine.Count - 1; i >= 0; i--)
        {
            while (NeedWait != 0) yield return null;
            if (!TryGiveHealRoutine[i].MoveNext())
            {
                TryGiveHealRoutine.RemoveAt(i);
            }
        }
        if (GiveHealInterrupted)
        {
            GiveHealInterrupted = false;
            yield break;
        }
        yield return StartCoroutine(target.Heal(GiveHealAmount));

        for (int i = GiveHealRoutine.Count - 1; i >= 0; i--)
        {
            while (NeedWait != 0) yield return null;
            if (!GiveHealRoutine[i].MoveNext())
            {
                GiveHealRoutine.RemoveAt(i);
            }
        }
    }
    private IEnumerator Heal(int amount)
    {
        HealAmount = amount;
        for (int i = TryHealRoutine.Count - 1; i >= 0; i--)
        {
            while (NeedWait != 0) yield return null;
            if (!TryHealRoutine[i].MoveNext())
            {
                TryHealRoutine.RemoveAt(i);
            }
        }
        if (HealInterrupted)
        {
            HealInterrupted = false;
            yield break;
        }
        Hp += HealAmount;

        for (int i = HealRoutine.Count - 1; i >= 0; i--)
        {
            while (NeedWait != 0) yield return null;
            if (!HealRoutine[i].MoveNext())
            {
                HealRoutine.RemoveAt(i);
            }
        }
    }

    public IEnumerator Die()
    {
        for (int i = TryDieRoutine.Count - 1; i >= 0; i--)
        {
            while (NeedWait != 0) yield return null;
            if (!TryDieRoutine[i].MoveNext())
            {
                TryDieRoutine.RemoveAt(i);
            }
        }
        if (DieInterrupted)
        {
            DieInterrupted = false;
            yield break;
        }
        yield return StartCoroutine(dieRoutine());

        for (int i = DieRoutine.Count - 1; i >= 0; i--)
        {
            while (NeedWait != 0) yield return null;
            if (!DieRoutine[i].MoveNext())
            {
                DieRoutine.RemoveAt(i);
            }
        }
        if (this is Enemy)
        {
            GameManager.Instance.EnemyList.Remove(this);
        }
        else if(this is Player)
        {
            GameManager.Instance.Allies.Remove(this);
        }
        Destroy(gameObject);
        Destroy(hpBar);
    }
    public IEnumerator PayCost(int cost, CostType type)
    {
        yield return StartCoroutine(payCost(cost, type));
        for (int i = PayCostRoutine.Count - 1; i >= 0; i--)
        {
            while (NeedWait != 0) yield return null;
            if (!PayCostRoutine[i].MoveNext())
            {
                PayCostRoutine.RemoveAt(i);
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
        meshRenderer = GetComponent<MeshRenderer>();
        BuffHandler = new Buff(this);
    }
    protected virtual void Start()
    {
        hpBar = Instantiate(PlayerUIManager.Instance.HpBar, PlayerUIManager.Instance.HpBars);
        hpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);
        hpBarImg = hpBar.transform.GetChild(1).GetComponent<RectTransform>();
        hpText = hpBar.GetComponentInChildren<TextMeshProUGUI>();
    }
    protected virtual void Update()
    {
        if (GameManager.Instance.Map[position.X, position.Y].Onsight == 0)
        {
            meshRenderer.enabled = false;
            hpBar.SetActive(false);
            return;
        }
        hpBar.SetActive(true);
        meshRenderer.enabled = true;
        hpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);
        hpText.text = $"{Hp}/{MaxHp}";
        hpBarImg.sizeDelta = new Vector2(150 * (float)Hp / MaxHp, hpBarImg.sizeDelta.y);
        hpBarImg.transform.localPosition = new Vector3(-75 + 75 * (float)Hp / MaxHp, 0);
    }
}
