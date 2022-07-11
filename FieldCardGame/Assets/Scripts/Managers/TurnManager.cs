using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
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
    public List<IEnumerator> TurnStartRoutine { get; set; } = new List<IEnumerator>();

    public bool GameEnd { get; set; } = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        StartCoroutine(TurnRoutine());
    }

    private IEnumerator TurnRoutine()
    {
        Character curChar;
        while (true)
        {
            yield return StartCoroutine(TurnAwakeRoutine());
            if (token == 0)
            {
                foreach (var j in GameManager.Instance.Allies)
                {
                    GameManager.Instance.CurPlayer = curChar = j;
                    yield return StartCoroutine(curChar.AwakeTurn());
                    if(curChar.StartBuffHandler.Count!=0)
                        yield return StartCoroutine(StartBuffRoutine(curChar));
                    if(curChar.StartDebuffHandler.Count!=0)
                        yield return StartCoroutine(StartDebuffRoutine(curChar));
                    if (curChar.ForceTurnEndDebuffHandler.Count != 0)
                    {
                        yield return StartCoroutine(ForceTurnEndDebuffRoutine(curChar));
                        yield return StartCoroutine(TurnEndRoutine(curChar));
                        continue;
                    }
                    yield return StartCoroutine(curChar.AfterBuff());
                    for (int i = 0; i < curChar.TurnStartDraw; i++)
                    {
                        yield return StartCoroutine(curChar.DrawCard());
                    }
                    yield return StartCoroutine(curChar.AfterDraw());
                    if(curChar.DrawBuffHandler.Count != 0)
                        yield return StartCoroutine(DrawBuffRoutine(curChar));
                    if (curChar.DrawDebuffHandler.Count != 0)
                        yield return StartCoroutine(DrawDebuffRoutine(curChar));
                    yield return StartCoroutine(curChar.StartTurn());
                    TurnEnd = false;
                    yield return new WaitUntil(() => { return !PlayerUIManager.Instance.UseMode && TurnEnd; });
                    if(curChar.TurnEndBuffHandler.Count!=0)
                        yield return StartCoroutine(TurnEndBuffRoutine(curChar));
                    if (curChar.TurnEndDebuffHandler.Count != 0)
                        yield return StartCoroutine(TurnEndDebuffRoutine(curChar));
                    for (int i = curChar.HandCard.Count - 1; i >= 0; i--)
                    {
                        yield return StartCoroutine(curChar.DropCard(i));
                    }
                }
                yield return StartCoroutine(TurnEndRoutine(null));
            }
            else
            {
                foreach (var j in GameManager.Instance.EnemyList)
                {
                    curChar = j;
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
                    yield return StartCoroutine(curChar.AfterBuff());
                    for (int i = 0; i < curChar.TurnStartDraw; i++)
                        yield return StartCoroutine(curChar.DrawCard());
                    yield return StartCoroutine(curChar.AfterDraw());
                    if (curChar.DrawBuffHandler.Count != 0)
                        yield return StartCoroutine(DrawBuffRoutine(curChar));
                    if (curChar.DrawDebuffHandler.Count != 0)
                        yield return StartCoroutine(DrawDebuffRoutine(curChar));
                    yield return StartCoroutine(curChar.StartTurn());
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
                StartCoroutine(TurnEndRoutine(null));
            }
        }
    }
    private IEnumerator TurnAwakeRoutine()
    {
        for (int i = TurnStartRoutine.Count - 1; i >= 0; i--)
        {
            if (!TurnStartRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                TurnStartRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
    }
    private IEnumerator StartBuffRoutine(Character curChar)
    {
        for (int i = curChar.StartBuffHandler.Count - 1; i >= 0; i--)
        {
            if (!curChar.StartBuffHandler[i].MoveNext())
            {
                while (curChar.NeedWait) yield return null;
                curChar.StartBuffHandler.RemoveAt(i);
            }
            while (curChar.NeedWait) yield return null;
        }
    }
    private IEnumerator StartDebuffRoutine(Character curChar)
    {
        for (int i = curChar.StartDebuffHandler.Count - 1; i >= 0; i--)
        {
            if (!curChar.StartDebuffHandler[i].MoveNext())
            {
                while (curChar.NeedWait) yield return null;
                curChar.StartDebuffHandler.RemoveAt(i);
            }
            while (curChar.NeedWait) yield return null;
        }
    }
    private IEnumerator DrawBuffRoutine(Character curChar)
    {
        for (int i = curChar.DrawBuffHandler.Count - 1; i >= 0; i--)
        {
            if (!curChar.DrawBuffHandler[i].MoveNext())
            {
                while (curChar.NeedWait) yield return null;
                curChar.DrawBuffHandler.RemoveAt(i);
            }
            while (curChar.NeedWait) yield return null;
        }
    }
    private IEnumerator DrawDebuffRoutine(Character curChar)
    {
        for (int i = curChar.DrawDebuffHandler.Count - 1; i >= 0; i--)
        {
            if (!curChar.DrawDebuffHandler[i].MoveNext())
            {
                while (curChar.NeedWait) yield return null;
                curChar.DrawDebuffHandler.RemoveAt(i);
            }
            while (curChar.NeedWait) yield return null;
        }
    }
    private IEnumerator ForceTurnEndDebuffRoutine(Character curChar)
    {
        for (int i = curChar.ForceTurnEndDebuffHandler.Count - 1; i >= 0; i--)
        {
            if (!curChar.ForceTurnEndDebuffHandler[i].MoveNext())
            {
                while (curChar.NeedWait) yield return null;
                curChar.ForceTurnEndDebuffHandler.RemoveAt(i);
            }
            while (curChar.NeedWait) yield return null;
        }
    }
    private IEnumerator TurnEndBuffRoutine(Character curChar)
    {
        for (int i = curChar.TurnEndBuffHandler.Count - 1; i >= 0; i--)
        {
            if (!curChar.TurnEndBuffHandler[i].MoveNext())
            {
                while (curChar.NeedWait) yield return null;
                curChar.TurnEndBuffHandler.RemoveAt(i);
            }
            while (curChar.NeedWait) yield return null;
        }
    }
    private IEnumerator TurnEndDebuffRoutine(Character curChar)
    {
        for (int i = curChar.TurnEndDebuffHandler.Count - 1; i >= 0; i--)
        {
            if (!curChar.TurnEndDebuffHandler[i].MoveNext())
            {
                while (curChar.NeedWait) yield return null;
                curChar.TurnEndDebuffHandler.RemoveAt(i);
            }
            while (curChar.NeedWait) yield return null;
        }
    }
    private IEnumerator TurnEndRoutine(Character curChar)
    {
        token = token == 0 ? 1 : 0;
        if (curChar != null)
        {
            yield return StartCoroutine(TurnEndBuffRoutine(curChar));
            yield return StartCoroutine(TurnEndDebuffRoutine(curChar));
            for (int i = curChar.HandCard.Count - 1; i >= 0; i--)
            {
                yield return StartCoroutine(curChar.DropCard(i));
            }
        }
    }
}
