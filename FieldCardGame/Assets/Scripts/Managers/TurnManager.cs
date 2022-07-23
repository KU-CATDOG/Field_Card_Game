using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; set; }
    public List<Player> DieAllyList { get; private set; } = new List<Player>();
    public List<Enemy> DieEnemyList { get; private set; } = new List<Enemy>();
    private int token = 0;
    public int Token
    {
        get
        {
            return token;
        }
        set
        {
            token = value;
        }
    }
    public bool NeedWait { get; set; }
    public bool IsPlayerTurn { get; set; } = true;
    public bool TurnEnd { get; set; } = false;
    public int PlayerIdx { get; set; }
    public int EnemyIdx { get; set; }
    private List<BuffRoutine> turnStartRoutine = new();
    public IReadOnlyList<BuffRoutine> TurnStartRoutine
    {
        get
        {
            return turnStartRoutine.AsReadOnly();
        }
    }
    public void AddTurnStartRoutine(IEnumerator routine, int priority)
    {
        turnStartRoutine.Add(new BuffRoutine(routine, priority));
        turnStartRoutine.Sort();
    }
    public void RemoveTurnStartRoutineByIdx(int idx)
    {
        turnStartRoutine.RemoveAt(idx);
    }

    public bool GameEnd { get; set; } = false;
    public void Initialize()
    {
        DieAllyList.Clear();
        DieEnemyList.Clear();
        token = 0;
        NeedWait = false;
        IsPlayerTurn = true;
        TurnEnd = false;
        turnStartRoutine.Clear();
        GameEnd = false;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator TurnRoutine()
    {
        Character curChar;
        while (true)
        {
            yield return StartCoroutine(TurnAwakeRoutine());
            if (token == 0)
            {
                for(PlayerIdx = 0; PlayerIdx < GameManager.Instance.Allies.Count; PlayerIdx++)
                {
                    var j = GameManager.Instance.Allies[PlayerIdx];
                    if (j.IsDie) continue;
                    GameManager.Instance.CurPlayer = curChar = j;
                    (GameManager.Instance.CurPlayer as Player).PlayerUI.SetActive(true);
                    yield return StartCoroutine(curChar.AwakeTurn());
                    if (curChar.StartBuffHandler.Count != 0)
                    {
                        yield return StartCoroutine(StartBuffRoutine(curChar));
                    }
                    if (curChar.StartDebuffHandler.Count != 0)
                        yield return StartCoroutine(StartDebuffRoutine(curChar));
                    if (curChar.ForceTurnEndDebuffHandler.Count != 0)
                    {
                        yield return StartCoroutine(ForceTurnEndDebuffRoutine(curChar));
                        yield return StartCoroutine(TurnEndRoutine(curChar));
                        continue;
                    }
                    if (j.IsDie) continue;
                    yield return StartCoroutine(curChar.AfterBuff());
                    for (int i = 0; i < curChar.TurnStartDraw; i++)
                    {
                        yield return StartCoroutine(curChar.DrawCard());
                    }
                    if (j.IsDie) continue;
                    yield return StartCoroutine(curChar.AfterDraw());
                    if (curChar.DrawBuffHandler.Count != 0)
                        yield return StartCoroutine(DrawBuffRoutine(curChar));
                    if (curChar.DrawDebuffHandler.Count != 0)
                        yield return StartCoroutine(DrawDebuffRoutine(curChar));
                    if (j.IsDie) continue;
                    yield return StartCoroutine(curChar.StartTurn());
                    TurnEnd = false;
                    if (j.IsDie) continue;
                    yield return new WaitUntil(() => { return !PlayerUIManager.Instance.UseMode && TurnEnd; });
                    if (curChar.TurnEndBuffHandler.Count != 0)
                        yield return StartCoroutine(TurnEndBuffRoutine(curChar));
                    if (curChar.TurnEndDebuffHandler.Count != 0)
                        yield return StartCoroutine(TurnEndDebuffRoutine(curChar));
                    for (int i = curChar.HandCard.Count - 1; i >= 0; i--)
                    {
                        yield return StartCoroutine(curChar.DropCard(i));
                    }
                    (GameManager.Instance.CurPlayer as Player).PlayerUI.SetActive(false);
                }
                ApplyDie();
                yield return StartCoroutine(TurnEndRoutine(null));
            }
            else if (token == 1)
            {
                for (EnemyIdx = 0; EnemyIdx < GameManager.Instance.EnemyList.Count; EnemyIdx++)
                {
                    var j = GameManager.Instance.EnemyList[EnemyIdx];
                    curChar = j;
                    if (j.IsDie) continue;
                    yield return StartCoroutine(curChar.AwakeTurn());
                    if (curChar.StartBuffHandler.Count != 0)
                        yield return StartCoroutine(StartBuffRoutine(curChar));
                    if (curChar.StartDebuffHandler.Count != 0)
                        yield return StartCoroutine(StartDebuffRoutine(curChar));
                    if (curChar.ForceTurnEndDebuffHandler.Count != 0)
                    {
                        yield return StartCoroutine(ForceTurnEndDebuffRoutine(curChar));
                        yield return StartCoroutine(TurnEndRoutine(curChar));
                        continue;
                    }
                    if (j.IsDie) continue;
                    yield return StartCoroutine(curChar.AfterBuff());
                    for (int i = 0; i < curChar.TurnStartDraw; i++)
                        yield return StartCoroutine(curChar.DrawCard());
                    if (j.IsDie) continue;
                    yield return StartCoroutine(curChar.AfterDraw());
                    if (curChar.DrawBuffHandler.Count != 0)
                        yield return StartCoroutine(DrawBuffRoutine(curChar));
                    if (curChar.DrawDebuffHandler.Count != 0)
                        yield return StartCoroutine(DrawDebuffRoutine(curChar));
                    if (j.IsDie) continue;
                    yield return StartCoroutine(curChar.StartTurn());
                    if (j.IsDie) continue;
                    yield return StartCoroutine((curChar as Enemy).EnemyRoutine());
                    if (curChar.TurnEndBuffHandler.Count != 0)
                        yield return StartCoroutine(TurnEndBuffRoutine(curChar));
                    if (curChar.TurnEndDebuffHandler.Count != 0)
                        yield return StartCoroutine(TurnEndDebuffRoutine(curChar));
                    for (int i = curChar.HandCard.Count - 1; i >= 0; i--)
                    {
                        yield return StartCoroutine(curChar.DropCard(i));
                    }
                }
                ApplyDie();
                yield return StartCoroutine(TurnEndRoutine(null));
            }
            else
            {
                break;
            }
        }
    }

    private void ApplyDie()
    {
        foreach (Enemy i in DieEnemyList)
        {
            GameManager.Instance.EnemyList.Remove(i);
            Destroy(i.HpBar);
            Destroy(i.gameObject);
        }
        foreach (Player i in DieAllyList)
        {
            GameManager.Instance.Allies.Remove(i);
            Destroy(i.HpBar);
            Destroy(i.gameObject);
        }
        DieEnemyList.Clear();
        DieAllyList.Clear();
    }

    private IEnumerator TurnAwakeRoutine()
    {
        for (int i = TurnStartRoutine.Count - 1; i >= 0; i--)
        {
            if (!TurnStartRoutine[i].Routine.MoveNext())
            {
                while (NeedWait) yield return null;
                RemoveTurnStartRoutineByIdx(i);
            }
            while (NeedWait) yield return null;
        }
    }
    private IEnumerator StartBuffRoutine(Character curChar)
    {
        for (int i = curChar.StartBuffHandler.Count - 1; !curChar.IsDie && i >= 0; i--)
        {
            while (curChar.NeedWait != 0) yield return null;
            if (!curChar.StartBuffHandler[i].Routine.MoveNext())
            {
                curChar.RemoveStartBuffByIdx(i);
            }
        }
    }
    private IEnumerator StartDebuffRoutine(Character curChar)
    {
        for (int i = curChar.StartDebuffHandler.Count - 1; !curChar.IsDie && i >= 0; i--)
        {
            while (curChar.NeedWait != 0) yield return null;
            if (!curChar.StartDebuffHandler[i].Routine.MoveNext())
            {
                curChar.RemoveStartDebuffByIdx(i);
            }
        }
    }
    private IEnumerator DrawBuffRoutine(Character curChar)
    {
        for (int i = curChar.DrawBuffHandler.Count - 1; !curChar.IsDie && i >= 0; i--)
        {
            while (curChar.NeedWait != 0) yield return null;
            if (!curChar.DrawBuffHandler[i].Routine.MoveNext())
            {
                curChar.RemoveDrawBuffByIdx(i);
            }
        }
    }
    private IEnumerator DrawDebuffRoutine(Character curChar)
    {
        for (int i = curChar.DrawDebuffHandler.Count - 1; !curChar.IsDie && i >= 0; i--)
        {
            while (curChar.NeedWait != 0) yield return null;
            if (!curChar.DrawDebuffHandler[i].Routine.MoveNext())
            {
                curChar.RemoveDrawDebuffByIdx(i);
            }
        }
    }
    private IEnumerator ForceTurnEndDebuffRoutine(Character curChar)
    {
        for (int i = curChar.ForceTurnEndDebuffHandler.Count - 1; !curChar.IsDie && i >= 0; i--)
        {
            while (curChar.NeedWait != 0) yield return null;
            if (!curChar.ForceTurnEndDebuffHandler[i].Routine.MoveNext())
            {
                curChar.RemoveForceTurnEndDebuffByIdx(i);
            }
        }
    }
    private IEnumerator TurnEndBuffRoutine(Character curChar)
    {
        for (int i = curChar.TurnEndBuffHandler.Count - 1; !curChar.IsDie && i >= 0; i--)
        {
            while (curChar.NeedWait != 0) yield return null;
            if (!curChar.TurnEndBuffHandler[i].Routine.MoveNext())
            {
                curChar.RemoveTurnEndBuffByIdx(i);
            }
        }
    }
    private IEnumerator TurnEndDebuffRoutine(Character curChar)
    {
        for (int i = curChar.TurnEndDebuffHandler.Count - 1; !curChar.IsDie && i >= 0; i--)
        {
            while (curChar.NeedWait != 0) yield return null;
            if (!curChar.TurnEndDebuffHandler[i].Routine.MoveNext())
            {
                curChar.RemoveTurnEndDebuffByIdx(i);
            }
        }
    }
    private IEnumerator TurnEndRoutine(Character curChar)
    {
        if (GameManager.Instance.GameOver)
        {
            token = 3;
        }
        else if (curChar != null)
        {
            yield return StartCoroutine(TurnEndBuffRoutine(curChar));
            yield return StartCoroutine(TurnEndDebuffRoutine(curChar));
            for (int i = curChar.HandCard.Count - 1; !curChar.IsDie && i >= 0; i--)
            {
                yield return StartCoroutine(curChar.DropCard(i));
            }
        }
        else
        {
            token = token == 0 ? 1 : 0;
        }
    }
}
