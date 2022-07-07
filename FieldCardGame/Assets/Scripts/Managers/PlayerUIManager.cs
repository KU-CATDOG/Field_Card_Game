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
    private Vector2 LeftSideVector;
    private float VectorLen;
    private float angle;
    private float[] evenAngles = new float[10];
    private float[] oddAngles = new float[9];
    private Vector3[] evenVectors = new Vector3[10];
    private Vector3[] oddVectors = new Vector3[9];
    private List<GameObject> CardImages = new List<GameObject>();
    [SerializeField]
    private Transform RightSide;
    private Vector2 RightSideVector;

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
        LeftSideVector = LeftSide.position - CenterPoint.transform.position;
        VectorLen = LeftSideVector.magnitude;
        RightSideVector = RightSide.position - CenterPoint.transform.position;
        angle = Mathf.Acos(Vector2.Dot(LeftSideVector, RightSideVector)/VectorLen/VectorLen);
        float oddAngle;
        float evenAngle;
        oddAngle = angle / 8f;
        evenAngle = angle / 9f;
        for(int i = 0; i < 9; i++)
        {
            oddAngles[i] = (i - 4) * oddAngle;
            oddVectors[i] = new Vector2(Mathf.Tan((i - 4) * oddAngle), 1).normalized * VectorLen;
        }
        for(int i = 0; i < 10; i++)
        {
            evenAngles[i] = (i - 4.5f) * evenAngle;
            evenVectors[i] = new Vector2(Mathf.Tan((i - 4.5f) * evenAngle), 1).normalized * VectorLen;
        }
    }
    public IEnumerator DrawCard()
    {
        List<ICard> cards = GameManager.Instance.Player.HandCard;
        yield return StartCoroutine(Rearrange(cards));
        
    }
    public IEnumerator DropCard()
    {
        List<ICard> cards = GameManager.Instance.Player.HandCard;
        yield return StartCoroutine(Rearrange(cards));
    }
    private IEnumerator Rearrange(List<ICard> cards)
    {
        int size = cards.Capacity;
        if (cards.Capacity % 2 == 0)
        {
            for (int i = 4 - size / 2; i <= 4 + size / 2; i++)
            {
                foreach (GameObject obj in CardImages)
                {
                    obj.transform.position = evenVectors[i] + new Vector3(0,0,-1) * i;
                    obj.transform.rotation = Quaternion.Euler(Vector3.forward * evenAngles[i]);
                }
            }
        }
        else
        {
            for (int i = 5 - size / 2; i <= 4 + size / 2; i++)
            {
                foreach (GameObject obj in CardImages)
                {
                    obj.transform.position = oddVectors[i] + new Vector3(0, 0, -1) * i;
                    obj.transform.rotation = Quaternion.Euler(Vector3.forward * oddAngles[i]);
                }
            }
        }
        yield break;
    }

}
