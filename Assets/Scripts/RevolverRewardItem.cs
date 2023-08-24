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
    private ItemSO _revolverRewardItemSO;
    public ItemSO RevolverRewardItemSO => _revolverRewardItemSO;

    public void Initialize(ItemSO revolverRewardItemSO)
    {
        _revolverRewardItemSO = revolverRewardItemSO;
        revolverewardItemImage.sprite = _revolverRewardItemSO.itemProperties.ItemSprite;
        revolveRewardItemText.text = _revolverRewardItemSO.itemProperties.ItemType.Equals(ItemTypes.DeathBomb) ? "Bomb" : "x" + revolverRewardItemSO.itemProperties.DefaultItemValue;
    }
}
