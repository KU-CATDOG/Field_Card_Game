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
        DontDestroyOnLoad(gameObject.transform.parent);
    }
    public IEnumerator StartLoad()
    {
        while(img.color.a + speed <= 1)
        {
            img.color = img.color + new Color(0, 0, 0, speed);
            yield return null;
        }
        img.color = new Color(0, 0, 0, 1);
    }
    public IEnumerator LoadEnd()
    {
        while (img.color.a - speed >= 0)
        {
            img.color = img.color - new Color(0, 0, 0, speed);
            yield return null;
        }
        img.color = new Color(0, 0, 0, 0);
    }
}
