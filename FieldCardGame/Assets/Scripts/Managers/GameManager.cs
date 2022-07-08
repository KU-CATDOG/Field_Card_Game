using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<CardObject> cardObjectList;
    public IReadOnlyList<CardObject> CardObjectList
    {
        get
        {
            return cardObjectList.AsReadOnly();
        }
    }
    public static GameManager Instance { get; set; }
    public List<Character> EnemyList { get; private set; }
    public Character Player { get; private set; }
    public List<Character> Allies { get; private set; } = new List<Character>();
    public const int MAPSIZE = 128;
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
        EnemyList = new List<Character>();
    }

    private void Start()
    {
        Player = FindObjectOfType<Character>();
        GenerateMap();
        Allies.Add(Player);
    }

    public Tile GetTilePrefab()
    {
        return tilePrefab;
    }
    public void GenerateMap()
    {
        //fixme
        for(int i = 0; i<MAPSIZE; i++)
        {
            for(int j = 0; j < MAPSIZE; j++)
            {
                Tile tile = Instantiate(tilePrefab, MapObject.transform);
                tile.transform.position = new Vector3(i, 0, j);
                Map[i, j] = tile;
            }
        }
    }
}
