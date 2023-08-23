using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public struct SpinZoneProperties
{
    [SerializeField] private ZoneTypes zoneType;
    [SerializeField] private int zoneIndex;
    
    public ZoneTypes ZoneType => zoneType;
    public int ZoneIndex => zoneIndex;
}

public class RevolverSpinPanelManager : MonoBehaviour
{
    [SerializeField] private bool sortRandomly = false;
    [SerializeField] private List<SpinSO> spinSOs;
    [SerializeField] internal List<SpinZoneProperties> spinZoneProperties;
    [SerializeField] private List<Transform> revolverSpinRewardPoints;
    [SerializeField] private Image revolverSpinBaseImage, revolverSpinIndicatorImage;
    [SerializeField] private List<ItemSO> revolverSpinRewardItemsSO;
    private SpinSO _spinSO;
    private int currentZoneIndex = 1;
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
    
    private void SetRevolverSpinProperties()
    {
        SetActiveSpinSO();
        SetRevolverSpinImage();
        InitializeRevolverSpinnerItems();
    }

    private void SetActiveSpinSO()
    {
        foreach (var activatedSpinSO in spinSOs.Where(activatedSpinSO => activatedSpinSO.spinProperties.ZoneType == GetZoneType()))
        {
            _spinSO = activatedSpinSO;
        }
    }

    private ZoneTypes GetZoneType()
    {
        return currentZoneIndex % spinSOs[2].spinProperties.ZoneIndex ==0 //spinZoneProperties[2] == Gold Zone
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
        bool isDeathBombAdded = false;
        for (int i = 0; i < revolverSpinRewardPoints.Count; i++)
        {
            var revolverSpinRewardItem = revolverSpinRewardPoints[i].GetComponent<RevolverRewardItem>();
            int itemIndex = sortRandomly ? Random.Range(0, revolverSpinRewardItemsSO.Count) : i;
            revolverSpinRewardItem.Initialize(revolverSpinRewardItemsSO[itemIndex]);
            if (revolverSpinRewardItemsSO[itemIndex].itemProperties.ItemType.Equals(ItemTypes.DeathBomb))
            {
                isDeathBombAdded = true;
            }
        }
        
        if (!isDeathBombAdded)
        {
            int randomIndex = Random.Range(0, revolverSpinRewardPoints.Count);
            var revolverSpinRewardItem = revolverSpinRewardPoints[randomIndex].GetComponent<RevolverRewardItem>();
            revolverSpinRewardItem.Initialize(revolverSpinRewardItemsSO[0]);
        }
    }
}
