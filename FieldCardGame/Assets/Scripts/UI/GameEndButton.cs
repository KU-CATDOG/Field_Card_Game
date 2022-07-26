using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameEndButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        if (data.button == 0)
        {
            transform.localScale /= 1.1f;
            GameManager.Instance.StartCoroutine(GameEndRoutine());
        }
    }
    public IEnumerator GameEndRoutine()
    {
        transform.parent.gameObject.SetActive(false);
        AsyncOperation async = SceneManager.LoadSceneAsync("MainMenu");
        while (!async.isDone) yield return null;
        yield return GameManager.Instance.StartCoroutine(TurnManager.Instance.Destroy());
        Destroy(PlayerUIManager.Instance.gameObject);
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.LoadingPanel.LoadEnd());
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
