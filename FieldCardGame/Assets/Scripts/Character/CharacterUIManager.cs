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
            {
                switch (buff.Key)
                {
                    case BuffType.Debug:
                        tmp1 += "Debug/";
                        break;
                    case BuffType.Shield:
                        tmp1 += $"Shield({buff.Value.Value})/";
                        break;
                    case BuffType.Strengthen:
                        tmp1 += $"Strengthen({buff.Value.Value})/";
                        break;
                    case BuffType.Will:
                        tmp1 += $"Will({buff.Value.Value})/";
                        break;
                }
            }
        }

        string tmp2 = "";
        foreach (var debuff in character.EffectHandler.DebuffDict)
        {
            if (debuff.Value.IsEnabled)
            {
                switch (debuff.Key)
                {
                    case DebuffType.Poison:
                        tmp2 += $"Poison({debuff.Value.Value})/";
                        break;
                    case DebuffType.Stun:
                        tmp2 += $"Stun({debuff.Value.Value})/";
                        break;
                    case DebuffType.Weakness:
                        tmp2 += $"Weakness({debuff.Value.Value})/";
                        break;
                }
            }
        }

        buffListText.text = tmp1;
        debuffListText.text = tmp2;
    }
}
