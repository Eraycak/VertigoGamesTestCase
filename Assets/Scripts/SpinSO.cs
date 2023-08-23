using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace  ScriptableObjectScripts
{
    [Serializable]
    public struct SpinProperties
    {
        [SerializeField] private ZoneTypes zoneType;
        [SerializeField] private Sprite spinRouletteSprite;
        [SerializeField] private Sprite spinIndicatorSprite;
        [SerializeField] private int zoneIndex;

        public ZoneTypes ZoneType => zoneType;
        public Sprite SpinRouletteSprite => spinRouletteSprite;
        public Sprite SpinIndicatorSprite => spinIndicatorSprite;
        public int ZoneIndex => zoneIndex;
    }

    [Serializable]
    public struct SpinItemProperties
    {
        [SerializeField] private ItemSO itemSO;
        public ItemSO ItemSO => itemSO;
    }
    
    [CreateAssetMenu(menuName = "SpinSystem/SpinSystem/SpinSO", fileName= "SpinSO", order = 0)]
    public class SpinSO : ScriptableObject
    {
        public SpinProperties spinProperties;
        public List<SpinItemProperties> spinItemProperties;
    }
}