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
    public Character Owner;


    private void Update()
    {
        if(!Owner)
        {
            Destroy(this);
            return;
        }
        string tmp1 = "";
        foreach (var buff in Owner.EffectHandler.BuffDict)
        {
            if (buff.Value.IsEnabled)
                tmp1 += $"{buff.Key.ToString()}({buff.Value.Value})/";
        }

        string tmp2 = "";
        foreach (var debuff in Owner.EffectHandler.DebuffDict)
        {
            if (debuff.Value.IsEnabled)
                tmp2 += $"{debuff.Key.ToString()}({debuff.Value.Value})/";
        }

        buffListText.text = tmp1;
        debuffListText.text = tmp2;

    }

}
