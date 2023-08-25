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

    //initializes the revolver reward item
    public void Initialize(ItemSO revolverRewardItemSO, int currentZoneIndex, SpinSO spinSO)
    {
        revolverRewardItemSO.itemProperties.DefaultItemValue = revolverRewardItemSO.itemProperties.InitialItemValue * CheckSuperZone(spinSO, currentZoneIndex);
        RevolverRewardItemSO = revolverRewardItemSO;
        ItemProperties itemProperties = RevolverRewardItemSO.itemProperties;
        revolverRewardItemImage.sprite = itemProperties.ItemSprite;
        revolverRewardItemText.text = itemProperties.ItemType.Equals(ItemTypes.DeathBomb) ? "Bomb" : "x" + itemProperties.DefaultItemValue;
    }

    //checks if the current zone is gold revolver spin zone
    //if it is gold revolver spin zone, it returns 10 * currentZoneIndex
    //else it returns 1 * currentZoneIndex
    private int CheckSuperZone(SpinSO spinSO, int currentZoneIndex = 1)
    {
        return spinSO.spinProperties.ZoneType.Equals(ZoneTypes.GoldRevolverSpin)
            ? 10 * currentZoneIndex
            : 1 * currentZoneIndex;
    }
}
