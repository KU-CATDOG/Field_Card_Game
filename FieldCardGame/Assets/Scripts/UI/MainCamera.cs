using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MainCamera : MonoBehaviour
{
    public static MainCamera Instance { get; private set; }
    public bool CameraLock { get; set; } = true;
    public bool HardLock { get; set; } = true;
    private Vector3 posVec;
    //private Vector3 useModePos;
    public Vector3 target;
    private float posVecMaxThreshold;
    private float posVecMinThreshold;
    private float MouseCameraMoveSpeed = 5f;
    private float KeyboardCameraMoveSpeed = 5f;

    private Vector3 right = new Vector3(1, 0, -1);
    private Vector3 left = new Vector3(-1, 0, 1);
    private Vector3 up = new Vector3(1, 0, 1);
    private Vector3 down = new Vector3(-1, 0, -1);
    public bool OnMoving { get; private set; } = false;
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
    void Start()
    {
        posVec = transform.position - GameManager.Instance.CurPlayer.transform.position;
        posVecMaxThreshold = posVec.magnitude;
        posVecMinThreshold = 3f;
        //useModePos = posVec + Vector3.up * 4;

    }
    // Update is called once per frame
    void Update()
    {
        if (TurnManager.Instance.IsPlayerTurn)
        {

            if (Input.GetKeyDown(KeyCode.Y))
            {
                HardLock = !HardLock;
            }
            CameraLock = HardLock;
            if (!HardLock)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    CameraLock = true;
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    CameraLock = false;
                }
            }
            if (CameraLock)
            {
                target = GameManager.Instance.CurPlayer.transform.position;
            }
            else
            {
                Vector3 MoveDelta = new Vector3();
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    MoveDelta += Time.deltaTime * KeyboardCameraMoveSpeed * up;
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    MoveDelta += Time.deltaTime * KeyboardCameraMoveSpeed * left;
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    MoveDelta += Time.deltaTime * KeyboardCameraMoveSpeed * down;
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    MoveDelta += Time.deltaTime * KeyboardCameraMoveSpeed * right;
                }

                if (Input.mousePosition.x >= Screen.width)
                {
                    MoveDelta += right * (Time.deltaTime * MouseCameraMoveSpeed);
                }
                if (Input.mousePosition.x <= 0)
                {
                    MoveDelta += left * (Time.deltaTime * MouseCameraMoveSpeed);
                }
                if (Input.mousePosition.y >= Screen.height)
                {
                    MoveDelta += up * (Time.deltaTime * MouseCameraMoveSpeed);
                }
                if (Input.mousePosition.y <= 0)
                {
                    MoveDelta += down * (Time.deltaTime * MouseCameraMoveSpeed);
                }
                target += MoveDelta;
            }
            if (!PlayerUIManager.Instance.PanelOpenned && Input.mouseScrollDelta.x == 0 && Input.mouseScrollDelta.y > 0 && posVecMinThreshold <= (posVec - posVec.normalized * Input.mouseScrollDelta.y).magnitude)
            {
                posVec -= posVec.normalized * Input.mouseScrollDelta.y;
            }
            else if (!PlayerUIManager.Instance.PanelOpenned && Input.mouseScrollDelta.x == 0 && Input.mouseScrollDelta.y < 0 && posVecMaxThreshold >= (posVec - posVec.normalized * Input.mouseScrollDelta.y).magnitude)
            {
                posVec -= posVec.normalized * Input.mouseScrollDelta.y;
            }

            transform.position = target + posVec;
            transform.LookAt(target);
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
