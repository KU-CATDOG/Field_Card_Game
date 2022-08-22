using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGainExp : MonoBehaviour
{
    public void click(){
        StartCoroutine((GameManager.Instance.CharacterSelected as Player).LevelUp());
    }
}
