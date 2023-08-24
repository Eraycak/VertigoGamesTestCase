using Enums;
using ScriptableObjectScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RevolverRewardItem : MonoBehaviour
{
    [SerializeField] private Image revolverRewardItemImage;
    [SerializeField] private TextMeshProUGUI revolverRewardItemText;
    private ItemSO _revolverRewardItemSO;

    public ItemSO RevolverRewardItemSO
    {
        get => _revolverRewardItemSO;
        set => _revolverRewardItemSO = value;
    }

    public void Initialize(ItemSO revolverRewardItemSO, int currentZoneIndex)
    {
        revolverRewardItemSO.itemProperties.DefaultItemValue *= currentZoneIndex;
        RevolverRewardItemSO = revolverRewardItemSO;
        ItemProperties itemProperties = RevolverRewardItemSO.itemProperties;
        revolverRewardItemImage.sprite = itemProperties.ItemSprite;
        revolverRewardItemText.text = itemProperties.ItemType.Equals(ItemTypes.DeathBomb) ? "Bomb" : "x" + itemProperties.DefaultItemValue;
    }
}
