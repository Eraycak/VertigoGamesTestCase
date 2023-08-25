using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ManagerScripts
{
    internal struct SpinZoneProperties
    {
        [SerializeField] private ZoneTypes zoneType;
        [SerializeField] private int zoneIndex;

        public ZoneTypes ZoneType => zoneType;
        public int ZoneIndex => zoneIndex;
    }

    public class RevolverSpinPanelManager : MonoBehaviour
    {
        private static RevolverSpinPanelManager _instance;

        public static RevolverSpinPanelManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<RevolverSpinPanelManager>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("RevolverSpinPanelManager");
                        _instance = obj.AddComponent<RevolverSpinPanelManager>();
                    }
                }

                return _instance;
            }
        }
        
        [SerializeField] private bool sortRandomly = false;
        [SerializeField] internal List<SpinSO> spinSOs;
        [SerializeField] internal List<SpinZoneProperties> spinZoneProperties;
        [SerializeField] private List<Transform> revolverSpinRewardPoints;
        [SerializeField] private Image revolverSpinBaseImage, revolverSpinIndicatorImage;
        [SerializeField] private List<ItemSO> revolverSpinRewardItemsSO;
        private SpinSO _spinSO;
        private int currentZoneIndex = 1;
        private Vector3 revolverWheelScale = Vector3.one;
        private Vector3 revolverWheelShrinkedScale = Vector3.zero;

        public int CurrentZoneIndex
        {
            get => currentZoneIndex;
            set => currentZoneIndex = value;
        }

        public Action OnZonePassed;

        private void OnEnable()
        {
            OnZonePassed += SetRevolverSpinProperties;
        }

        private void OnDisable()
        {
            OnZonePassed -= SetRevolverSpinProperties;
        }

        private void OnValidate()
        {
            SetRevolverSpinProperties();
        }

        internal void SetRevolverSpinProperties()
        {
            SetActiveSpinSO();
            SetRevolverSpinImage();
            InitializeRevolverSpinnerItems();
            ChangeExitButtonInteractability();
        }

        private void ChangeExitButtonInteractability()
        {
            CollectedItemsPanelManager collectedItemsPanelManager = CollectedItemsPanelManager.Instance;
            if (_spinSO.spinProperties.ZoneType.Equals(ZoneTypes.BronzeRevolverSpin))
            {
                collectedItemsPanelManager.DisableButton();
            }
            else
            {
                collectedItemsPanelManager.EnableButton();
            }
        }

        private void SetActiveSpinSO()
        {
            foreach (var activatedSpinSO in spinSOs.Where(activatedSpinSO =>
                         activatedSpinSO.spinProperties.ZoneType == GetZoneType()))
            {
                _spinSO = activatedSpinSO;
            }
        }

        private ZoneTypes GetZoneType()
        {
            return currentZoneIndex % spinSOs[2].spinProperties.ZoneIndex == 0 //spinZoneProperties[2] == Gold Zone
                ? spinSOs[2].spinProperties.ZoneType
                : (currentZoneIndex % spinSOs[1].spinProperties.ZoneIndex == 0 //spinZoneProperties[1] == Silver Zone
                    ? spinSOs[1].spinProperties.ZoneType
                    : spinSOs[0].spinProperties.ZoneType); //spinZoneProperties[0] == Bronze Zone
        }

        private void SetRevolverSpinImage()
        {
            revolverSpinBaseImage.sprite = _spinSO.spinProperties.SpinRouletteSprite;
            revolverSpinIndicatorImage.sprite = _spinSO.spinProperties.SpinIndicatorSprite;
        }

        private void InitializeRevolverSpinnerItems()
        {
            var shuffledIndices = CreateShuffledIndices();

            bool isDeathBombAdded = false;
            var bombRevolverSpinRewardItem = revolverSpinRewardPoints[0].GetComponent<RevolverRewardItem>();
            for (int i = 0; i < revolverSpinRewardPoints.Count; i++)
            {
                var revolverSpinRewardItem = revolverSpinRewardPoints[i].GetComponent<RevolverRewardItem>();
                int itemIndex =
                    sortRandomly
                        ? shuffledIndices[i]
                        : i;
                revolverSpinRewardItem.Initialize(revolverSpinRewardItemsSO[itemIndex], currentZoneIndex, _spinSO);
                if (revolverSpinRewardItemsSO[itemIndex].itemProperties.ItemType.Equals(ItemTypes.DeathBomb))
                {
                    isDeathBombAdded = true;
                    bombRevolverSpinRewardItem = revolverSpinRewardItem;
                }
            }

            isDeathBombAdded = CheckZoneAndBombConditions(isDeathBombAdded, shuffledIndices , bombRevolverSpinRewardItem);
        }

        private bool CheckZoneAndBombConditions(bool isDeathBombAdded, List<int> shuffledIndices , RevolverRewardItem bombRevolverSpinRewardItem)
        {
            if (currentZoneIndex % spinSOs[2].spinProperties.ZoneIndex == 0 ||
                currentZoneIndex % spinSOs[1].spinProperties.ZoneIndex == 0)
            {
                if (isDeathBombAdded)
                {
                    bombRevolverSpinRewardItem.Initialize(
                        revolverSpinRewardItemsSO[shuffledIndices[revolverSpinRewardItemsSO.Count + 1]], currentZoneIndex, _spinSO);
                    return false;
                }
            }
            else
            {
                if (!isDeathBombAdded)
                {
                    int randomIndex = Random.Range(0, revolverSpinRewardPoints.Count);
                    var revolverSpinRewardItem = revolverSpinRewardPoints[randomIndex].GetComponent<RevolverRewardItem>();
                    revolverSpinRewardItem.Initialize(revolverSpinRewardItemsSO[0], currentZoneIndex, _spinSO);
                    return true;
                }
            }

            return false;
        }

        private List<int> CreateShuffledIndices()
        {
            List<int> shuffledIndices = new List<int>();
            if (sortRandomly)
            {
                shuffledIndices.AddRange(Enumerable.Range(0, revolverSpinRewardItemsSO.Count)
                    .OrderBy(a => Guid.NewGuid()));
            }

            return shuffledIndices;
        }

        internal void ScaleDown()
        {
            transform.DOScale(revolverWheelShrinkedScale, 1f);
        }
        
        internal void ScaleUp()
        {
            transform.DOScale(revolverWheelScale, 1f);
        }
    }
}