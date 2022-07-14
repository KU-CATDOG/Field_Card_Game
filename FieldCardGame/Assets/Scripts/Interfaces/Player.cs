using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Character
{
    public int Level { get; set; } = 1;
    public int Exp { get; set; }
    public int Gold { get; set; } = 0;
    public bool GainExpInterrupted { get; set; }
    public int GainedExp { get; set; }
    public List<IEnumerator> TryGainExpRoutine { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> GainExpRoutine { get; private set; } = new List<IEnumerator>();


    public bool LevelUpInterrupted { get; set; }
    public List<IEnumerator> TryLevelUpRoutine { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> LevelUpRoutine { get; private set; } = new List<IEnumerator>();
    [SerializeField]
    private GameObject playerUI;
    public GameObject PlayerUI
    {
        get
        {
            return playerUI;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.Allies.Add(this);
    }
    protected override void Start()
    {
        base.Start();
        playerUI = Instantiate(PlayerUI, PlayerUIManager.Instance.PlayerSpecificArea).gameObject;
        PlayerUI.SetActive(false);
    }
    public IEnumerator GainExp(int exp)
    {
        GainedExp = exp;
        for (int i = TryGainExpRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = TryGainExpRoutine[i];
            while (NeedWait != 0) yield return null;
            if (!routine.MoveNext())
            {
                TryGainExpRoutine.RemoveAt(i);
            }
        }
        if (GainExpInterrupted || IsDie)
        {
            GainExpInterrupted = false;
            yield break;
        }

        Exp += GainedExp;
        if (Exp >= Mathf.Pow(2, Level + 1))
        {
            Exp -= (int)Mathf.Pow(2, Level + 1);
            yield return StartCoroutine(LevelUp());
        }

        for (int i = GainExpRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = GainExpRoutine[i];
            while (NeedWait != 0) yield return null;
            if (!routine.MoveNext())
            {
                GainExpRoutine.RemoveAt(i);
            }
        }
        yield break;
    }
    public IEnumerator LevelUp()
    {
        for (int i = TryLevelUpRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = TryLevelUpRoutine[i];
            while (NeedWait != 0) yield return null;
            if (!routine.MoveNext())
            {
                TryLevelUpRoutine.RemoveAt(i);
            }
        }
        if (LevelUpInterrupted || IsDie)
        {
            LevelUpInterrupted = false;
            yield break;
        }
        yield return StartCoroutine(levelUp());
        Level++;
        for (int i = GainExpRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = GainExpRoutine[i];
            while (NeedWait != 0) yield return null;
            if (!routine.MoveNext())
            {
                GainExpRoutine.RemoveAt(i);
            }
        }
        yield break;
    }
    protected abstract IEnumerator levelUp();

}
