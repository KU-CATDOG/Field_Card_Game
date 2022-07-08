using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    private void Start()
    {
        
    }
    public static List<IEnumerator> MouseEvent { get; private set; } = new List<IEnumerator>();
    public int SiblingIndex { get; set; }
    public bool moveInterrupted { get; set; } = false;
    public bool OnMoving { get; set; } = false;
    public void CardMouseEnter()
    {
        MouseEvent.Add(PlayerUIManager.Instance.HighlightCard(this));
    }
    public void CardMouseExit()
    {
        MouseEvent.Add(PlayerUIManager.Instance.DehighlightCard(this));
    }
}
