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
        ResetItemSO(revolverRewardItemSO, currentZoneIndex);
        revolverRewardItemSO.itemProperties.DefaultItemValue *= CheckSuperZone(spinSO, currentZoneIndex);
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

    //resets the item value to its initial value
    //if the current zone index is 1
    //due to resetting the item value if game is restarted or started
    private void ResetItemSO(ItemSO revolverRewardItemSO, int currentZoneIndex = 1)
    {
        if (currentZoneIndex == 1)
        {
            revolverRewardItemSO.itemProperties.DefaultItemValue = revolverRewardItemSO.itemProperties.InitialItemValue;
        }
    }
}
