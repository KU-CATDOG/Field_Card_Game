using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance { get; set; }
    public float CardUseHeight { get; set; }
    [SerializeField]
    private Button turnEndButton;
    public Button TurnEndButton
    {
        get
        {
            return TurnEndButton;
        }
    }
    [SerializeField]
    private Transform hpBars;
    public Transform HpBars
    {
        get
        {
            return hpBars;
        }
    }
    [SerializeField]
    private GameObject hpBar;
    public GameObject HpBar
    {
        get
        {
            return hpBar;
        }
    }
    [SerializeField]
    private TextMeshProUGUI LevelText;
    [SerializeField]
    private RectTransform ExpBar;
    private Vector2 ExpBarPos;
    [SerializeField]
    private TextMeshProUGUI HpText;
    [SerializeField]
    private TextMeshProUGUI GoldText;
    [SerializeField]
    private CardPile CardPilePanel;
    [SerializeField]
    private CardPile DiscardedPilePanel;
    [SerializeField]
    private GameObject CenterPoint;
    [SerializeField]
    private GameObject CardArea;
    [SerializeField]
    private Transform LeftSide;
    [SerializeField]
    private Transform HighlightedAnchor;
    [SerializeField]
    private Transform playerSpecificArea;
    public Transform PlayerSpecificArea
    {
        get
        {
            return playerSpecificArea;
        }
    }
    private Vector2 LeftSideVector;
    private float VectorLen;
    public bool OnRoutine { get; set; }
    public bool UseMode { get; set; }
    public ICard UseModeCard;
    public Coordinate CardUsePos { get; set; }
    public bool UseTileSelected { get; set; }
    public bool ReadyUseMode { get; set; }
    public bool PanelOpenned { get; set; }
    private float angle;
    private float[] evenAngles = new float[10];
    private float[] oddAngles = new float[9];
    private Vector3[] evenVectors = new Vector3[10];
    private Vector3[] oddVectors = new Vector3[9];
    public List<CardObject> CardImages { get; private set; } = new List<CardObject>();
    [SerializeField]
    private Transform RightSide;
    private Vector2 RightSideVector;
    private Vector3 centerPos;
    private int defaultSiblingIndex;
    public int DefaultSiblingIndex
    {
        get
        {
            return defaultSiblingIndex;
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        ExpBarPos = ExpBar.position;
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
        CardImages.Add(Instantiate(GameManager.Instance.CardObjectDict[GameManager.Instance.CurPlayer.drawCard.GetCardID()], CardArea.transform));
        CardImages[CardImages.Count - 1].isPileCard = false;
        yield return StartCoroutine(Rearrange());

    }
    public void DropCard(CardObject card)
    {
        int idx = card.SiblingIndex - defaultSiblingIndex;
        CardImages.RemoveAt(idx);
        //needAnimation
        Destroy(card.gameObject);
        StartCoroutine(Rearrange());
    }
    private IEnumerator MoveCard(CardObject card, Vector2 target, float timeLimit = 0.3f)
    {
        bool interrupted = false;
        yield return new WaitUntil(() =>
        {
            card.MoveInterrupted = true;
            if (!card.OnMoving)
            {
                card.OnMoving = true;
                return true;
            }
            return false;
        });
        card.MoveInterrupted = false;
        Vector2 movVec = target - (Vector2)card.transform.position;
        float time = 0f;
        movVec /= timeLimit;
        yield return new WaitUntil(() =>
        {
            if (!card || card.MoveInterrupted)
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
        List<ICard> cards = GameManager.Instance.CurPlayer.HandCard;
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
                StartCoroutine(MoveCard(obj, centerPos + evenVectors[5 - size / 2 + i]));
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
                StartCoroutine(MoveCard(obj, centerPos + oddVectors[4 - size / 2 + i]));
                obj.transform.rotation = Quaternion.Euler(Vector3.back * oddAngles[4 - size / 2 + i] * 70f);
            }
        }
        yield break;
    }
    public IEnumerator HighlightCard(CardObject card)
    {

        List<ICard> cards = GameManager.Instance.CurPlayer.HandCard;
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
            StartCoroutine(MoveCard(obj, target));
        }
        card.transform.localScale = CardObject.HighlightedCardSize;
        target = new Vector2(centerPos.x + (cards.Count % 2 == 0 ? evenVectors[5 - size / 2 + cardIndex] : oddVectors[4 - size / 2 + cardIndex]).x, HighlightedAnchor.position.y);
        StartCoroutine(MoveCard(card, target, 0.05f));
        card.transform.SetAsLastSibling();
        card.transform.rotation = Quaternion.Euler(Vector2.zero);
        yield break;
    }
    public IEnumerator DehighlightCard(CardObject card)
    {
        card.transform.localScale = CardObject.OriginCardSize;
        card.transform.SetSiblingIndex(card.SiblingIndex);
        StartCoroutine(Rearrange());
        yield break;
    }
    public IEnumerator UseCard(CardObject card)
    {
        if (!card.Usable)
        {
            yield break;
        }
        ReadyUseMode = false;
        UseMode = true;
        //StartCoroutine(MainCamera.Instance.moveCamera(true));
        List<ICard> cards = GameManager.Instance.CurPlayer.HandCard;
        int cardIdx = card.SiblingIndex - defaultSiblingIndex;
        UseModeCard = cards[cardIdx];
        int size = cards.Count;
        for (int i = 0; i < size; i++)
        {
            if (i == cardIdx) continue;
            CardObject obj = CardImages[i];
            Vector3 target = obj.transform.position - new Vector3(0, CardUseHeight, 0);
            StartCoroutine(MoveCard(obj, target));
        }
        card.gameObject.SetActive(false);
        int range = UseModeCard.GetRange();
        List<Coordinate> inRange = new List<Coordinate>();
        bool[,] visited = new bool[128, 128];
        bfs(range, GameManager.Instance.CurPlayer.position, inRange);
        foreach (Coordinate i in inRange)
        {
            GameManager.Instance.Map[i.X, i.Y].TileColor.material.color = (UseModeCard as IPlayerCard).GetUnAvailableTileColor();
        }

        foreach (Coordinate i in UseModeCard.GetAvailableTile(GameManager.Instance.CurPlayer.position))
        {
            GameManager.Instance.Map[i.X, i.Y].TileColor.material.color = (UseModeCard as IPlayerCard).GetAvailableTileColor();
        }
        bool UseCancel = false;
        yield return new WaitUntil(() =>
        {
            UseCancel = Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1);
            return UseTileSelected || UseCancel;
        });
        if (UseCancel)
        {
            card.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(DehighlightCard(card));
            UseMode = false;
            foreach (Coordinate i in inRange)
            {
                GameManager.Instance.Map[i.X, i.Y].RestoreColor();
            }
            yield break;
        }
        UseMode = false;
        OnRoutine = true;
        foreach (Coordinate i in inRange)
        {
            GameManager.Instance.Map[i.X, i.Y].RestoreColor();
        }
       // yield return StartCoroutine(MainCamera.Instance.moveCamera(false));
        yield return StartCoroutine(GameManager.Instance.CurPlayer.CardUse(CardUsePos, cardIdx));
        OnRoutine = false;
        UseTileSelected = false;
    }
    private void bfs(int level, Coordinate center, List<Coordinate> list)
    {
        int dist = 1;
        bool[,] visited = new bool[128, 128];
        Queue<Coordinate> queue = new Queue<Coordinate>();
        Queue<Coordinate> nextQueue = new Queue<Coordinate>();
        queue.Enqueue(center);
        while (dist++ <= level)
        {
            while (queue.Count != 0)
            {
                Coordinate tmp = queue.Dequeue();
                list.Add(tmp);
                Coordinate tile;
                if ((tile = tmp.GetDownTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetLeftTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetRightTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                };
                if ((tile = tmp.GetUpTile()) != null && !visited[tile.X, tile.Y])
                {
                    visited[tile.X, tile.Y] = true;
                    nextQueue.Enqueue(tile);
                }
            }
            queue = new Queue<Coordinate>(nextQueue);
            nextQueue.Clear();
        }
        while (queue.Count != 0)
        {
            Coordinate tmp = queue.Dequeue();
            list.Add(tmp);
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
        /*while (ReadyUseMode)
        {
            movVec = (Input.mousePosition - rect.position);
            if (movVec.magnitude < threshold)
            {
                rect.position = Input.mousePosition;
                yield return new WaitForFixedUpdate();
            }
            movVec = movVec.normalized;
            card.transform.position += (Vector3)movVec * Time.deltaTime * speed;
            yield return new WaitForFixedUpdate();
        }*/
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

    public void OpenCardPilePanel()
    {
        PanelOpenned = true;
        StartCoroutine(CardPilePanel.ShowPile(GameManager.Instance.CurPlayer.CardPile));
    }

    public void OpenDiscardedPilePanel()
    {
        PanelOpenned = true;
        StartCoroutine(DiscardedPilePanel.ShowPile(GameManager.Instance.CurPlayer.DiscardedPile, false));
    }

    private void Update()
    {
        if (GameManager.Instance.GameOver || GameManager.Instance.GameClear)
        {
            Destroy(gameObject);
        }
        int lev = (GameManager.Instance.Allies[0] as Player).Level;
        int exp = (GameManager.Instance.Allies[0] as Player).Exp;
        HpText.text = $"Hp {GameManager.Instance.Allies[0].Hp}/{GameManager.Instance.Allies[0].MaxHp}";
        LevelText.text = $"Lv. {lev}";
        ExpBar.sizeDelta = new Vector2( 200 * (float)exp / Mathf.Pow(2, 1 + lev), ExpBar.sizeDelta.y);
        ExpBar.position = ExpBarPos + new Vector2((float)exp / Mathf.Pow(2, 1 + lev) *100, 0);
        GoldText.text = $"Gold {(GameManager.Instance.Allies[0] as Player).Gold}";
    }

    private void LateUpdate()
    {
        ExecuteCardMouseEvents();
    }
}
