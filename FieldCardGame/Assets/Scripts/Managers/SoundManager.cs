using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// fixme
    /// </summary>
    
    private float bgmV;
    public float BGMVolume
    {
        get
        {
            return bgmV;
        }
        set
        {
            bgmV = value;
            BGM.volume = value;
        }
    }
    private float sfxV;
    public float SFXVolume
    {
        get
        {
            return sfxV;
        }
        set
        {
            sfxV = value;
            SFX.volume = value;
        }
    }
    [SerializeField]
    List<AudioClip> bgmClips;
    [SerializeField]
    List<AudioClip> sfxClips;
    [SerializeField]
    AudioSource sfx;
    public AudioSource SFX
    {
        get
        {
            return sfx;
        }
    }
    [SerializeField]
    AudioSource bgm;
    public AudioSource BGM
    {
        get
        {
            return bgm;
        }
    }
    public Dictionary<string, AudioClip> BGMDict = new();
    public Dictionary<string, AudioClip> SFXDict = new();
    public static SoundManager Instance { get; set; }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        BGMDict.Add("Grassland", bgmClips[0]);
        SFXDict.Add(sfxClips[0].name, sfxClips[0]);
        
    }
    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bgm.Stop();
        if (BGMDict.ContainsKey(scene.name))
        {
            bgm.clip = BGMDict[SceneManager.GetActiveScene().name];
            bgm.Play();
            bgm.loop = true;
        }
    }
    private void Update()
    {
        
    }


}
