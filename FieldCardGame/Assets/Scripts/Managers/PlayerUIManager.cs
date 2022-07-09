using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance { get; set; }
    public float CardUseHeight { get; set; }
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
    public bool TileSelected { get; set; }
    public bool UseMode { get; set; }
    public ICard UseModeCard;
    public coordinate CardUsePos { get; set; }
    public bool UseTileSelected { get; set; }
    public bool ReadyUseMode { get; set; }

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
        RectTransform rect = CardArea.GetComponent<RectTransform>();
        CardUseHeight = rect.position.y + rect.sizeDelta.y;
        defaultSiblingIndex = CardArea.transform.childCount;
        centerPos = new Vector2(CenterPoint.transform.position.x, -Mathf.Tan(80f / 180f * Mathf.PI) * (RightSide.position - LeftSide.position).magnitude / 2);
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
        CardImages.Add(Instantiate(GameManager.Instance.CardObjectList[GameManager.Instance.Player.drawCard.GetCardID()], CardArea.transform));
        yield return StartCoroutine(Rearrange());

    }
    public IEnumerator DropCard(CardObject card)
    {
        int idx = card.SiblingIndex; 
        CardImages.RemoveAt(idx);
        //needAnimation
        Destroy(CardImages[idx].gameObject);
        yield return StartCoroutine(Rearrange());
    }
    private IEnumerator moveCard(CardObject card, Vector2 target, float timeLimit = 0.3f)
    {
        bool interrupted = false;
        yield return new WaitUntil(() => {
            card.moveInterrupted = true;
            if (!card.OnMoving)
            {
                card.OnMoving = true;
                return true;
            }
            return false;
        });
        card.moveInterrupted = false;
        Vector2 movVec = target - (Vector2)card.transform.position;
        float time = 0f;
        movVec /= timeLimit;
        yield return new WaitUntil(() =>
        {
            if (card.moveInterrupted)
            {
                interrupted = true;
                return true;
            }
            card.transform.position += (Vector3)movVec * Time.deltaTime;
            time += Time.deltaTime;
            return time > timeLimit;
        });
        if (!interrupted)
        {
            card.transform.position = target;
        }
        card.OnMoving = false;
    }
    public IEnumerator Rearrange(int exceptFor = -1)
    {
        List<ICard> cards = GameManager.Instance.Player.HandCard;
        int size = cards.Count;
        if (cards.Count % 2 == 0)
        {
            for (int i = 0; i < size; i++)
            {
                if (exceptFor == i)
                {
                    continue;
                }
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
                if (exceptFor == i)
                {
                    continue;
                }
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
        card.transform.localScale = CardObject.highlightedCardSize;
        target = new Vector2(centerPos.x + (cards.Count % 2 == 0 ? evenVectors[5 - size / 2 + cardIndex] : oddVectors[4 - size / 2 + cardIndex]).x, HighlightedAnchor.position.y);
        StartCoroutine(moveCard(card, target, 0.05f));
        card.transform.SetAsLastSibling();
        card.transform.rotation = Quaternion.Euler(Vector2.zero);
        yield break;
    }
    public IEnumerator DehighlightCard(CardObject card)
    {
        card.transform.localScale = CardObject.originCardSize;
        card.transform.SetSiblingIndex(card.SiblingIndex);
        StartCoroutine(Rearrange());
        yield break;
    }
    public IEnumerator UseCard(CardObject card)
    {
        ReadyUseMode = false;
        UseMode = true;
        StartCoroutine(MainCamera.Instance.moveCamera(true));
        List<ICard> cards = GameManager.Instance.Player.HandCard;
        int cardIdx = card.SiblingIndex - defaultSiblingIndex;
        UseModeCard = cards[cardIdx];
        int size = cards.Count;
        for (int i = 0; i < size; i++)
        {
            if (i == cardIdx) continue;
            CardObject obj = CardImages[i];
            Vector3 target = obj.transform.position - new Vector3(0, CardUseHeight, 0);
            StartCoroutine(moveCard(obj, target));
        }
        card.gameObject.SetActive(false);
        int range = UseModeCard.GetRange();
        List<coordinate> inRange = new List<coordinate>();
        dfs(0, range, GameManager.Instance.Player.position, inRange);
        foreach(coordinate i in inRange)
        {
            GameManager.Instance.Map[i.X, i.Y].TileColor.material.color = UseModeCard.GetUnAvailableTileColor();
        }
        foreach (coordinate i in UseModeCard.GetAvailableTile(GameManager.Instance.Player.position))
        {
            GameManager.Instance.Map[i.X, i.Y].TileColor.material.color = UseModeCard.GetAvailableTileColor();
        }
        yield return new WaitUntil(() =>
        {
            return UseTileSelected;
        });
        foreach(coordinate i in inRange)
        {
            GameManager.Instance.Map[i.X, i.Y].RestoreColor();
        }
        UseMode = false;
        yield return StartCoroutine(MainCamera.Instance.moveCamera(false));
        yield return StartCoroutine(GameManager.Instance.Player.CardUse(CardUsePos, cardIdx));
    }
    private void dfs(int level, int limit, coordinate now, List<coordinate> inRange)
    {
        if (level > limit)
            return;
        inRange.Add(now);
        if (now.GetDownTile() != null)
        {
            dfs(level + 1, limit, now.GetDownTile(), inRange);
        }
        if (now.GetUpTile() != null)
        {
            dfs(level + 1, limit, now.GetUpTile(), inRange);

        }
        if (now.GetLeftTile() != null)
        {
            dfs(level + 1, limit, now.GetLeftTile(), inRange);

        }
        if (now.GetRightTile() != null)
        {
            dfs(level + 1, limit, now.GetRightTile(), inRange);
        }
    }
    /* implement if need
    public IEnumerator UseCardCancel(CardObject card)
    {

    }
    */
    public IEnumerator ReadyUseCard(CardObject card)
    {
        StartCoroutine(HighlightCard(card));
        ReadyUseMode = true;
        RectTransform rect = card.GetComponent<RectTransform>();
        float speed = 10000f;
        float threshold = 1000f;
        Vector2 movVec;
        yield return new WaitUntil(() =>
        {
            movVec = (Input.mousePosition - rect.position);
            if (movVec.magnitude < threshold)
            {
                rect.position = Input.mousePosition;
                return !ReadyUseMode;
            }
            movVec = movVec.normalized;
            card.transform.position += (Vector3)movVec * Time.deltaTime * speed;
            return !ReadyUseMode;
        });
    }
    public IEnumerator ReadyUseCardCancel(CardObject card)
    {
        StartCoroutine(DehighlightCard(card));
        ReadyUseMode = false;
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
    private void LateUpdate()
    {
        ExecuteCardMouseEvents();
    }

}
