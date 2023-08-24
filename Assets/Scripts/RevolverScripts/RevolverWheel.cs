using System;
using DG.Tweening;
using Enums;
using ManagerScripts;
using ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RevolverWheel : MonoBehaviour
{
    [SerializeField] private Transform revolverWheelTransform;
    [SerializeField] private Button revolverSpinButton;
    [SerializeField] private float maxSpeed= 1000f;
    [SerializeField] private float minSpeed = 100f;
    [SerializeField] private float totalSpinTime = 10f;
    [SerializeField] private int revolverRewardPointsNumber = 8;
    [SerializeField] private GameObject rewardWinCardPanel;
    private float activeSpinningSpeed;
    private float elapsedTime;
    private bool spinning = false;
    private float angleForEachRewardPoint;

    private void OnValidate()
    {
        revolverSpinButton ??= transform.parent.GetChild(2).GetComponent<Button>();
    }

    private void Awake()
    {
        revolverSpinButton.onClick.RemoveAllListeners();
        revolverSpinButton.onClick.AddListener(SpinRevolver);
        angleForEachRewardPoint = 360f / revolverRewardPointsNumber;
    }

    private void SpinRevolver()
    {
        spinning = true;
        SetInteractabilityOfButton(!spinning);
        activeSpinningSpeed = Random.Range(minSpeed, maxSpeed);
        elapsedTime = 0;
    }

    private void SetInteractabilityOfButton(bool interactable = true)
    {
        revolverSpinButton.interactable = interactable;
    }

    private void Update()
    {
        if (spinning)
        {
            elapsedTime += Time.deltaTime;
            activeSpinningSpeed = Mathf.Lerp(activeSpinningSpeed, 0, elapsedTime / totalSpinTime);
            revolverWheelTransform.Rotate(0, 0, activeSpinningSpeed * Time.deltaTime);
            if (activeSpinningSpeed <= 0.1f)
            {
                spinning = false;
                SetInteractabilityOfButton(!spinning);
                elapsedTime = 0;
                float zRotation = revolverWheelTransform.eulerAngles.z;
                float remainder = zRotation % angleForEachRewardPoint;
                float roundedZRotation = zRotation - remainder + (remainder < angleForEachRewardPoint / 2 ? 0 : angleForEachRewardPoint);
                int rewardIndex = Mathf.RoundToInt(roundedZRotation / angleForEachRewardPoint) %
                                  revolverRewardPointsNumber;
                ItemSO rewardItemSO = revolverWheelTransform.GetChild(0).GetChild(rewardIndex).GetChild(0)
                    .GetComponent<RevolverRewardItem>().RevolverRewardItemSO;
                revolverWheelTransform.DORotate(new Vector3(0f, 0f, roundedZRotation), 0.5f).OnComplete(() =>
                {
                    rewardWinCardPanel.SetActive(true);
                    var winCondition = !rewardItemSO.itemProperties.ItemType.Equals(ItemTypes.DeathBomb);
                    rewardWinCardPanel.GetComponent<RewardWinCardPanelManager>().SetRewardWinCardPanelProperties(rewardItemSO.itemProperties.ItemSprite, rewardItemSO.itemProperties.DefaultItemValue.ToString(), winCondition, rewardItemSO.itemProperties.ItemType);
                });
            }
        }
    }
}
