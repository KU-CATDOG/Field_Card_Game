using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Character
{
    public int Level { get; set; } = 1;
    public int Exp { get; set; }
    public int Gold { get; set; } = 0;
    [SerializeField]
    private GameObject playerUI;
    public GameObject PlayerUI
    {
        get
        {
            return playerUI;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.Allies.Add(this);
    }
    protected virtual void Start()
    {
        playerUI = Instantiate(PlayerUI, PlayerUIManager.Instance.PlayerSpecificArea).gameObject;
        PlayerUI.SetActive(false);
    }
}
