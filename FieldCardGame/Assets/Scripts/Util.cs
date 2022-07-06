using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Buff
{
    debug,
}
public enum Debuff
{
    debug,
}
public enum BuffType
{

}
public class coordinate
{
    public int X { get; set; }
    public int Y { get; set; }
}
public class Util
{
    public void SyncCoroutine(Character character, IEnumerator coroutine)
    {
        character.NeedWait = true;
        GameManager.Instance.StartCoroutine(coroutine);
        character.NeedWait = false;

    }
}