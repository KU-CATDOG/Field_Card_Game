using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Enemy
{
    private int crystalCount;
    protected override void Start()
    {
        base.Start();
        Hp = MaxHp = 80;
        GiveExp = 1;
        TurnStartDraw = 4;
        crystalCount = 4;
}
    protected override IEnumerator payCost(int cost, CostType type)
    {
        yield break;
    }
    public override bool PayTest(int cost, CostType type)
    {
        return true;
    }

    public override IEnumerator AwakeTurn()
    {
        yield break;
    }
    public override IEnumerator AfterBuff()
    {
        yield break;
    }
    public override IEnumerator AfterDraw()
    {
        yield break;
    }
    public override IEnumerator StartTurn()
    {
        crystalCount = 4;
        yield break;
    }
    protected override IEnumerator getDmg(int dmg)
    {
        yield break;
    }
    protected override IEnumerator dieRoutine()
    {
        GameManager.Instance.GameClear = true;
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.GameClearRoutine());
    }

    private bool[] calcAtkWheater(int queenAtxIdx, int bishopAtxIdx)
    {
        bool[] res = new bool[] {false, false};
        List<Coordinate> tiles;

        if (queenAtxIdx != -1)
        {
            tiles = HandCard[queenAtxIdx].GetAvailableTile(position);

            foreach (var T in tiles)
            {
                foreach (var j in GameManager.Instance.Allies)
                {
                    if (T.X == j.position.X && T.Y == j.position.Y)
                    {
                        res[0] = true;
                        break;
                    }
                }
                if (res[0])
                    break;
            }
        }
        if (bishopAtxIdx != -1)
        {
            tiles = HandCard[bishopAtxIdx].GetAreaofEffect(position);

            foreach (var T in tiles)
            {
                foreach (var j in GameManager.Instance.Allies)
                {
                    if (T.X == j.position.X && T.Y == j.position.Y)
                    {
                        res[1] = true;
                        break;
                    }
                }
                if (res[1])
                    break;
            }
        }

        return res;

    }

    private List<int> recordCardIdx()
    {
        List<int> a = new List<int> ();

        for (int i = 0; i < 4; i++)
        {
            a.Add(-1);
        }

        for (int i = 0; i < HandCard.Count; i++)
        {
            if (HandCard[i] is QueenAttack)
            {
                a[0] = i;
            }
            else if (HandCard[i] is BishopBomb)
            {
                a[1] = i;
            }
            else if (HandCard[i] is QueenRMove)
            {
                a[2] = i;
            }
            else if (HandCard[i] is QueenBMove)
            {
                a[3] = i;
            }
        }

        return a;
    }

    public override IEnumerator EnemyRoutine()
    {
        List<int> cardIdx;
     
        while (crystalCount > 0)
        {
            cardIdx = recordCardIdx();

            bool[] atkAvilable = calcAtkWheater(cardIdx[0], cardIdx[1]);

            if (atkAvilable[0] && crystalCount >= 3)
            {
                int minDist = int.MaxValue;
                List<Coordinate> tiles = HandCard[cardIdx[0]].GetAvailableTile(position);

                Coordinate toAtk = tiles[0];
                foreach (var i in tiles)
                {
                    foreach (var j in GameManager.Instance.Allies)
                    {
                        if (minDist > Coordinate.Distance(i, j.position))
                        {
                            minDist = Coordinate.Distance(i, j.position);
                            toAtk = i;
                        }
                    }
                }

                crystalCount -= 3;
                yield return StartCoroutine(CardUse(toAtk, cardIdx[0]));
            }
            else if (atkAvilable[1] && crystalCount >= 2)
            {
                int minDist = int.MaxValue;
                List<Coordinate> tiles = HandCard[cardIdx[1]].GetAreaofEffect(position);

                Coordinate toAtk = tiles[0];
                foreach (var i in tiles)
                {
                    foreach (var j in GameManager.Instance.Allies)
                    {
                        if (minDist > Coordinate.Distance(i, j.position))
                        {
                            minDist = Coordinate.Distance(i, j.position);
                            toAtk = i;
                        }
                    }
                }
                crystalCount -= 2;
                yield return StartCoroutine(CardUse(toAtk, cardIdx[1]));
            }
            else
            {
                int[] distance; 
                Coordinate[] toGo = new Coordinate[] { position, position };

                if (crystalCount >= 3)
                {
                    distance = new int[] { int.MaxValue, int.MaxValue };
                    if (cardIdx[2] > -1)
                    {
                        List<Coordinate> tiles = HandCard[cardIdx[2]].GetAvailableTile(position);

                        foreach (var T in tiles)
                        {
                            foreach (var j in GameManager.Instance.Allies)
                            {
                                var dt = Coordinate.Distance(T, j.position);
                                if (distance[0] > dt)
                                {
                                    distance[0] = dt;
                                    toGo[0] = T;
                                }
                            }
                        }
                    }
                    if (cardIdx[3] > -1)
                    {
                        List<Coordinate> tiles = HandCard[cardIdx[3]].GetAvailableTile(position);

                        foreach (var T in tiles)
                        {
                            foreach (var j in GameManager.Instance.Allies)
                            {
                                var dt = Coordinate.Distance(T, j.position);
                                if (distance[1] > dt)
                                {
                                    distance[1] = dt;
                                    toGo[1] = T;
                                }
                            }
                        }
                    }
                    crystalCount -= 1;
                    yield return StartCoroutine(CardUse(toGo[distance[0] < distance[1] ? 0 : 1], cardIdx[distance[0] < distance[1] ? 2 : 3]));
                    Debug.Log(crystalCount);    
                }
                else
                {
                    distance = new int[] { int.MinValue, int.MinValue };
                    if (cardIdx[2] > -1)
                    {
                        List<Coordinate> tiles = HandCard[cardIdx[2]].GetAvailableTile(position);

                        foreach (var T in tiles)
                        {
                            foreach (var j in GameManager.Instance.Allies)
                            {
                                var dt = Coordinate.Distance(T, j.position);
                                if (distance[0] < dt)
                                {
                                    distance[0] = dt;
                                    toGo[0] = T;
                                }
                            }
                        }
                    }
                    if (cardIdx[3] > -1)
                    {
                        List<Coordinate> tiles = HandCard[cardIdx[3]].GetAvailableTile(position);

                        foreach (var T in tiles)
                        {
                            foreach (var j in GameManager.Instance.Allies)
                            {
                                var dt = Coordinate.Distance(T, j.position);
                                if (distance[1] < dt)
                                {
                                    distance[1] = dt;
                                    toGo[1] = T;
                                }
                            }
                        }
                    }
                    crystalCount -= 1;
                    yield return StartCoroutine(CardUse(toGo[distance[0] > distance[1] ? 0 : 1], cardIdx[distance[0] > distance[1] ? 2 : 3]));

                }
            }
        }

    }
    protected override void InitializeDeck()
    {
        CardPile.Add(new QueenBMove());
        CardPile.Add(new QueenRMove());
        CardPile.Add(new QueenAttack());
        CardPile.Add(new BishopBomb());
    }
}
