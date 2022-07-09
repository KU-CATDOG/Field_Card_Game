using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Material DiscoveredColor;
    [SerializeField]
    private Material OnSightColor;
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
            if (value == true && !Onsight)
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
    private bool onSight;
    public bool Onsight
    {
        get
        {
            return onSight;
        }
        set
        {
            if (value == true)
            {
                TileColor.material = OnSightColor;
            }
            else
            {
                TileColor.material = DiscoveredColor;
            }
            onSight = value;
        }
    }
    public Character CharacterOnTile { get; set; }
    public List<object> EntityOnTile { get; set; } = new List<object>();
    public MeshRenderer TileColor { get; set; }
    public List<IEnumerator> OnCharacterEnterRoutine { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> OnCharacterStayRoutine { get; private set; } = new List<IEnumerator>();
    public List<IEnumerator> OnCharacterExitRoutine { get; private set; } = new List<IEnumerator>();

    //fixme
    public void OnPointerEnter(PointerEventData data)
    {
        if (!PlayerUIManager.Instance.UseMode) return;
        if (!PlayerUIManager.Instance.UseModeCard.IsAvailablePosition(GameManager.Instance.Player.position, position)) return;
        OriginColor = TileColor.material.color;
        foreach (Coordinate i in PlayerUIManager.Instance.UseModeCard.GetAreaofEffect())
        {
            Coordinate target = position + i;
            GameManager.Instance.Map[target.X, target.Y].TileColor.material.color = (PlayerUIManager.Instance.UseModeCard as IPlayerCard).GetColorOfEffect(i);
        }
    }
    public void OnPointerExit(PointerEventData data)
    {
        if (!PlayerUIManager.Instance.UseMode) return;
        if (!PlayerUIManager.Instance.UseModeCard.IsAvailablePosition(GameManager.Instance.Player.position, position)) return;
        foreach (Coordinate i in PlayerUIManager.Instance.UseModeCard.GetAreaofEffect())
        {
            Coordinate target = position + i;
            GameManager.Instance.Map[target.X, target.Y].TileColor.material.color = OriginColor;
        }
    }
    public void OnPointerClick(PointerEventData data)
    {
        Debug.Log("CLICK " + Time.frameCount);
        if (!PlayerUIManager.Instance.UseMode) return;
        if (data.button != 0) return;
        if (!PlayerUIManager.Instance.UseModeCard.IsAvailablePosition(GameManager.Instance.Player.position, position)) return;
        PlayerUIManager.Instance.UseTileSelected = true;
        PlayerUIManager.Instance.CardUsePos = position;
        foreach (Coordinate i in PlayerUIManager.Instance.UseModeCard.GetAreaofEffect())
        {
            Coordinate target = GameManager.Instance.Player.position + i;
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
        if (Onsight)
        {
            TileColor.material = OnSightColor;
        }
        else if (Discovered)
        {
            TileColor.material = DiscoveredColor;
        }
        else
        {
            TileColor.material = DefaultColor;
        }
    }
}
