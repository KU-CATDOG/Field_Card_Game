using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance { get; set; }
    [SerializeField]
    private Button TurnEndButton;
    [SerializeField]
    private Button CardPile;
    [SerializeField]
    private Button DiscardedPile;
    [SerializeField]
    private GameObject CenterPoint;
    [SerializeField]
    private GameObject CardArea;
    [SerializeField]
    private Transform LeftSide;
    [SerializeField]
    private Transform HighlightedAnchor;
    private Vector2 LeftSideVector;
    private float VectorLen;
    private float angle;
    private float[] evenAngles = new float[10];
    private float[] oddAngles = new float[9];
    private Vector3[] evenVectors = new Vector3[10];
    private Vector3[] oddVectors = new Vector3[9];
    private List<CardObject> CardImages = new List<CardObject>();
    [SerializeField]
    private Transform RightSide;
    private Vector2 RightSideVector;
    private Vector3 centerPos;
    private int defaultSiblingIndex;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        defaultSiblingIndex = CardArea.transform.childCount;
        centerPos = new Vector2(CenterPoint.transform.position.x, -Mathf.Tan(80f / 180f * Mathf.PI) * (RightSide.position - LeftSide.position).magnitude/2);
        CenterPoint.transform.position = centerPos;
        LeftSideVector = LeftSide.position - centerPos;
        VectorLen = LeftSideVector.magnitude;
        RightSideVector = RightSide.position - centerPos;
        angle = Mathf.Acos(Vector2.Dot(LeftSideVector, RightSideVector) / VectorLen / VectorLen);
        float oddAngle;
        float evenAngle;
        oddAngle = angle / 8f;
        evenAngle = angle / 9f;
        for (int i = 0; i < 9; i++)
        {
            oddAngles[i] = (i - 4) * oddAngle;
            oddVectors[i] = new Vector2(Mathf.Tan((i - 4) * oddAngle), 1).normalized * VectorLen;
        }
        for (int i = 0; i < 10; i++)
        {
            evenAngles[i] = (i - 4.5f) * evenAngle;
            evenVectors[i] = new Vector2(Mathf.Tan((i - 4.5f) * evenAngle), 1).normalized * VectorLen;
        }
    }
    public IEnumerator DrawCard()
    {
        CardImages.Add(Instantiate(GameManager.Instance.CardObjectList[GameManager.Instance.Player.addedCard.GetCardID()], CardArea.transform));
        yield return StartCoroutine(Rearrange());

    }
    public IEnumerator DropCard()
    {
        yield return StartCoroutine(Rearrange());
    }
    public IEnumerator Rearrange()
    {
        List<ICard> cards = GameManager.Instance.Player.HandCard;
        int size = cards.Count;
        if (cards.Count % 2 == 0)
        {
            for (int i = 0; i < size; i++)
            {
                CardObject obj = CardImages[i];
                obj.SiblingIndex = defaultSiblingIndex + i;
                StartCoroutine(moveCard(obj, centerPos + evenVectors[5 - size / 2 + i]));
                obj.transform.rotation = Quaternion.Euler(Vector3.back * evenAngles[5 - size / 2 + i] * 70f);
            }
        }
        else
        {
            for (int i = 0; i < size; i++)
            {
                CardObject obj = CardImages[i];
                obj.SiblingIndex = defaultSiblingIndex + i;
                StartCoroutine(moveCard(obj, centerPos + oddVectors[4 - size / 2 + i]));
                obj.transform.rotation = Quaternion.Euler(Vector3.back * oddAngles[4 - size / 2 + i] * 70f);
            }
        }
        yield break;
    }
    public IEnumerator HighlightCard(CardObject card)
    {

        List<ICard> cards = GameManager.Instance.Player.HandCard;
        int size = cards.Count;
        int cardIndex = card.SiblingIndex - defaultSiblingIndex; 
        Vector2 target;
        for (int i = 0; i < size; i++)
        {
            if (i + defaultSiblingIndex == card.SiblingIndex)
            {
                continue;
            }
            float val = Mathf.Abs(i - cardIndex) + 0.2f;
            CardObject obj = CardImages[i];
            Vector2 movVec = new Vector2(Mathf.Log(10000, Mathf.Abs(val)) * ((i - cardIndex < 0) ? -1f : 1f), 0);
            target = (Vector2)centerPos + (cards.Count % 2 == 0 ? (Vector2)evenVectors[5 - size / 2 + i] : (Vector2)oddVectors[4 - size / 2 + i]) + movVec;
            StartCoroutine(moveCard(obj, target));
        }
        card.transform.localScale *= 1.5f;
        target = new Vector2(centerPos.x + (cards.Count % 2 == 0 ? evenVectors[5 - size / 2 + cardIndex] : oddVectors[4 - size / 2 + cardIndex]).x, HighlightedAnchor.position.y);
        StartCoroutine(moveCard(card, target, 0.05f));
        card.transform.SetAsLastSibling();
        card.transform.rotation = Quaternion.Euler(Vector2.zero);
        yield break;
    }
    private int i = 0;
    private IEnumerator moveCard(CardObject card, Vector2 target, float timeLimit = 0.3f)
    {
        int id = i++;
        if (card.SiblingIndex - defaultSiblingIndex == 1)
        {
            Debug.Log("ENTER ID:" + id);
        }
        yield return new WaitUntil(() => {
            if (card.SiblingIndex - defaultSiblingIndex == 1)
            {
                Debug.Log("WAIT ID:" + id);
            }
            card.moveInterrupted = true; return !card.OnMoving; });
        card.moveInterrupted = false;
        if (card.SiblingIndex - defaultSiblingIndex == 1)
        {
            Debug.Log("IN ID:" + id);
        }
        card.OnMoving = true;
        Vector2 movVec = target - (Vector2)card.transform.position;
        float time = 0f;
        movVec /= timeLimit;
        yield return new WaitUntil(() =>
        {
            if (card.SiblingIndex - defaultSiblingIndex == 1)
            {
            Debug.Log("RUNNING ID: " + id);
            }
            if (card.moveInterrupted)
            {
                return true;
            }
            card.transform.position += (Vector3)movVec * Time.deltaTime;
            time += Time.deltaTime;
            return time > timeLimit;
        }); 
        if (card.SiblingIndex - defaultSiblingIndex == 1)
        {
            Debug.Log("OUT ID" + id);
        }
        card.OnMoving = false;
    }
    public IEnumerator DehighlightCard(CardObject card)
    {
        card.transform.localScale /= 1.5f;
        card.transform.SetSiblingIndex(card.SiblingIndex);
        StartCoroutine(Rearrange());
        yield break;
    }
    public void ExecuteCardMouseEvents()
    {
        CardObject.MouseEvent.Reverse();
        for (int i = CardObject.MouseEvent.Count - 1; i >= 0; i--)
        {
            StartCoroutine(CardObject.MouseEvent[i]);
            CardObject.MouseEvent.RemoveAt(i);
        }
    }
    private void Update()
    {
        Debug.Log("UPDATE===");
    }
    private void LateUpdate()
    {
        Debug.Log("LATE UPDATE IN===");
        ExecuteCardMouseEvents();
        Debug.Log("LATE UPDATE OUT===");
    }

}
