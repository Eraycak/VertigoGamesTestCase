using System;
using Enums;
using UnityEngine;

namespace ScriptableObjectScripts
{
    [Serializable]
    public struct ItemProperties
    {
        [SerializeField] private ItemTypes itemType;
        [SerializeField] private Sprite itemSprite;
        [SerializeField] private int defaultItemValue;
        
        public ItemTypes ItemType => itemType;
        public Sprite ItemSprite => itemSprite;
        public int DefaultItemValue
        {
            get => defaultItemValue == 0 ? 1 : defaultItemValue;

            set => defaultItemValue = value;
        }
    }

    [CreateAssetMenu(menuName = "ItemSystem/ItemSO", fileName = "ItemSO")]
    public class ItemSO : ScriptableObject
    {
        public ItemProperties itemProperties;
    }
}
