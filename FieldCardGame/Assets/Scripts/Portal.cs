using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Portal : MonoBehaviour, IInteractable
{
    [SerializeField]
    private string target;
    private Coordinate pos;
    public Coordinate position
    {
        get
        {
            if (pos == null)
                pos = new Coordinate((int)transform.position.x, (int)transform.position.z);
            return pos;
        }
        set
        {
            pos = value;
            transform.position = new Vector3(pos.X, transform.position.y, pos.Y);
        }
    }
    public Coordinate GetPosition()
    {
        return position;
    }
    public void Interaction()
    {
        GameManager.Instance.StartCoroutine(LoadTarget());
    }
    private IEnumerator LoadTarget()
    {
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.LoadingPanel.StartLoad());
        AsyncOperation async = SceneManager.LoadSceneAsync(target);
        yield return new WaitUntil(() => { return async.isDone; });
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.StartCoroutine(GameManager.Instance.LoadingPanel.LoadEnd());
    }

}
