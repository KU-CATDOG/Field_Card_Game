using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterUIManager : MonoBehaviour
{
    [SerializeField]
    private Slider hpBar;
    [SerializeField]
    private TextMeshProUGUI hpText;
    [SerializeField]
    private TextMeshProUGUI buffListText;
    [SerializeField]
    private TextMeshProUGUI debuffListText;

    private Character character;

    private void Start()
    {
        character = GetComponentInParent<Character>();
    }

    private void Update()
    {
        string tmp1 = "";
        foreach (var buff in character.EffectHandler.BuffDict)
        {
            if (buff.Value.IsEnabled)
                tmp1 += $"{buff.Key.ToString()}({buff.Value.Value})/";
        }

        string tmp2 = "";
        foreach (var debuff in character.EffectHandler.DebuffDict)
        {
            if (debuff.Value.IsEnabled)
                tmp2 += $"{debuff.Key.ToString()}({debuff.Value.Value})/";
        }

        buffListText.text = tmp1;
        debuffListText.text = tmp2;

        LookCamera();
    }

    private void LookCamera()
    {
        transform.LookAt(transform.position + MainCamera.Instance.transform.rotation * Vector3.forward, MainCamera.Instance.transform.rotation * Vector3.up);
    }
}
