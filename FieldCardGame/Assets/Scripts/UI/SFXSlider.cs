using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SFXSlider : MonoBehaviour
{
    private Slider sfx;
    public Slider Volume
    {
        get
        {
            if (!sfx)
            {
                sfx = GetComponent<Slider>();
            }
            return sfx;
        }
    }
    public void ValueChanged()
    {
        SoundManager.Instance.SFXVolume = Volume.value;
    }
}
