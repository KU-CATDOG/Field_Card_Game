using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        if(!PlayerUIManager.Instance.UseMode)
            transform.position = GameManager.Instance.Player.transform.position + posVec;
        
    }
    public IEnumerator moveCamera(bool useMode)
    {
        OnMoving = true;
        float timeLimit = 0.5f;
        float time = 0f;
        Vector3 moveVec;
        if (useMode)
        {
            moveVec = useModePos - posVec;
            moveVec /= timeLimit;
            yield return new WaitUntil(() =>
            {
                time += Time.deltaTime;
                transform.position += moveVec * Time.deltaTime;
                transform.LookAt(GameManager.Instance.Player.transform.position);
                return time > timeLimit;
            });
            transform.position = GameManager.Instance.Player.transform.position + useModePos;
            transform.LookAt(GameManager.Instance.Player.transform.position);
        }
        else
        {
            moveVec = posVec - useModePos;
            moveVec /= timeLimit;
            yield return new WaitUntil(() =>
            {
                time += Time.deltaTime;
                transform.position += moveVec * Time.deltaTime;
                transform.LookAt(GameManager.Instance.Player.transform.position);
                return time > timeLimit;
            });
            transform.position = GameManager.Instance.Player.transform.position + posVec;
            transform.LookAt(GameManager.Instance.Player.transform.position);
        }
        OnMoving = false;
    }
    
}
