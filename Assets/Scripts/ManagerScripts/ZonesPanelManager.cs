using System;
using DG.Tweening;
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
        [SerializeField] private Image zonesFrameImage;
        [SerializeField] private Sprite currentZoneFrameSprite, superZoneFrameSprite;
        [SerializeField] private float sizeOfCardZone = 110f;
        [SerializeField] private GameObject CardZonePrefab;
        private float targetPositionX = 0f;
        private void Awake()
        {
            targetPositionX = zonesContentTransform.localPosition.x - sizeOfCardZone;
            InstantiateCardZones(11);
            zonesContentTransform.GetChild(RevolverSpinPanelManager.Instance.CurrentZoneIndex - 1).GetComponent<Image>().enabled = true;
            zonesContentTransform.GetChild(RevolverSpinPanelManager.Instance.CurrentZoneIndex - 1).GetComponent<Image>().sprite = currentZoneFrameSprite;
        }

        internal void MoveToNextZone()
        {
            zonesContentTransform.GetChild(RevolverSpinPanelManager.Instance.CurrentZoneIndex - 1).GetComponent<Image>().enabled = false;
            RevolverSpinPanelManager.Instance.CurrentZoneIndex++;
            zonesContentTransform.DOLocalMoveX(targetPositionX, 1f).onComplete +=
                () =>
                {
                    zonesContentTransform.GetChild(RevolverSpinPanelManager.Instance.CurrentZoneIndex - 1).GetComponent<Image>().enabled = true;
                    //zonesContentTransform.GetChild(RevolverSpinPanelManager.Instance.CurrentZoneIndex - 1).GetComponent<Image>().sprite = RevolverSpinPanelManager.Instance.CurrentZoneIndex - 1 == RevolverSpinPanelManager.Instance.spinZoneProperties[0].ZoneIndex ? currentZoneFrameSprite : superZoneFrameSprite;
                    if (RevolverSpinPanelManager.Instance.CurrentZoneIndex %
                        RevolverSpinPanelManager.Instance.spinSOs[2].spinProperties.ZoneIndex == 0)
                    {
                        zonesContentTransform.GetChild(RevolverSpinPanelManager.Instance.CurrentZoneIndex - 1).GetComponent<Image>().sprite = superZoneFrameSprite;
                    }
                    else if (RevolverSpinPanelManager.Instance.CurrentZoneIndex %
                             RevolverSpinPanelManager.Instance.spinSOs[1].spinProperties.ZoneIndex == 0)
                    {
                        zonesContentTransform.GetChild(RevolverSpinPanelManager.Instance.CurrentZoneIndex - 1).GetComponent<Image>().sprite = superZoneFrameSprite;
                        InstantiateCardZones(6);
                    }
                    else
                    {
                        zonesContentTransform.GetChild(RevolverSpinPanelManager.Instance.CurrentZoneIndex - 1).GetComponent<Image>().sprite = currentZoneFrameSprite;
                    }
                    RevolverSpinPanelManager.Instance.SetRevolverSpinProperties();
                    targetPositionX -= sizeOfCardZone;
                };
        }
        
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