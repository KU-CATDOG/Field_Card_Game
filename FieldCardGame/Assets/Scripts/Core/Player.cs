using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Character
{
    public override int Hp 
    { 
        get => base.Hp;
        set
        {
            if(value < Hp)
            {
                GameManager.Instance.StartCoroutine(DmgEffect());
            }
            hp = value;
        }
    }
    private IEnumerator DmgEffect()
    {
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.DmgEffect.StartLoad(0.3f, 0.08f));
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.DmgEffect.LoadEnd(0.08f));
    }
    private List<IInteractable> interactables = new();
    public IReadOnlyList<IInteractable> Interactables => interactables.AsReadOnly();
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
    [SerializeField]
    private SkillTreePanel skillTreePanel;
    public SkillTreePanel TreePanel => skillTreePanel;
    protected override void Awake()
    {
        base.Awake();
        OnPosChanged += CheckInteraction;
        GameManager.Instance.Allies.Add(this);
    }
    protected override void Start()
    {
        base.Start();
        playerUI = Instantiate(PlayerUI, PlayerUIManager.Instance.PlayerSpecificArea).gameObject;
        PlayerUI.SetActive(false);
        if(skillTreePanel)
            skillTreePanel = Instantiate(skillTreePanel, PlayerUIManager.Instance.GetComponentInChildren<Canvas>().transform);
    }
    public override IEnumerator AwakeTurn()
    {
        CheckInteraction();
        yield break;
    }
    private void CheckInteraction()
    {
        ClearInteractable();
        Coordinate coord;
        if ((coord = position.GetLeftTile()) != null)
        {
            Tile tile = GameManager.Instance.Map[coord.X, coord.Y];
            if (tile.CharacterOnTile is IInteractable)
            {
                AddInteractable(tile.CharacterOnTile as IInteractable);
            }
            foreach (var i in tile.EntityOnTile)
            {
                if (i is IInteractable)
                {
                    AddInteractable(i as IInteractable);
                }
            }
        }
        if ((coord = position.GetRightTile()) != null)
        {
            Tile tile = GameManager.Instance.Map[coord.X, coord.Y];
            if (tile.CharacterOnTile is IInteractable)
            {
                AddInteractable(tile.CharacterOnTile as IInteractable);
            }
            foreach (var i in tile.EntityOnTile)
            {
                if (i is IInteractable)
                {
                    AddInteractable(i as IInteractable);
                }
            }
        }
        if ((coord = position.GetUpTile()) != null)
        {
            Tile tile = GameManager.Instance.Map[coord.X, coord.Y];
            if (tile.CharacterOnTile is IInteractable)
            {
                AddInteractable(tile.CharacterOnTile as IInteractable);
            }
            foreach (var i in tile.EntityOnTile)
            {
                if (i is IInteractable)
                {
                    AddInteractable(i as IInteractable);
                }
            }
        }
        if ((coord = position.GetDownTile()) != null)
        {
            Tile tile = GameManager.Instance.Map[coord.X, coord.Y];
            if (tile.CharacterOnTile is IInteractable)
            {
                AddInteractable(tile.CharacterOnTile as IInteractable);
            }
            foreach (var i in tile.EntityOnTile)
            {
                if (i is IInteractable)
                {
                    AddInteractable(i as IInteractable);
                }
            }
        }
    }

    public IEnumerator GainExp(int exp)
    {
        GainedExp = exp;
        for (int i = TryGainExpRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = TryGainExpRoutine[i];
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
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
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
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
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                TryLevelUpRoutine.RemoveAt(i);
            }
        }
        if (LevelUpInterrupted || IsDie)
        {
            LevelUpInterrupted = false;
            yield break;
        }
        PlayerUIManager.Instance.SkillPanel.ShowReward(GameManager.Instance.LvUpHandler.GetAvailableSkill(3));
        yield return StartCoroutine(levelUp());
        Level++;
        for (int i = LevelUpRoutine.Count - 1; !IsDie && i >= 0; i--)
        {
            IEnumerator routine = LevelUpRoutine[i];
            if (!routine.MoveNext())
            {
                if(routine.Current != null)
                {
                    yield return routine.Current;
                }
                LevelUpRoutine.RemoveAt(i);
            }
        }
        yield break;
    }
    private void AddInteractable(IInteractable interactable)
    {
        if(interactables.Count == 0)
        {
            PlayerUIManager.Instance.InteractButton.gameObject.SetActive(true);
        }
        interactables.Add(interactable);
    }
    private void ClearInteractable()
    {
        PlayerUIManager.Instance.InteractButton.gameObject.SetActive(false);
        interactables.Clear();
    }
    protected abstract IEnumerator levelUp();

}
