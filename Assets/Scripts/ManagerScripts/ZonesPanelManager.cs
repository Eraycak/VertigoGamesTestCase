using System.Collections.Generic;
using DG.Tweening;
using ScriptableObjectScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ManagerScripts
{
    public class ZonesPanelManager : MonoBehaviour
    {
        private static ZonesPanelManager _instance;

        public static ZonesPanelManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ZonesPanelManager>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("ZonesPanelManager");
                        _instance = obj.AddComponent<ZonesPanelManager>();
                    }
                }

                return _instance;
            }
        }
        
        [SerializeField] private Transform zonesContentTransform;
        [SerializeField] private Sprite currentZoneFrameSprite, superZoneFrameSprite;
        [SerializeField] private float sizeOfCardZone = 110f;
        [SerializeField] private GameObject CardZonePrefab;
        private float targetPositionX;
        
        //targetPositionX is the x position of the next zone
        //instantiate 11 card zones at the beginning
        //set the current zone image to the first zone
        private void Awake()
        {
            targetPositionX = zonesContentTransform.localPosition.x - sizeOfCardZone;
            InstantiateCardZones(11);
            Image currentZoneImage = zonesContentTransform.GetChild(RevolverSpinPanelManager.Instance.CurrentZoneIndex - 1).GetComponent<Image>();
            currentZoneImage.enabled = true;
            currentZoneImage.sprite = currentZoneFrameSprite;
        }

        //moves the zones content to the next zone
        //if the current zone is silver zone, it instantiates 6 card zones
        //onZonePassed event is invoked to update the revolver spin panel
        //scale up the revolver wheel
        internal void MoveToNextZone()
        {
            int currentZoneIndex = ++RevolverSpinPanelManager.Instance.CurrentZoneIndex;
            zonesContentTransform.GetChild(currentZoneIndex - 2).GetComponent<Image>().enabled = false;
            List<SpinSO> spinSOs = RevolverSpinPanelManager.Instance.spinSOs;
            zonesContentTransform.DOLocalMoveX(targetPositionX, 1f).onComplete +=
                () =>
                {
                    Image currentZoneImage = zonesContentTransform
                        .GetChild(currentZoneIndex - 1).GetComponent<Image>();
                    currentZoneImage.enabled = true;
                    if (currentZoneIndex %
                        spinSOs[2].spinProperties.ZoneIndex == 0)
                    {
                        currentZoneImage.sprite = superZoneFrameSprite;
                    }
                    else if (currentZoneIndex %
                             spinSOs[1].spinProperties.ZoneIndex == 0)
                    {
                        currentZoneImage.sprite = superZoneFrameSprite;
                        InstantiateCardZones(6);
                    }
                    else
                    {
                        currentZoneImage.sprite = currentZoneFrameSprite;
                    }
                    RevolverSpinPanelManager.Instance.OnZonePassed?.Invoke();
                    RevolverSpinPanelManager.Instance.ScaleUp();
                    targetPositionX -= sizeOfCardZone;
                };
        }
        
        //instantiates the card zones according to the number of card zones
        private void InstantiateCardZones(int numberOfCardZones)
        {
            for (int i = 0; i < numberOfCardZones; i++)
            {
                Transform newCard = Instantiate(CardZonePrefab, zonesContentTransform).transform;
                newCard.GetComponent<Image>().enabled = false;
                newCard.GetChild(0).GetComponent<TextMeshProUGUI>().text = zonesContentTransform.childCount.ToString();
            }
        }
    }
}