using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Loading : MonoBehaviour
{
    private Image img;
    float speed = 0.02f;
    void Start()
    {
        img = GetComponent<Image>();
    }
    public IEnumerator StartLoad(float margin = 1f, float speed = 0.02f)
    {
        while(img.color.a + speed <= margin)
        {
            img.color = img.color + new Color(0, 0, 0, speed);
            yield return null;
        }
        img.color = new Color(img.color.r, img.color.g, img.color.b, margin);
    }
    public IEnumerator LoadEnd(float speed = 0.02f)
    {
        while (img.color.a - speed >= 0)
        {
            img.color = img.color - new Color(0, 0, 0, speed);
            yield return null;
        }
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
    }
}
