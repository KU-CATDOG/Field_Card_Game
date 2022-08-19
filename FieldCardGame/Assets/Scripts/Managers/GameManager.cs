using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public LevelUpHandler LvUpHandler;
    public Dictionary<System.Type, LevelUpSkill> BaseSkillDict{ get; } = new();    
    private Dictionary<System.Type, int> CharacterIDDict;
    private List<SpawnEntity> worldEntityList;
    private Dictionary<int, ICard> cardDict = new Dictionary<int, ICard>();
    [SerializeField]
    private CharacterUIManager effectUIPrefab;
    public CharacterUIManager EffectUIPrefab => effectUIPrefab;
    [SerializeField]
    private SkillImage skillImg;
    public SkillImage SkillImage => skillImg;
    [SerializeField]
    private GameObject empty;
    public GameObject Empty
    {
        get
        {
            return empty;
        }
    }
    [SerializeField]
    private Loading dmgEffect;
    public Loading DmgEffect
    {
        get => dmgEffect;
        set => dmgEffect = value;
    }
    [SerializeField]
    private DropCardObject dropCardObject;
    public DropCardObject DropCardObject
    {
        get
        {
            return dropCardObject;
        }
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
    public Character CharacterSelectedPrefab {get; set;}
    private Character characterSelected;
    public Character CharacterSelected
    {
        get=> characterSelected;
        set
        {
            characterSelected = value;
            LvUpHandler = new();
        }
    }
    private Dictionary<int, Enemy> enemyDict = new();
    public IReadOnlyDictionary<int, Enemy> EnemyDict
    {
        get
        {
            return enemyDict;
        }
    }
    //fixme
    private List<GameObject> neutralList = new();
    public List<GameObject> NeutralList => neutralList;
    private Dictionary<int, Enemy> neutralDict = new();
    public IReadOnlyDictionary<int, Enemy> NeutralDict
    {
        get
        {
            return neutralDict;
        }
    }
    public static GameManager Instance { get; set; }
    public List<Character> EnemyList { get; private set; } = new List<Character>();
    public Character CurPlayer { get; set; }
    public List<Character> Allies { get; private set; } = new List<Character>();
    public const int MAPSIZE = 32;
    private GameObject MapObject;
    [SerializeField]
    private Tile tilePrefab;
    public Tile[,] Map = new Tile[MAPSIZE, MAPSIZE];
    //[SerializeField]
    //private int[,] EnemyID;
    //private ArrayForInspector EnemyID = new ArrayForInspector();

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
        InitializeBaseSkillDict();
        InitializeEnemyDict();
        InitializeSpawnEntityList();
        InitializeCharacterIDDict();
    }
    private void InitializeBaseSkillDict()
    {
        BaseSkillDict[typeof(Paladin)] = new PaladinBaseSkill();
    }
    private void InitializeCharacterIDDict()
    {
        CharacterIDDict = new();
        CharacterIDDict.Add(typeof(Paladin), 1);
        CharacterIDDict.Add(typeof(Warlock), 3);
    }
    private void InitializeSpawnEntityList()
    {
        worldEntityList = new(Resources.LoadAll<SpawnEntity>("Prefabs/WorldEntity"));
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
        var cardList = Assembly
          .GetAssembly(typeof(ICard))
          .GetTypes()
          .Where(t => typeof(ICard).IsAssignableFrom(t) && !t.IsInterface);
        if (DEBUGMOD)
        {
            foreach (var i in cardList)
            {
                ICard card = System.Activator.CreateInstance(i) as ICard;
                cardDict.Add(card.GetCardID(), card);
            }
            cardObjectList = Resources.LoadAll<CardObject>("Prefabs/CardObject");
            foreach(var i in cardObjectList)
            {
                cardObjectDict.Add(i.ID, i);
            }
        }
        else
        {
            foreach (var i in cardList)
            {
                ICard card = System.Activator.CreateInstance(i) as ICard;
                if (card.GetCardID() / 1000000 == CharacterIDDict[CharacterSelected.GetType()])
                {
                    cardDict.Add(card.GetCardID(), card);
                }
            }
            cardObjectList = Resources.LoadAll<CardObject>("Prefabs/CardObject");
            foreach (var i in cardObjectList)
            {
                if (i.ID / 1000000 == CharacterIDDict[CharacterSelected.GetType()])
                {
                    cardObjectDict.Add(i.ID, i);
                }
            }
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
        PlayerUIManager.Instance.UseMode = PlayerUIManager.Instance.ReadyUseMode = PlayerUIManager.Instance.OnRoutine = false;
        TurnManager.Instance.Token = 3;
        TurnManager.Instance.TurnEnd = true;
        yield return StartCoroutine(LoadingPanel.StartLoad());
        Destroy(CharacterSelected.gameObject);
        Initialize();
        TurnManager.Instance.Initialize();
        gameClearPanel.SetActive(true);
    }

    public void Initialize()
    {
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
        Tile[] tiles = MapObject.GetComponentsInChildren<Tile>();
        foreach(var i in tiles)
        {
            i.position = new Coordinate((int)i.transform.position.x, (int)i.transform.position.z);
            i.WallMask = i.WallMask;
            Map[i.position.X, i.position.Y] = i;
        }
        /*
        int entityIdx = 0;
        for (int i = 0; i < MAPSIZE; i++)
        {
            for (int j = 0; j < MAPSIZE; j++)
            {
                Tile tile = Instantiate(tilePrefab, MapObject.transform);
                if(i == 15 && j != 15)
                {
                    tile.WallMask |= 0b0100;
                }
                tile.transform.position = new Vector3(i, 0, j);
                tile.position = new Coordinate(i, j);
                Map[i, j] = tile;
                for (int k = 0; k < worldEntityList.Count; k++, entityIdx = (entityIdx+1) % worldEntityList.Count)
                {
                    var entity = worldEntityList[entityIdx];
                    if(Random.Range(0,1f) <= entity.GenerateProbability)
                    {
                        var inst = Instantiate(entity);
                        inst.position = new Coordinate(i, j);
                        break;
                    }

                }
            }
        }
        //fixme*/
        CharacterSelected.gameObject.SetActive(true);
        CharacterSelected.position = new Coordinate(0, 0);
        Map[CharacterSelected.position.X, CharacterSelected.position.Y].CharacterOnTile = CharacterSelected;
        CharacterSelected.SightUpdate(CharacterSelected.Sight);/*
        Map[10, 10].CharacterOnTile = CharacterSelected;
        Character enemy = Instantiate(EnemyDict[90]);
        enemy.position = new Coordinate(15, 15);*/
        StartCoroutine(TurnManager.Instance.TurnRoutine());
    }
    //fixme
    public void GetCardReward(int rewardNum)
    {
        List<ICard> rewardList = new();
        bool[] visited = new bool[cardDict.Count];
        int num = cardDict.Count;
        for(int i=0; i<rewardNum; i++)
        {
            int rand = Random.Range(0, num);
            if (visited[rand])
            {
                i--;
                continue;
            }
            visited[rand] = true;
            rewardList.Add(System.Activator.CreateInstance(cardDict.Values.ElementAt(rand).GetType()) as ICard);
        }
        PlayerUIManager.Instance.OpenRewardPanel(rewardList);
    }
}
