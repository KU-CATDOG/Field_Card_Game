using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector2 originPos;
    private Vector2 useModePos;
    private Vector2 originScale;
    private Vector2 highlightedScale;
    private float moveSpeed = 3f;
    public void OnPointerEnter(PointerEventData data)
    {
        transform.localScale = highlightedScale;
    }
    public void OnPointerExit(PointerEventData data)
    {
        transform.localScale = originScale;
    }
    void Start()
    {
        originPos = transform.position;
        originScale = transform.localScale;
        useModePos = new Vector2(Screen.width * 2 - originPos.x, originPos.y);
        highlightedScale = transform.localScale * 1.2f;
    }
    private void FixedUpdate()
    {
        float threshold = 0.5f;
        Vector2 targetPos;
        if (!PlayerUIManager.Instance.UseMode && !PlayerUIManager.Instance.OnRoutine)
        {
            targetPos = originPos;
        }
        else 
        {
            targetPos = useModePos;
        }
        if(Mathf.Abs(transform.position.x - targetPos.x) > threshold)
        {
            transform.position += ((Vector3)targetPos - transform.position) * Time.fixedDeltaTime * moveSpeed;
        }
    }
}
