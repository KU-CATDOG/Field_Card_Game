using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private Dictionary<int, ICard> cardDict = new Dictionary<int, ICard>();
    [SerializeField]
    private Loading dmgEffect;
    public Loading DmgEffect
    {
        get => dmgEffect;
        set => dmgEffect = value;
    }

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
            gameOver = value;
        }
    }
    public bool GameClear { get; set; }
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
    private GameObject gameClearPanel;
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
    private Dictionary<int, Enemy> enemyDict = new();
    public IReadOnlyDictionary<int, Enemy> EnemyDict
    {
        get
        {
            return enemyDict;
        }
    }

    public static GameManager Instance { get; set; }
    public List<Character> EnemyList { get; private set; } = new List<Character>();
    public Character CurPlayer { get; set; }
    public List<Character> Allies { get; private set; } = new List<Character>();
    public const int MAPSIZE = 128;
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
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        InitializeEnemyDict();
    }
    private void InitializeEnemyDict()
    {
        Enemy[] enemyList = new Enemy[0];
        enemyList = Resources.LoadAll<Enemy>("Prefabs/Character/Enemy");
        foreach(var i in enemyList)
        {
            enemyDict.Add(i.ID, i);
        }
    }
    private void InitializeDictionary()
    {
        Debug.Log(CharacterSelected is Paladin);
        CardObject[] cardObjectList = new CardObject[0];
        if(CharacterSelected is Paladin || DEBUGMOD)
        {
           cardObjectList = Resources.LoadAll<CardObject>("Prefabs/CardObject");
        }
        foreach(var i in cardObjectList)
        {
            cardObjectDict.Add(i.ID, i);
        }
        var cardList = Assembly
          .GetAssembly(typeof(ICard))
          .GetTypes()
          .Where(t => typeof(ICard).IsAssignableFrom(t) && !t.IsInterface);
        foreach(var i in cardList)
        {
            ICard card = System.Activator.CreateInstance(i) as ICard;
            cardDict.Add(card.GetCardID(), card);
        }
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
        Initialize();
        TurnManager.Instance.Initialize();
        gameOverPanel.SetActive(true);
    }
    public IEnumerator GameClearRoutine()
    {
        yield return StartCoroutine(LoadingPanel.StartLoad());
        TurnManager.Instance.Initialize();
        Destroy(CharacterSelected.gameObject);
        Initialize();
        gameClearPanel.SetActive(true);
    }

    public void Initialize()
    {
        CharacterSelected = null;
        GameOver = false;
        GameClear = false;
        CharacterSelected = null;
        Allies.Clear();
        EnemyList.Clear();
        cardDict.Clear();
        cardObjectDict.Clear();
    }

    public Tile GetTilePrefab()
    {
        return tilePrefab;
    }
    public void GenerateMap()
    {
        InitializeDictionary();
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
        Character enemy = Instantiate(EnemyDict[1]);
        enemy.position = new Coordinate(15, 15);
        StartCoroutine(TurnManager.Instance.TurnRoutine());
    }
}
