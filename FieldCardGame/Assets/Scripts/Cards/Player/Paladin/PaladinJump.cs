using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinJump : IPlayerCard, IAttackCard
{
    public List<int> Damage
    {
        get
        {
            List<int> ret = new();
            ret.Add(damage);
            return ret;
        }
    }
    public void SetDmg(int value)
    {
        SetDamage(Mathf.Max(0, GetDamage() + value));

        Damage[0] = GetDamage();
    }
    private int range = 3;
    private int cost = 1;
    private int damage = 14;
    private bool interrupted;
    public bool Disposable { get; set; }
    public string ExplainText
    {
        get
        {
            return $"해당 위치로 점프하여 이동, 1칸 거리의 적들에게 {damage}의 피해를 준다.";
        }
    }
    public IEnumerator GetCardRoutine(Character owner)
    {
        yield break;
    }
    public IEnumerator RemoveCardRoutine(Character owner)
    {
        yield break;
    }
    public int GetRange()
    {
        return range;
    }
    public void SetRange(int _range)
    {
        range = _range;
    }
    public int GetDamage()
    {
        return damage;
    }
    public void SetDamage(int _damage)
    {
        damage = _damage;
    }
    public Color GetUnAvailableTileColor()
    {
        return Color.red;
    }
    public List<Coordinate> GetAvailableTile(Coordinate pos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        int level = 1;
        bool[,] visited = new bool[128, 128];
        Queue<Coordinate> queue = new Queue<Coordinate>();
        Queue<string> queueDir = new Queue<string>();
        Queue<Coordinate> nextQueue = new Queue<Coordinate>();
        Queue<string> nextQueueDir = new Queue<string>();
        queue.Enqueue(pos);
        queueDir.Enqueue("pos");
        while (level++ <= GetRange())
        {
            while (queue.Count != 0)
            {
                Coordinate tmp = queue.Dequeue();
                string tmpDir = queueDir.Dequeue();
                if (tmp.X != pos.X || tmp.Y != pos.Y)
                {
                    if (!GameManager.Instance.Map[tmp.X, tmp.Y].CharacterOnTile)
                        ret.Add(tmp);
                }
                Coordinate tile;
                if ((tile = tmp.GetDownTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    if (tmpDir == "pos" || tmpDir == "down") {
                        nextQueue.Enqueue(tile);
                        nextQueueDir.Enqueue("down");
                    }
                };
                if ((tile = tmp.GetLeftTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    if (tmpDir == "pos" || tmpDir == "left") {
                        nextQueue.Enqueue(tile);
                        nextQueueDir.Enqueue("left");
                    }
                };
                if ((tile = tmp.GetRightTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    if (tmpDir == "pos" || tmpDir == "right") {
                        nextQueue.Enqueue(tile);
                        nextQueueDir.Enqueue("right");
                    }
                };
                if ((tile = tmp.GetUpTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    if (tmpDir == "pos" || tmpDir == "up") {
                        nextQueue.Enqueue(tile);
                        nextQueueDir.Enqueue("up");
                    }
                }
            }
            queue = new Queue<Coordinate>(nextQueue);
            queueDir = new Queue<string>(nextQueueDir);
            nextQueue.Clear();
            nextQueueDir.Clear();
        }
        while (queue.Count != 0)
        {
            Coordinate tile = queue.Dequeue();
            if (!GameManager.Instance.Map[tile.X, tile.Y].CharacterOnTile)
                ret.Add(tile);
        }
        return ret;
    }
    public Color GetAvailableTileColor()
    {
        return Color.blue;
    }
    private List<Coordinate> GetDamageArea(Coordinate pos)
    {
        List<Coordinate> ret = new List<Coordinate>();

        ret.Add(pos.GetUpTilewithoutTest());
        ret.Add(pos.GetDownTilewithoutTest());
        ret.Add(pos.GetLeftTilewithoutTest());
        ret.Add(pos.GetRightTilewithoutTest());
        return ret;
    }
    public List<Coordinate> GetAreaofEffect(Coordinate relativePos)
    {
        List<Coordinate> ret = new List<Coordinate>();
        List<Coordinate> dmgArea = new List<Coordinate>();

        ret.Add(new Coordinate(0, 0));
        
        dmgArea = GetDamageArea(new Coordinate(0, 0));
        for (int i = 0; i < dmgArea.Count; i++)
        {
            ret.Add(dmgArea[i]);
        }
        
        return ret;
    }
    public Color GetColorOfEffect(Coordinate pos)
    {
        if (pos.X == 0 && pos.Y == 0)
        {
            return Color.white;
        }
        return Color.black;
    }
    public bool IsAvailablePosition(Coordinate caster, Coordinate target)
    {
        List<Coordinate> availablePositions = GetAvailableTile(caster);
        if (availablePositions.Exists((i) => i.X == target.X && i.Y == target.Y))
        {
            return true;
        }
        return false;
    }
    private IEnumerator JumpRoutine(Character caster, Coordinate target, float height)
    {
        float distance = Coordinate.EuclideanDist(caster.position, target);
        float slope = 4 * (height - caster.transform.position.y) / distance / distance;
        while(caster.position != target && !caster.MoveInterrupted)
        {
            float x = Mathf.Sqrt(Mathf.Pow(target.X - caster.transform.position.x, 2) + Mathf.Pow(target.Y - caster.transform.position.z, 2));
            caster.transform.position = new Vector3(caster.transform.position.x, height - slope * (x - distance/2) * (x - distance/2),caster.transform.position.z);
            yield return new WaitForFixedUpdate();
        }
    }
    public IEnumerator CardRoutine(Character caster, Coordinate target)
    {
        List<Coordinate> attackArea = new List<Coordinate>();
        List<Coordinate> damageArea = new List<Coordinate>();

        Coordinate pos;
        Character enemy;

        if (interrupted)
        {
             interrupted = false;
             yield break;
        }
        float tmp = caster.transform.position.y;
        caster.StartCoroutine(JumpRoutine(caster, target, 3));
        yield return caster.StartCoroutine(caster.Move(target, 5f));
        caster.transform.position = new Vector3(caster.transform.position.x, tmp, caster.transform.position.z);
        attackArea = GetDamageArea(target);
        for (int i = 0; i<attackArea.Count;i++)
        {
            pos = attackArea[i];
            if (Coordinate.OutRange(pos))
            {
                continue;
            }
            if (interrupted)
            {
                interrupted = false;
                yield break;
            }
            enemy = GameManager.Instance.Map[pos.X, pos.Y].CharacterOnTile;
            if (enemy is Enemy)
            {
                damageArea.Add(pos);
            }
        }
        if (damageArea.Count == 0)
            yield break;
        for (int i = 0; i < damageArea.Count - 1; i++)
        {
            pos = damageArea[i];
            enemy = GameManager.Instance.Map[pos.X, pos.Y].CharacterOnTile;
            GameManager.Instance.StartCoroutine(caster.HitAttack(enemy, GetDamage()));
        }
        pos = damageArea[damageArea.Count - 1];
        enemy = GameManager.Instance.Map[pos.X, pos.Y].CharacterOnTile;
        yield return GameManager.Instance.StartCoroutine(caster.HitAttack(enemy, GetDamage()));
    }
    public void CardRoutineInterrupt()
    {
        interrupted = true;
    }
    public int GetCost()
    {
        return cost;
    }
    public void SetCost(int _cost)
    {
        cost = _cost;
    }
    public CostType GetCostType()
    {
        return CostType.PaladinEnergy;
    }
    public CardType GetCardType()
    {
        return CardType.Move;
    }
    public int GetCardID()
    {
        return 1019110;
    }
}
