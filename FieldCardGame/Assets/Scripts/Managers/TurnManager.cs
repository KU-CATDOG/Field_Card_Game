using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    private int token = 0;
    public bool NeedWait { get; set; }
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
                foreach(var j in GameManager.Instance.Allies)
                {
                    curChar = j;
                    TurnEnd = false;
                    yield return StartCoroutine(curChar.AwakeTurn());
                    yield return StartCoroutine(BuffRoutine(curChar));
                    yield return StartCoroutine(DebuffRoutine(curChar));
                    if (curChar.ForceTurnEndDebuffHandler.Count != 0)
                    {
                        yield return StartCoroutine(ForceTurnEndDebuffRoutine(curChar));
                        yield return StartCoroutine(TurnEndRoutine(curChar));
                        continue;
                    }
                    yield return StartCoroutine(curChar.AfterBuff());
                    yield return StartCoroutine(curChar.DrawCard());
                    yield return StartCoroutine(curChar.AfterDraw());
                    yield return StartCoroutine(DrawBuffRoutine(curChar));
                    yield return StartCoroutine(DrawDebuffRoutine(curChar));
                    yield return StartCoroutine(curChar.StartTurn());
                    yield return new WaitUntil(() => { return TurnEnd; });
                    yield return StartCoroutine(TurnEndBuffRoutine(curChar));
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
                foreach(var j  in GameManager.Instance.EnemyList)
                {
                    curChar = j;
                    yield return StartCoroutine(curChar.AwakeTurn());
                    yield return StartCoroutine(BuffRoutine(curChar));
                    yield return StartCoroutine(DebuffRoutine(curChar));
                    if (curChar.ForceTurnEndDebuffHandler.Count != 0)
                    {
                        yield return StartCoroutine(TurnEndDebuffRoutine(curChar));
                        yield return StartCoroutine(TurnEndRoutine(curChar));
                        continue;
                    }
                    yield return StartCoroutine(curChar.AfterBuff());
                    yield return StartCoroutine(curChar.DrawCard());
                    yield return StartCoroutine(curChar.AfterDraw());
                    yield return StartCoroutine(DrawBuffRoutine(curChar));
                    yield return StartCoroutine(DrawDebuffRoutine(curChar));
                    yield return StartCoroutine(curChar.StartTurn());
                    yield return StartCoroutine(EnemyRoutine(curChar));
                    yield return StartCoroutine(TurnEndBuffRoutine(curChar));
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
    private IEnumerator EnemyRoutine(Character curChar)
    {
        yield break;
        ///need implement
        ///1. 몬스터는 카드를 사용하는데
        ///2. 아무튼 단기적으로 효율 좋은 순서와 조합으로 카드 사용 (체력이 높거나 플레이어의 체력이 낮으면 공격적으로, 체력이 낮으면 수비적으로 카드 사용)
        ///3. 사용할 때 단기적으로 효율 좋은 위치(공격이라면 최대한 많은 적이 맞으면서 최대한 범위 중심에 가깝게 있도록, 힐이라면 최대한 많은 아군이 맞으면서 최대한 범위 중심에 가깝게 있도록 등)에 사용
        ///
    }
    private IEnumerator TurnAwakeRoutine()
    {
        for (int i = TurnStartRoutine.Count - 1; i < TurnStartRoutine.Count; i--)
        {
            if (!TurnStartRoutine[i].MoveNext())
            {
                while (NeedWait) yield return null;
                TurnStartRoutine.RemoveAt(i);
            }
            while (NeedWait) yield return null;
        }
    }
    private IEnumerator BuffRoutine(Character curChar)
    {
        for (int i = curChar.BuffHandler.Count - 1; i < curChar.BuffHandler.Count; i--)
        {
            if (!curChar.BuffHandler[i].MoveNext())
            {
                while (curChar.NeedWait) yield return null;
                curChar.BuffHandler.RemoveAt(i);
            }
            while (curChar.NeedWait) yield return null;
        }
    }
    private IEnumerator DebuffRoutine(Character curChar)
    {
        for (int i = curChar.DebuffHandler.Count - 1; i < curChar.DebuffHandler.Count; i--)
        {
            if (!curChar.DebuffHandler[i].MoveNext())
            {
                while (curChar.NeedWait) yield return null;
                curChar.DebuffHandler.RemoveAt(i);
            }
            while (curChar.NeedWait) yield return null;
        }
    }
    private IEnumerator DrawBuffRoutine(Character curChar)
    {
        for (int i = curChar.DrawBuffHandler.Count - 1; i < curChar.DrawBuffHandler.Count; i--)
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
        for (int i = curChar.DrawDebuffHandler.Count - 1; i < curChar.DrawDebuffHandler.Count; i--)
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
        for (int i = curChar.ForceTurnEndDebuffHandler.Count - 1; i < curChar.ForceTurnEndDebuffHandler.Count; i--)
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
        for (int i = curChar.TurnEndBuffHandler.Count - 1; i < curChar.TurnEndBuffHandler.Count; i--)
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
        for (int i = curChar.TurnEndDebuffHandler.Count - 1; i < curChar.TurnEndDebuffHandler.Count; i--)
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
            for(int i = curChar.HandCard.Count-1; i>=0; i--)
            {
                yield return StartCoroutine(curChar.DropCard(i));
            }
        }
    }
}
