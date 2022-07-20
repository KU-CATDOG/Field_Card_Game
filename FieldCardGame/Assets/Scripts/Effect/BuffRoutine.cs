using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffRoutine : System.IComparable
{
    public int Priority { get; set; }
    public IEnumerator Routine { get; set; }
    public BuffRoutine(IEnumerator routine, int priority)
    {
        Priority = priority;
        Routine = routine;
    }
    public int CompareTo(object a)
    {
        return Priority - (a as BuffRoutine).Priority;
    }
}
