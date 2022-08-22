using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMSlider : MonoBehaviour
{
    private Slider bgm;
    public Slider Volume
    {
        get
        {
            if (!bgm)
            {
                bgm = GetComponent<Slider>();
            }
            return bgm;
        }
    }
    public void ValueChanged()
    {
        SoundManager.Instance.BGMVolume = Volume.value;
    }
}
