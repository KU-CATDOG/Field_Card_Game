using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CardObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static List<IEnumerator> MouseEvent { get; private set; } = new List<IEnumerator>();
    public int SiblingIndex { get; set; }
    public bool MoveInterrupted { get; set; } = false;
    public bool OnMoving { get; set; } = false;
    public static Vector3 HighlightedCardSize { get; private set; } = Vector3.zero;
    public static Vector3 OriginCardSize { get; private set; } = Vector3.zero;
    private RawImage image;
    private void Awake()
    {
        if(OriginCardSize == Vector3.zero)
        {
            OriginCardSize = transform.localScale;
            HighlightedCardSize = OriginCardSize * 1.5f;
        }
    }
    private void Start()
    {
        image = GetComponent<RawImage>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (PlayerUIManager.Instance.ReadyUseMode || PlayerUIManager.Instance.UseMode) return;
        MouseEvent.Add(PlayerUIManager.Instance.HighlightCard(this));
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (PlayerUIManager.Instance.ReadyUseMode || PlayerUIManager.Instance.UseMode) return;
        MouseEvent.Add(PlayerUIManager.Instance.DehighlightCard(this));
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlayerUIManager.Instance.ReadyUseMode && !PlayerUIManager.Instance.UseMode)
        {
            if (Input.mousePosition.y > PlayerUIManager.Instance.CardUseHeight && eventData.button == 0)
            {
                MouseEvent.Add(PlayerUIManager.Instance.UseCard(this));
            }
            else
            {
                MouseEvent.Add(PlayerUIManager.Instance.ReadyUseCardCancel(this));
            }

        }
        else if(!PlayerUIManager.Instance.UseMode)
        {
            if(eventData.button == 0)
                MouseEvent.Add(PlayerUIManager.Instance.ReadyUseCard(this));

        }
    }
}
