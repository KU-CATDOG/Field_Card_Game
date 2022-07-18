using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Dictionary<int, ICard> cardDict = new Dictionary<int, ICard>();
    private bool gameOver;
    public bool GameOver
    {
        get
        {
            return gameOver;
        }
        set
        {
            if (value)
            {
                StartCoroutine(GameOverRoutine());
            }
            gameOver = true;
        }
    }
    public IReadOnlyDictionary<int, ICard> CardDict
    {
        get
        {
            return cardDict;
        }
    }
    private Dictionary<int, CardObject> cardObjectDict = new Dictionary<int, CardObject>();
    public IReadOnlyDictionary<int, CardObject> CardObjectDict
    {
        get
        {
            return cardObjectDict;
        }
    }
    public bool DEBUGMOD;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private Loading loadingPanel;
    public Loading LoadingPanel
    {
        get
        {
            return loadingPanel;
        }
    }
    public Character CharacterSelected { get; set; }
    [SerializeField]
    private List<Enemy> enemyDict;
    public IReadOnlyList<Enemy> EnemyDict
    {
        get
        {
            return enemyDict.AsReadOnly();
        }
    }

    public static GameManager Instance { get; set; }
    public List<Character> EnemyList { get; private set; } = new List<Character>();
    public Character CurPlayer { get; set; }
    public List<Character> Allies { get; private set; } = new List<Character>();
    public const int MAPSIZE = 128;
    [SerializeField]
    private List<CardObject> cardObjectList;
    private GameObject MapObject;
    [SerializeField]
    private Tile tilePrefab;
    public Tile[,] Map = new Tile[MAPSIZE, MAPSIZE];
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
        InitializeDictionary();
        DontDestroyOnLoad(gameObject);
    }
    private void InitializeDictionary()
    {
        ICard card;
        card = new DebugCard();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[0]);

        card = new PaladinMove();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[1]);

        card = new Attack();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[2]);

        card = new WarlockMove();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[3]);

        card = new WarlockSnatch();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[4]);

        card = new WarlockGathering();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[5]);

        card = new WarlockDrain();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[6]);

        card = new PaladinSMA();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[7]);

        card = new PaladinProtect();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[8]);

        card = new PaladinShining();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[9]);

        card = new WarlockSoulBead();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[10]);

        card = new PaladinRevelation1();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[11]);

        card = new PaladinRevelation2();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[12]);

        card = new PaladinRevelation3();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[13]);

        card = new PaladinRevelation4();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[14]);

        card = new PaladinRevelation5();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[15]);

        card = new PaladinRevelation6();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[16]);

        card = new PaladinRevelation7();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[17]);

    }

    private void Start()
    {
        //fixme
        if (DEBUGMOD)
        {
            GameManager.Instance.CharacterSelected = GameManager.Instance.Allies[0];
            GenerateMap();
        }
        //fixme
    }
    private IEnumerator GameOverRoutine()
    { 
        yield return StartCoroutine(LoadingPanel.StartLoad());
        gameOverPanel.SetActive(true);
    }
    public Tile GetTilePrefab()
    {
        return tilePrefab;
    }
    public void GenerateMap()
    {
        MapObject = GameObject.Find("Map");
        for (int i = 0; i < MAPSIZE; i++)
        {
            for (int j = 0; j < MAPSIZE; j++)
            {
                Tile tile = Instantiate(tilePrefab, MapObject.transform);
                tile.transform.position = new Vector3(i, 0, j);
                tile.position = new Coordinate(i, j);
                Map[i, j] = tile;
            }
        }
        //fixme
        CharacterSelected.gameObject.SetActive(true);
        CharacterSelected.position = new Coordinate(10, 10);
        Map[10, 10].CharacterOnTile = CharacterSelected;
        CharacterSelected.SightUpdate(CharacterSelected.Sight);
        Character enemy = Instantiate(EnemyDict[0]);
        enemy.position = new Coordinate(15, 15);
        StartCoroutine(TurnManager.Instance.TurnRoutine());
    }
}
