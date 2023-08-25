using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using ScriptableObjectScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ManagerScripts
{
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
        
        private bool useSpinSOsItems = false;
        [SerializeField] private bool sortRandomly = false;
        [SerializeField] internal List<SpinSO> spinSOs;
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

        //Creates action event to call from other scripts and change the revolver spin properties
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
            ChangeUseSpinSOsItemsValue();
            SetRevolverSpinProperties();
        }

        //Changes the useSpinSOsItems value according to the sortRandomly value
        private void ChangeUseSpinSOsItemsValue()
        {
            if (sortRandomly)
            {
                useSpinSOsItems = !sortRandomly;
            }
            else
            {
                useSpinSOsItems = !sortRandomly;
            }
        }

        //Sets the revolver spin properties
        private void SetRevolverSpinProperties()
        {
            SetActiveSpinSO();
            SetRevolverSpinImage();
            InitializeRevolverSpinnerItems();
            ChangeExitButtonInteractability();
        }

        //Changes the interactability of the exit button according to the zone type
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

        //Sets the active spin scriptable object according to the zone type
        private void SetActiveSpinSO()
        {
            foreach (var activatedSpinSO in spinSOs.Where(activatedSpinSO =>
                         activatedSpinSO.spinProperties.ZoneType == GetZoneType()))
            {
                _spinSO = activatedSpinSO;
            }
        }

        //Gets the zone type according to the current zone index
        private ZoneTypes GetZoneType()
        {
            return currentZoneIndex % spinSOs[2].spinProperties.ZoneIndex == 0 //spinZoneProperties[2] == Gold Zone
                ? spinSOs[2].spinProperties.ZoneType
                : (currentZoneIndex % spinSOs[1].spinProperties.ZoneIndex == 0 //spinZoneProperties[1] == Silver Zone
                    ? spinSOs[1].spinProperties.ZoneType
                    : spinSOs[0].spinProperties.ZoneType); //spinZoneProperties[0] == Bronze Zone
        }

        //Sets the revolver spin image according to the zone type
        private void SetRevolverSpinImage()
        {
            revolverSpinBaseImage.sprite = _spinSO.spinProperties.SpinRouletteSprite;
            revolverSpinIndicatorImage.sprite = _spinSO.spinProperties.SpinIndicatorSprite;
        }

        //Initializes the revolver spinner items
        private void InitializeRevolverSpinnerItems()
        {
            var shuffledIndices = CreateShuffledIndices();

            bool isDeathBombAdded = false;
            //this is just tmp value for keeping the reference of the bomb revolver spin reward item
            //and using it in the CheckZoneAndBombConditions method
            var bombRevolverSpinRewardItem = revolverSpinRewardPoints[0].GetComponent<RevolverRewardItem>();
            for (int i = 0; i < revolverSpinRewardPoints.Count; i++)
            {
                var revolverSpinRewardItem = revolverSpinRewardPoints[i].GetComponent<RevolverRewardItem>();
                //if sortRandomly is true, it uses shuffled indices
                //else it uses the normal indices how they are in the revolverSpinRewardItemsSO list
                int itemIndex = sortRandomly ? shuffledIndices[i] : i;
                if (useSpinSOsItems)
                {
                    revolverSpinRewardItem.Initialize(_spinSO.spinItemProperties[itemIndex].ItemSO, currentZoneIndex, _spinSO);
                }
                else
                {
                    revolverSpinRewardItem.Initialize(revolverSpinRewardItemsSO[itemIndex], currentZoneIndex, _spinSO);
                }
                (isDeathBombAdded, bombRevolverSpinRewardItem) = IsDeathBombAdded(itemIndex, isDeathBombAdded, revolverSpinRewardItem, bombRevolverSpinRewardItem);
            }

            CheckZoneAndBombConditions(isDeathBombAdded, shuffledIndices , bombRevolverSpinRewardItem);
        }

        //Checks if the death bomb is added to the revolver spinner items
        //if it is added, it returns true and the bomb revolver spin reward item
        private (bool isDeathBombAdded, RevolverRewardItem bombRevolverSpinRewardItem) IsDeathBombAdded(int itemIndex,
            bool isDeathBombAdded, RevolverRewardItem revolverSpinRewardItem, RevolverRewardItem bombRevolverSpinRewardItem)
        {
            if (revolverSpinRewardItemsSO[itemIndex].itemProperties.ItemType.Equals(ItemTypes.DeathBomb))
            {
                isDeathBombAdded = true;
                bombRevolverSpinRewardItem = revolverSpinRewardItem;
            }

            return (isDeathBombAdded, bombRevolverSpinRewardItem);
        }

        //Checks the zone and bomb conditions
        //if the zone index is divisible by the zone index of the gold or silver zone
        //it checks if the death bomb is added to the revolver spinner items
        //if it is added, it removes the bomb revolver spin reward item from the revolver spinner items
        //else if the zone index is not divisible by the zone index of the gold or silver zone
        //it checks if the death bomb is added to the revolver spinner items
        //if it is not added, it adds the bomb revolver spin reward item to the revolver spinner items
        private void CheckZoneAndBombConditions(bool isDeathBombAdded, List<int> shuffledIndices,
            RevolverRewardItem bombRevolverSpinRewardItem)
        {
            if (currentZoneIndex % spinSOs[2].spinProperties.ZoneIndex == 0 ||
                currentZoneIndex % spinSOs[1].spinProperties.ZoneIndex == 0)
            {
                if (isDeathBombAdded)
                {
                    bombRevolverSpinRewardItem.Initialize(
                        revolverSpinRewardItemsSO[shuffledIndices[revolverSpinRewardItemsSO.Count + 1]], currentZoneIndex, _spinSO);
                }
            }
            else
            {
                if (!isDeathBombAdded)
                {
                    int randomIndex = Random.Range(0, revolverSpinRewardPoints.Count);
                    var revolverSpinRewardItem = revolverSpinRewardPoints[randomIndex].GetComponent<RevolverRewardItem>();
                    revolverSpinRewardItem.Initialize(revolverSpinRewardItemsSO[0], currentZoneIndex, _spinSO);
                }
            }
        }

        //Creates shuffled indices if sortRandomly is true
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

        //Scales down the revolver wheel via DOTween
        internal void ScaleDown()
        {
            transform.DOScale(revolverWheelShrinkedScale, 1f);
        }
        
        //Scales up the revolver wheel via DOTween
        internal void ScaleUp()
        {
            transform.DOScale(revolverWheelScale, 1f);
        }
    }
}