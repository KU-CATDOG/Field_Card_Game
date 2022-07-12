using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    class i
    {
        public i(int a,int b, int c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }
        public int a;
        public int b;
        public int c;
    }/*
    private void Start()
    {
        List<i> list = new List<i>();
        list.Add(new i(1, 2, 3));
        list.Add(new i(1, 3, 4));
        list.Add(new i(1, 2, 3));
        list.Add(new i(0, 2, 4));
        list.Add(new i(1, 2, 4));
        Debug.Log(list.Remove(new i(1, 2, 3)));
        foreach(var j in list)
            Debug.Log(j.a + " " + j.b + " " + j.c);
    }*/
    /*
    private int t = 0;

    void Start()
    {
    }
    private void Update()
    {
        StartCoroutine(test2());
        if (t++ < 20)
        {
            StartCoroutine(test(t));
        }
        //Debug.Log("UPDATE");
    }
    int frame=0;
    private IEnumerator test2()
    {
        yield return new WaitForEndOfFrame();
        //Debug.Log("FRAME END: " + Time.frameCount);
    }
    private IEnumerator test(int id)
    {
        int i = 0;
        yield return new WaitUntil(() => { Debug.Log(id); return i++ > 10; });
        //Debug.Log(id + "EXIT");
    }*/
}
