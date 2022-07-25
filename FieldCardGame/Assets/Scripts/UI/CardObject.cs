using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class CardObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private CardUsableEffect usableEffect;
    private Transform parent;
    public Vector3 pos;
    public Vector3 position
    {
        get
        {
            return transform.position;
        }
        set
        {
            if (usableEffect)
            {
                usableEffect.transform.position = value;
            }
            pos = transform.position = value;
        }
    }
    private Quaternion rot;
    public Quaternion rotation
    {
        get
        {
            return rot;
        }
        set
        {
            if (usableEffect)
            {
                usableEffect.transform.rotation = value;
            }
            rot = transform.rotation = value;
        }
    }
    public CardUsableEffect UsableEffect
    {
        get
        {
            if (!usableEffect)
            {
                parent = Instantiate(GameManager.Instance.Empty).transform;
                usableEffect = Instantiate(PlayerUIManager.Instance.CardUsableEffect, parent);
                transform.parent = parent;
                parent.parent = PlayerUIManager.Instance.CardArea.transform;
                usableEffect.transform.rotation = transform.rotation;
                usableEffect.transform.position = transform.position;
            }
            return usableEffect;
        }
    }

    [SerializeField]
    private int id;
    public int ID
    {
        get
        {
            return id;
        }
    }
    private TextMeshProUGUI costText;
    private TextMeshProUGUI CostText
    {
        get
        {
            if (!costText)
            {
                costText = transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            }
            return costText;
        }
    }
    private TextMeshProUGUI rangeText;
    private TextMeshProUGUI RangeText
    {
        get
        {
            if (!rangeText)
            {
                rangeText = transform.Find("Range").GetComponent<TextMeshProUGUI>();
            }
            return rangeText;
        }
    }
    private TextMeshProUGUI explainText;
    private TextMeshProUGUI ExplainText
    {
        get
        {
            if (!explainText)
            {
                explainText = transform.Find("ExplainText").GetComponent<TextMeshProUGUI>();
            }
            return explainText;
        }
    }
    public ICard ReferenceCard { get; set; }
    private bool usable;
    public bool Usable
    {
        get
        {
            return usable;
        }
        set
        {
            if(value)
            {
                UsableEffect.gameObject.SetActive(true);
            }
            else
            {
                UsableEffect.gameObject.SetActive(false);
            }
            usable = value;
        }
    }
    public bool isPileCard { get; set; }
    public bool IsRewardCard { get; set; }
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
        Usable = true;
    }
    private void Start()
    {
        image = GetComponent<RawImage>();
    }
    private void Update()
    {
        CostText.text =  $"{ReferenceCard.GetCost()}";
        RangeText.text = $"{ReferenceCard.GetRange()}";
        ExplainText.text = $"{(ReferenceCard as IPlayerCard).ExplainText}";

        if (isPileCard || IsRewardCard)
            return;
        int idx = SiblingIndex - PlayerUIManager.Instance.DefaultSiblingIndex;
        ICard card = GameManager.Instance.CurPlayer.HandCard[idx];
        if (GameManager.Instance.CurPlayer.PayTest(card.GetCost(), card.GetCostType()))
        {
            Usable = true;
        }
        else
        {
            Usable = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (PlayerUIManager.Instance.PanelOpenned)
        {
            transform.localScale = HighlightedCardSize;
            return;
        }
        if (PlayerUIManager.Instance.ReadyUseMode || PlayerUIManager.Instance.UseMode) return;
        MouseEvent.Add(PlayerUIManager.Instance.HighlightCard(this));
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (PlayerUIManager.Instance.PanelOpenned)
        {
            transform.localScale = OriginCardSize;
            return;
        }
        if (PlayerUIManager.Instance.ReadyUseMode || PlayerUIManager.Instance.UseMode) return;
        MouseEvent.Add(PlayerUIManager.Instance.DehighlightCard(this));
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //fixme
        if (!Usable)
        {
            return;
        }
        if (PlayerUIManager.Instance.PanelOpenned)
        {
            if(IsRewardCard)
            {
                StartCoroutine(rewardRoutine());
            }
            else
            {

            }
            return;
        }
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
    private IEnumerator rewardRoutine()
    {
        Usable = false;
        yield return StartCoroutine(GameManager.Instance.CharacterSelected.AddCard(GameManager.Instance.CardDict[ID]));
        PlayerUIManager.Instance.CloseRewardPanel();
    }
    public void SetSiblingIndex(int idx)
    {
        if (parent)
        {
            parent.SetSiblingIndex(idx);
        }
        else
        {
            SetSiblingIndex(idx);
        }
    }
    public void SetAsLastSibling()
    {
        if (parent)
        {
            parent.SetAsLastSibling();
        }
        else
        {
            SetAsLastSibling();
        }
    }
    public void SetActive(bool value)
    {
        if (parent)
        {
            parent.gameObject.SetActive(value);
        }
        else
        {
            gameObject.SetActive(value);
        }
    }
    public void Destroy()
    {
        if (parent)
        {
            Destroy(parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
