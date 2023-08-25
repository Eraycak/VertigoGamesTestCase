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
        [SerializeField] private int initialItemValue;
        
        public ItemTypes ItemType => itemType;
        public Sprite ItemSprite => itemSprite;
        public int DefaultItemValue
        {
            get => defaultItemValue == 0 ? 1 : defaultItemValue;

            set => defaultItemValue = value;
        }

        public int InitialItemValue
        {
            get => initialItemValue == 0 ? 1 : initialItemValue;
        }
    }

    [CreateAssetMenu(menuName = "ItemSystem/ItemSO", fileName = "ItemSO")]
    public class ItemSO : ScriptableObject
    {
        public ItemProperties itemProperties;
    }
}
