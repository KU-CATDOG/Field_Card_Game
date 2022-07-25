using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Material DiscoveredColor;
    [SerializeField]
    private Material DefaultColor;
    private Color OriginColor;
    public Coordinate position { get; set; }
    private bool discovered;
    public bool Discovered
    {
        get
        {
            return discovered;
        }
        set
        {
            if (value)
            {
                TileColor.material = DiscoveredColor;
                discovered = value;
            }
            else
            {
                discovered = value;
            }
        }
    }

    public Character CharacterOnTile { get; set; }
    public List<object> EntityOnTile { get; set; } = new List<object>();
    public MeshRenderer TileColor { get; set; }

    private List<BuffRoutine> onCharacterEnterRoutine = new();
    public IReadOnlyList<BuffRoutine> OnCharacterEnterRoutine
    {
        get
        {
            return onCharacterEnterRoutine.AsReadOnly();
        }
    }
    public void AddOnCharacterEnterRoutine(IEnumerator routine, int priority)
    {
        onCharacterEnterRoutine.Add(new BuffRoutine(routine, priority));
        onCharacterEnterRoutine.Sort();
    }
    public void RemoveOnCharacterEnterRoutineByIdx(int idx)
    {
        onCharacterEnterRoutine.RemoveAt(idx);
    }

    private List<BuffRoutine> onCharacterStayRoutine = new();
    public IReadOnlyList<BuffRoutine> OnCharacterStayRoutine
    {
        get
        {
            return onCharacterStayRoutine.AsReadOnly();
        }
    }
    public void AddOnCharacterStayRoutine(IEnumerator routine, int priority)
    {
        onCharacterStayRoutine.Add(new BuffRoutine(routine, priority));
        onCharacterStayRoutine.Sort();
    }
    public void RemoveOnCharacterStayRoutineByIdx(int idx)
    {
        onCharacterStayRoutine.RemoveAt(idx);
    }

    private List<BuffRoutine> onCharacterExitRoutine = new();
    public IReadOnlyList<BuffRoutine> OnCharacterExitRoutine
    {
        get
        {
            return onCharacterExitRoutine.AsReadOnly();
        }
    }
    public void AddOnCharacterExitRoutine(IEnumerator routine, int priority)
    {
        onCharacterExitRoutine.Add(new BuffRoutine(routine, priority));
        onCharacterExitRoutine.Sort();
    }
    public void RemoveOnCharacterExitRoutineByIdx(int idx)
    {
        onCharacterExitRoutine.RemoveAt(idx);
    }
    //fixme
    public void OnPointerEnter(PointerEventData data)
    {
        if (!PlayerUIManager.Instance.UseMode) return;
        if (!PlayerUIManager.Instance.UseModeCard.IsAvailablePosition(GameManager.Instance.CurPlayer.position, position)) return;
        OriginColor = TileColor.material.color;
        foreach (Coordinate i in PlayerUIManager.Instance.UseModeCard.GetAreaofEffect(position - GameManager.Instance.CurPlayer.position))
        {
            Coordinate target = position + i;
            if (Coordinate.OutRange(target))
              continue;
            GameManager.Instance.Map[target.X, target.Y].OriginColor = GameManager.Instance.Map[target.X, target.Y].TileColor.material.color;
            GameManager.Instance.Map[target.X, target.Y].TileColor.material.color = (PlayerUIManager.Instance.UseModeCard as IPlayerCard).GetColorOfEffect(i);

            if (PlayerUIManager.Instance.UseModeCard is IPlayerConditionCard)
            {
                if((PlayerUIManager.Instance.UseModeCard as IPlayerConditionCard).isSatisfied(target))
                    GameManager.Instance.Map[target.X, target.Y].TileColor.material.color = (PlayerUIManager.Instance.UseModeCard as IPlayerConditionCard).SatisfiedAreaColor();
            }
        }
    }
    public void OnPointerExit(PointerEventData data)
    {
        if (!PlayerUIManager.Instance.UseMode) return;
        if (!PlayerUIManager.Instance.UseModeCard.IsAvailablePosition(GameManager.Instance.CurPlayer.position, position)) return;
        foreach (Coordinate i in PlayerUIManager.Instance.UseModeCard.GetAreaofEffect(position - GameManager.Instance.CurPlayer.position))
        {
            Coordinate target = position + i;
            if (Coordinate.OutRange(target))
              continue;
            GameManager.Instance.Map[target.X, target.Y].TileColor.material.color = GameManager.Instance.Map[target.X,target.Y].OriginColor;
        }
    }
    public void OnPointerClick(PointerEventData data)
    {
        if (!PlayerUIManager.Instance.UseMode) return;
        if (data.button != 0) return;
        if (!PlayerUIManager.Instance.UseModeCard.IsAvailablePosition(GameManager.Instance.CurPlayer.position, position)) return;
        PlayerUIManager.Instance.UseTileSelected = true;
        PlayerUIManager.Instance.CardUsePos = position;
        foreach (Coordinate i in PlayerUIManager.Instance.UseModeCard.GetAreaofEffect(position - GameManager.Instance.CurPlayer.position))
        {
            Coordinate target = position + i;
            if (Coordinate.OutRange(target))
                continue;
            GameManager.Instance.Map[target.X, target.Y].RestoreColor();
        }

    }

    public List<int> RoutineID { get; private set; }
    public List<IEnumerator> TileRoutine { get; private set; }
    private void Awake()
    {
        TileColor = GetComponent<MeshRenderer>();
        TileColor.material = DefaultColor;
    }
    public void RestoreColor()
    {
        if (Discovered)
        {
            TileColor.material = DiscoveredColor;
        }
        else
        {
            TileColor.material = DefaultColor;
        }
    }
}
