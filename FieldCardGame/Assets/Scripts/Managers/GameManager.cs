using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Dictionary<int, ICard> cardDict = new Dictionary<int, ICard>();
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
    public static GameManager Instance { get; set; }
    public List<Character> EnemyList { get; private set; }
    public Character Player { get; private set; }
    public List<Character> Allies { get; private set; } = new List<Character>();
    public const int MAPSIZE = 128;
    [SerializeField]
    private List<CardObject> cardObjectList;
    [SerializeField]
    private GameObject MapObject;
    [SerializeField]
    private Tile tilePrefab;
    public Tile[,] Map = new Tile[MAPSIZE, MAPSIZE];
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        InitializeDictionary();
        EnemyList = new List<Character>();
        Player = FindObjectOfType<Character>();
        GenerateMap();
        Allies.Add(Player);
    }
    private void InitializeDictionary()
    {
        ICard card;
        card = new PaladinMove();
        cardDict.Add(card.GetCardID(), card);
        cardObjectDict.Add(card.GetCardID(), cardObjectList[1]);
    }

    private void Start()
    {
        //fixme
    }

    public Tile GetTilePrefab()
    {
        return tilePrefab;
    }
    private void GenerateMap()
    {
        //fixme
        for(int i = 0; i<MAPSIZE; i++)
        {
            for(int j = 0; j < MAPSIZE; j++)
            {
                Tile tile = Instantiate(tilePrefab, MapObject.transform);
                tile.transform.position = new Vector3(i, 0, j);
                tile.position = new Coordinate(i, j);
                Map[i, j] = tile;
            }
        }
        //fixme
        Player.position = new Coordinate(10, 10);
        Map[10, 10].CharacterOnTile = Player;
        Player.SightUpdate(Player.Sight);
    }
}
