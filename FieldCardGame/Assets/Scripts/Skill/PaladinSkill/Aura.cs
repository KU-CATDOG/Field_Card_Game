using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : LevelUpSkill
{
    public static bool Maximization { get; set; }
    public static bool Stigma { get; set; } = false;
    public static int Range { get; set; } = 1;
    public static int Dmg { get; set; } = 5;
    private int count = 0;
    public override int Count
    {
        get=>count;
        set=>count = value;
    }
    public override int ID => 1;
    protected override void levelUpRoutine()
    {
        GameManager.Instance.CharacterSelected.AddTurnEndBuff(_Aura(), 0);
        return;
    }
    private IEnumerator _Aura()
    {
        while (true)
        {
            yield return GameManager.Instance.StartCoroutine(bfs());
        }
    }
    private IEnumerator bfs()
    {
        List<Coordinate> ret = new();
        int level = 1;
        bool[,] visited = new bool[128, 128];
        Queue<Coordinate> queue = new Queue<Coordinate>();
        Queue<Coordinate> nextQueue = new Queue<Coordinate>();
        queue.Enqueue(GameManager.Instance.CharacterSelected.position);
        while (level++ <= Range)
        {
            while (queue.Count != 0)
            {
                Coordinate tmp = queue.Dequeue();
                Coordinate tile;
                ret.Add(tmp);
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
            ret.Add(tmp);
        }
        Color colorVec = new Color(169 / 255f, 0 / 255f, 255 / 255f) - GameManager.Instance.Map[0, 0].TileColor.material.color;
        float time = 0f;
        while (time < 0.3f)
        {
            time += Time.deltaTime;
            foreach (var i in ret)
            {
                GameManager.Instance.Map[i.X, i.Y].TileColor.material.color += colorVec * Time.deltaTime / 0.3f;
            }
            yield return null;
        }
        time = 0f;
        while (time < 0.3f)
        {
            time += Time.deltaTime;
            foreach (var i in ret)
            {
                GameManager.Instance.Map[i.X, i.Y].TileColor.material.color -= colorVec * Time.deltaTime / 0.3f;
            }
            yield return null;
        }
        foreach (var i in ret)
        {
            GameManager.Instance.Map[i.X, i.Y].RestoreColor();
        }
        foreach (var i in ret)
        {
            if (GameManager.Instance.Map[i.X, i.Y].CharacterOnTile is Enemy)
            {
                if(Maximization && Coordinate.Distance(i, GameManager.Instance.CharacterSelected.position) <= 3)
                {
                    yield return GameManager.Instance.StartCoroutine(GameManager.Instance.CharacterSelected.HitAttack(GameManager.Instance.Map[i.X, i.Y].CharacterOnTile, Dmg*2));
                }
                else
                {
                    yield return GameManager.Instance.StartCoroutine(GameManager.Instance.CharacterSelected.HitAttack(GameManager.Instance.Map[i.X, i.Y].CharacterOnTile, Dmg));
                }
                if (Stigma)
                {
                    if(GameManager.Instance.Map[i.X, i.Y].CharacterOnTile)
                        GameManager.Instance.Map[i.X, i.Y].CharacterOnTile.EffectHandler.DebuffDict[DebuffType.DivineStigma].SetEffect(1);
                }
            }
        }
    }
    public override string GetText()
    {
        return "AURA\n 턴 종료 마다 1칸 범위의 적에게 5의 데미지를 주는 오오라를 펼칩니다.";
    }
    List<LevelUpSkill> nextSkillList;
    public override List<LevelUpSkill> GetNextSkillList()
    {
        if(nextSkillList == null)
        {
            nextSkillList = new();
            nextSkillList.Add(new AuraDmgUp());
            nextSkillList.Add(new AuraRangeUp());
            Sanctuary tmp = new();
            if (GameManager.Instance.LvUpHandler.SkillDict.ContainsKey(tmp.ID))
            {
                nextSkillList.Add(GameManager.Instance.LvUpHandler.SkillDict[tmp.ID]);

            }
            else
            {
                nextSkillList.Add(tmp);
            }
        }
        return nextSkillList;
    }

}
