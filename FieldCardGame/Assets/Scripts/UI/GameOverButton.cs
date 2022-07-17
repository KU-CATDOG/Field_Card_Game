using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        if (data.button == 0)
        {
            transform.localScale /= 1.1f;
            transform.parent.gameObject.SetActive(false);
            SceneManager.LoadScene("MainMenu");
            GameManager.Instance.StartCoroutine(GameManager.Instance.LoadingPanel.LoadEnd());
        }
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
