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

    public void Initialize(ItemSO revolverRewardItemSO, int currentZoneIndex, SpinSO spinSO)
    {
        ResetItemSO(revolverRewardItemSO, currentZoneIndex);
        revolverRewardItemSO.itemProperties.DefaultItemValue *= CheckSuperZone(spinSO, currentZoneIndex);
        RevolverRewardItemSO = revolverRewardItemSO;
        ItemProperties itemProperties = RevolverRewardItemSO.itemProperties;
        revolverRewardItemImage.sprite = itemProperties.ItemSprite;
        revolverRewardItemText.text = itemProperties.ItemType.Equals(ItemTypes.DeathBomb) ? "Bomb" : "x" + itemProperties.DefaultItemValue;
    }

    private int CheckSuperZone(SpinSO spinSO, int currentZoneIndex = 1)
    {
        Debug.Log(spinSO.spinProperties.ZoneType + "  " + spinSO.spinProperties.ZoneType.Equals(ZoneTypes.GoldRevolverSpin));
        return spinSO.spinProperties.ZoneType.Equals(ZoneTypes.GoldRevolverSpin)
            ? 10 * currentZoneIndex
            : 1 * currentZoneIndex;
    }

    private void ResetItemSO(ItemSO revolverRewardItemSO, int currentZoneIndex = 1)
    {
        if (currentZoneIndex == 1)
        {
            revolverRewardItemSO.itemProperties.DefaultItemValue = revolverRewardItemSO.itemProperties.InitialItemValue;
        }
    }
}
