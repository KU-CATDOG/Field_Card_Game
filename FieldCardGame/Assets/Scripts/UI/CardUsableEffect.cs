using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardUsableEffect : MonoBehaviour
{
    private Vector3 originScale;
    public Vector3 OriginScale
    {
        get
        {
            if (originScale == Vector3.zero)
            {
                originScale = transform.localScale;
            }
            return originScale;
        }
    }
    private Vector3 highlightedScale;
    public Vector3 HighlightedScale
    {
        get
        {
            if(highlightedScale == Vector3.zero)
            {
                highlightedScale = OriginScale * 1.5f;
            }
            return highlightedScale;
        }
    }

    float speed = 10f;
    float time = 0f;
    private float a1;
    private RawImage img1;
    private Vector2 originSizeLayer1;
    private float a2;
    private RawImage img2;
    private Vector2 originSizeLayer2;
    private float a3;
    private RawImage img3;
    private Vector2 originSizeLayer3;
    private float a0;
    private RawImage img0;
    private float a4;
    private RawImage img4;
    private Vector2 originSizeLayer4;
    public CardObject Target { get; set; }

    private RectTransform defaultOuterLayer;
    private RectTransform usableEffectLayer1;
    public RectTransform UsableEffectLayer1
    {
        get
        {
            if (!defaultOuterLayer)
            {
                defaultOuterLayer = transform.Find("DefaultOuterLayer").GetComponent<RectTransform>();
                img0 = defaultOuterLayer.GetComponent<RawImage>();
                a0 = img0.color.a;
            }
            if (!usableEffectLayer1)
            {
                usableEffectLayer1 = transform.Find("UsableEffectLayer1").GetComponent<RectTransform>();
                originSizeLayer1 = usableEffectLayer1.sizeDelta;
                img1 = usableEffectLayer1.GetComponent<RawImage>();
                a1 = img1.color.a;
            }
            return usableEffectLayer1;
        }
    }
    private RectTransform usableEffectLayer2;
    public RectTransform UsableEffectLayer2
    {
        get
        {
            if (!usableEffectLayer2)
            {
                usableEffectLayer2 = transform.Find("UsableEffectLayer2").GetComponent<RectTransform>();
                originSizeLayer2 = usableEffectLayer2.sizeDelta;
                img2 = usableEffectLayer2.GetComponent<RawImage>();
                a2 = img2.color.a;

            }
            return usableEffectLayer2;
        }
    }
    private RectTransform usableEffectLayer3;
    public RectTransform UsableEffectLayer3
    {
        get
        {
            if (!defaultInnerLayer)
            {
                defaultInnerLayer = transform.Find("DefaultInnerLayer").GetComponent<RectTransform>();
                originSizeLayer4 = defaultInnerLayer.sizeDelta;
                img4 = defaultInnerLayer.GetComponent<RawImage>();
                a4 = img4.color.a;
            }
            if (!usableEffectLayer3)
            {
                usableEffectLayer3 = transform.Find("UsableEffectLayer3").GetComponent<RectTransform>();
                originSizeLayer3 = usableEffectLayer3.sizeDelta;
                img3 = usableEffectLayer3.GetComponent<RawImage>();
                a3 = img3.color.a;

            }
            return usableEffectLayer3;
        }
    }

    private RectTransform defaultInnerLayer;

    void Update()
    {
        transform.position = Target.transform.position;
        transform.rotation = Target.transform.rotation;
        if(time <= 12f / speed)
        {
            UsableEffectLayer1.sizeDelta += Vector2.one * speed * Time.deltaTime;
            UsableEffectLayer2.sizeDelta += Vector2.one * speed * Time.deltaTime;
            UsableEffectLayer3.sizeDelta += Vector2.one * speed * Time.deltaTime;
            defaultInnerLayer.sizeDelta += Vector2.one * speed * Time.deltaTime;

            img1.color += new Color(0, 0, 0, (a0 - a1) / 12f * speed * Time.deltaTime);
            img2.color += new Color(0, 0, 0, (a1 - a2) /12f * speed * Time.deltaTime);
            img3.color += new Color(0, 0, 0, (a2 - a3) / 12f * speed * Time.deltaTime);
            img4.color += new Color(0, 0, 0, (a3 - a4) / 12f * speed * Time.deltaTime);
        }
        else
        {
            time = 0f;
            UsableEffectLayer1.sizeDelta = originSizeLayer1;
            UsableEffectLayer2.sizeDelta = originSizeLayer2;
            UsableEffectLayer3.sizeDelta = originSizeLayer3;
            defaultInnerLayer.sizeDelta = originSizeLayer4;

            img1.color = new Color(1, 1, 1, a1);
            img2.color = new Color(1, 1, 1, a2);
            img3.color = new Color(1, 1, 1, a3);
            img4.color = new Color(1, 1, 1, a4);
        }
        time += Time.deltaTime;
    }
}
