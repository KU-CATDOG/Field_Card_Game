using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffRoutine : IComparer
{
    public int Priority { get; set; }
    public IEnumerator Routine { get; set; }
    public BuffRoutine(IEnumerator routine, int priority)
    {
        Priority = priority;
        Routine = routine;
    }
    public int Compare(object a, object b)
    {
        return (a as BuffRoutine).Priority - (b as BuffRoutine).Priority;
    }

}
