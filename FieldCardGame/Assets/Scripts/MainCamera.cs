using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MainCamera : MonoBehaviour, IScrollHandler
{
    public static MainCamera Instance { get; private set; }
    private Vector3 posVec;
    private Vector3 useModePos;
    public bool OnMoving { get; private set; } = false;
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
    }
    void Start()
    {
        posVec = transform.position - GameManager.Instance.Player.transform.position;
        useModePos = posVec + Vector3.up * 4;
        
    }
    public void OnScroll(PointerEventData data)
    {/*
        if (data.scrollDelta > 0)
        {

        }
        else
        {

        }*/
    }
    // Update is called once per frame
    void Update()
    {
        if (!PlayerUIManager.Instance.UseMode)
        {
            transform.position = GameManager.Instance.Player.transform.position + posVec;
            transform.LookAt(GameManager.Instance.Player.transform);
        }
        
    }
    
    /*
    public IEnumerator moveCamera(bool useMode)
    {
        OnMoving = true;
        float timeLimit = 0.2f;
        float time = 0f;
        float threshold = 0.1f;
        Vector3 moveVec;
        Vector3 target;
        if (useMode)
        {
            target = useModePos + GameManager.Instance.Player.transform.position;
            moveVec = useModePos - posVec;
            moveVec /= timeLimit;
         
            yield return new WaitUntil(() =>
            {
                time += Time.deltaTime;
                transform.position += moveVec * Time.deltaTime;
                transform.LookAt(GameManager.Instance.Player.transform.position);
                return time > timeLimit || Vector3.Distance(transform.position,  target) < threshold;
            });
            transform.position = GameManager.Instance.Player.transform.position + useModePos;
            transform.LookAt(GameManager.Instance.Player.transform.position);
        }
        else
        {
            target = posVec + GameManager.Instance.Player.transform.position;
            moveVec = posVec - useModePos;
            moveVec /= timeLimit;
            yield return new WaitUntil(() =>
            {
                time += Time.deltaTime;
                transform.position += moveVec * Time.deltaTime;
                transform.LookAt(GameManager.Instance.Player.transform.position);
                return time > timeLimit || Vector3.Distance(transform.position, target) < threshold;
            });
            Debug.Log(transform.position);
            Debug.Log(GameManager.Instance.Player.transform.position + posVec);
            transform.position = GameManager.Instance.Player.transform.position + posVec;
            transform.LookAt(GameManager.Instance.Player.transform.position);
        }
        OnMoving = false;
    }*/
    
}
