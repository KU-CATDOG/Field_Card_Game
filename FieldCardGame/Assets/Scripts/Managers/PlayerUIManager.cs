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
    private RectTransform CardAreaTransform;
    [SerializeField]
    private Transform LeftSide;
    private Vector2 LeftSideVector;
    private float VectorLen;
    private float angle;
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
        VectorLen = leftside
        RightSideVector = RightSide.position - CenterPoint.transform.position;
        Vector2.Dot(LeftSideVector, RightSideVector)/;
    }

}
