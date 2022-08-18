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
        //fixme
        GameManager.Instance.CharacterSelected.transform.position = new Vector3(10, 1, 10);
        //
        GameManager.Instance.CharacterSelected.gameObject.SetActive(false);
        DontDestroyOnLoad(GameManager.Instance.CharacterSelected.gameObject);
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.LoadingPanel.StartLoad());
        AsyncOperation async = SceneManager.LoadSceneAsync("Grassland");
        yield return new WaitUntil(() => {  return async.isDone; });
        GameManager.Instance.GenerateMap();
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.StartCoroutine(GameManager.Instance.LoadingPanel.LoadEnd());
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
