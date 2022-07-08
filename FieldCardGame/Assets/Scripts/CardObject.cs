using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardObject : MonoBehaviour
{
    public static List<IEnumerator> MouseEvent { get; private set; } = new List<IEnumerator>();
    public int SiblingIndex { get; set; }
    public bool moveInterrupted { get; set; } = false;
    public bool OnMoving { get; set; } = false;
    public static Vector3 highlightedCardSize { get; private set; } = Vector3.zero;
    public static Vector3 originCardSize { get; private set; } = Vector3.zero;
    private RawImage image;
    private void Awake()
    {
        if(originCardSize == Vector3.zero)
        {
            originCardSize = transform.localScale;
            highlightedCardSize = originCardSize * 1.5f;
        }
    }
    private void Start()
    {
        image = GetComponent<RawImage>();
    }
    public void CardMouseEnter()
    {
        if (PlayerUIManager.Instance.ReadyUseMode) return;
        MouseEvent.Add(PlayerUIManager.Instance.HighlightCard(this));
    }
    public void CardMouseExit()
    {
        if (PlayerUIManager.Instance.ReadyUseMode) return;
        MouseEvent.Add(PlayerUIManager.Instance.DehighlightCard(this));
    }
    public void CardOnClick()
    {
        if (PlayerUIManager.Instance.ReadyUseMode)
        {
            MouseEvent.Add(PlayerUIManager.Instance.ReadyUseCardCancel(this));
            
        }
        else
        {
            MouseEvent.Add(PlayerUIManager.Instance.ReadyUseCard(this));
            
        }
    }
}
