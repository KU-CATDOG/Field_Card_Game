using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CharacterSelectButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Character character;
    private GameStartButton gameStartButton;
    private RawImage image;
    private static CharacterSelectButton prev;
    private bool selected;
    public bool Selected
    {
        get
        {
            return selected;
        }
        set
        {
            selected = value;
            if(selected == true)
            {
                if(prev && prev != this)
                    prev.Selected = false;
                else if (prev == this)
                    return;
                else
                {
                    gameStartButton.gameObject.SetActive(true);
                }
                GameManager.Instance.CharacterSelectedPrefab = character;
                image.color = Color.red;
                prev = this;
            }
            if(selected == false)
            {
                transform.localScale /= 1.1f;
                image.color = Color.white;
            }
        }
    }
    private void Awake()
    {
        gameStartButton = FindObjectOfType<GameStartButton>();
        image = GetComponent<RawImage>();
    }
    public void OnPointerClick(PointerEventData data)
    {
        if (data.button == 0)
        {
            Selected = true;
        }
    }
    public void OnPointerEnter(PointerEventData data)
    {
        if(!Selected)
            transform.localScale *= 1.1f;
    }
    public void OnPointerExit(PointerEventData data)
    {
        if(!Selected)
            transform.localScale /= 1.1f;
    }
}
