using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class GameStartButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool onLoading;
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void OnPointerClick(PointerEventData data)
    {
        if (!onLoading && data.button == 0)
        {
            onLoading = true;
            GameManager.Instance.StartCoroutine(LoadScene());
        }
    }
    private IEnumerator LoadScene()
    {
        GameManager.Instance.CharacterSelected = Instantiate(GameManager.Instance.CharacterSelected);
        GameManager.Instance.CharacterSelected.gameObject.SetActive(false);
        DontDestroyOnLoad(GameManager.Instance.CharacterSelected.gameObject);
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.LoadingPanel.StartLoad());
        AsyncOperation async = SceneManager.LoadSceneAsync("MainField");
        yield return new WaitUntil(() => {  return async.isDone; });
        GameManager.Instance.StartCoroutine(GameManager.Instance.LoadingPanel.LoadEnd());
        GameManager.Instance.GenerateMap();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        transform.localScale *= 1.1f;
    }
    public void OnPointerExit(PointerEventData data)
    {
        transform.localScale /= 1.1f;
    }
}
