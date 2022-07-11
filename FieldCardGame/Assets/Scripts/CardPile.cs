using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class CardPile : MonoBehaviour
{
    private float scrollSpeed;
    [SerializeField]
    private Transform panel;
    private float maxThreshold;
    private float softMaxThreshold;
    private float minThreshold;
    private float softMinThreshold;
    private Vector2 originPos;
    private Vector2 padding;
    private Vector2 space;
    [SerializeField]
    private Transform leftTopAnchor;
    [SerializeField]
    private Transform rightTopAnchor;
    [SerializeField]
    private Transform secondRowAnchor;
    private int cardNumperRow;
    private bool scrollLock;
    private List<CardObject> cardObjects = new List<CardObject>();
    public bool Close { get; set; }
    private void Initialize()
    {
        scrollSpeed = 20f;
        originPos = panel.position;
        cardNumperRow = 5;
        padding = leftTopAnchor.position;
        space = new Vector2((rightTopAnchor.position.x - leftTopAnchor.position.x) / cardNumperRow, secondRowAnchor.position.y - leftTopAnchor.position.y);
        minThreshold = originPos.y + space.y;
        softMinThreshold = originPos.y;
    }
    public IEnumerator ShowPile(List<ICard> cardList, bool blindOrder = true)
    {
        gameObject.SetActive(true);
        Initialize();
        maxThreshold = originPos.y - Mathf.Max(((cardList.Count - 1) / 5 - 1), 1) * space.y;
        softMaxThreshold = maxThreshold - Mathf.Max(((cardList.Count - 1) / 5 - 2), 0) * space.y + originPos.y;
        Vector2 nextPos = padding;
        scrollLock = true;
        List<ICard> ordered = new List<ICard>(cardList);
        if (blindOrder)
        {
            ordered = ordered.OrderBy((x) => x.GetCardID()).ToList<ICard>();
        }
        else
        {
            ordered.Reverse();
        }
        for (int i = 0; i < ordered.Count; i++)
        {
            CardObject card = Instantiate(GameManager.Instance.CardObjectDict[ordered[i].GetCardID()], panel);
            cardObjects.Add(card);
            card.isPileCard = true;
            card.transform.position = nextPos;
            nextPos = padding + new Vector2(space.x * ((i + 1) % 5), space.y * ((i + 1) / 5));
        }
        panel.position += Vector3.up * space.y / 3;
        scrollLock = false;
        yield break;
    }
    public IEnumerator ClosePile()
    {
        panel.position = originPos;
        scrollLock = true;
        for (int i = cardObjects.Count - 1; i >= 0; i--)
        {
            Destroy(cardObjects[i].gameObject);
            cardObjects.RemoveAt(i);
        }
        gameObject.SetActive(false);
        PlayerUIManager.Instance.PanelOpenned = false;
        scrollLock = false;
        yield break;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close = true;
        }
        if (Close)
        {
            Close = false;
            StartCoroutine(ClosePile());
        }
        Vector2 movVec = Vector2.zero;
        if (panel.position.y > softMaxThreshold)
        {
            movVec += Vector2.down * Mathf.Exp((-softMaxThreshold + panel.position.y) / 50f);
        }
        else if (panel.position.y < softMinThreshold)
        {
            movVec += Vector2.up * Mathf.Exp((softMinThreshold - panel.position.y) / 50f);
        }
        if (!scrollLock && Input.mouseScrollDelta.x == 0 && Input.mouseScrollDelta.y > 0 && minThreshold <= (panel.position.y))
        {
            movVec -= Vector2.up * Input.mouseScrollDelta.y * scrollSpeed;
        }
        else if (!scrollLock && Input.mouseScrollDelta.x == 0 && Input.mouseScrollDelta.y < 0 && maxThreshold >= (panel.position.y))
        {
            movVec -= Vector2.up * Input.mouseScrollDelta.y * scrollSpeed;
        }
        panel.position += (Vector3)movVec;

    }
}
