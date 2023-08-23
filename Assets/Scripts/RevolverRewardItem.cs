using System.Collections;
using System.Collections.Generic;
using Enums;
using ScriptableObjectScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RevolverRewardItem : MonoBehaviour
{
    [SerializeField] private Image revolverewardItemImage;
    [SerializeField] private TextMeshProUGUI revolveRewardItemText;

    public void Initialize(ItemSO revolverRewardItemSO)
    {
        revolverewardItemImage.sprite = revolverRewardItemSO.itemProperties.ItemSprite;
        revolveRewardItemText.text = revolverRewardItemSO.itemProperties.ItemType.Equals(ItemTypes.DeathBomb) ? "Bomb" : "x" + revolverRewardItemSO.itemProperties.DefaultItemValue;
    }
}
