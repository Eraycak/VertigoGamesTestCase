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

    //finds and assigns the revolver spin button if it is not assigned
    private void OnValidate()
    {
        revolverSpinButton ??= transform.parent.GetChild(2).GetComponent<Button>();
    }

    //removes the listeners from the button and adds them again
    //this is done to prevent the multiple calls of the listeners
    //calculates the angle for each reward point
    private void Awake()
    {
        revolverSpinButton.onClick.RemoveAllListeners();
        revolverSpinButton.onClick.AddListener(SpinRevolver);
        angleForEachRewardPoint = 360f / revolverRewardPointsNumber;
    }

    //if it is clicked, it spins the revolver
    private void SpinRevolver()
    {
        spinning = true;
        SetInteractabilityOfButton(!spinning);
        activeSpinningSpeed = Random.Range(minSpeed, maxSpeed);
        elapsedTime = 0;
    }

    //sets the interactability of the button
    //if it is not interactable, it means that the revolver is spinning
    private void SetInteractabilityOfButton(bool interactable = true)
    {
        revolverSpinButton.interactable = interactable;
    }

    //if the revolver is spinning, it rotates the revolver wheel
    private void Update()
    {
        if (spinning)
        {
            elapsedTime += Time.deltaTime;
            activeSpinningSpeed = Mathf.Lerp(activeSpinningSpeed, 0, elapsedTime / totalSpinTime);
            revolverWheelTransform.Rotate(0, 0, activeSpinningSpeed * Time.deltaTime);
            //if the revolver wheel is almost stopped, it stops the revolver wheel
            if (activeSpinningSpeed <= 0.1f)
            {
                ResetToDefaultValues();
                var roundedZRotation = CalculateRoundedZRotation();
                
                int rewardIndex = Mathf.RoundToInt(roundedZRotation / angleForEachRewardPoint) %
                                  revolverRewardPointsNumber;
                ItemSO rewardItemSO = revolverWheelTransform.GetChild(0).GetChild(rewardIndex).GetChild(0)
                    .GetComponent<RevolverRewardItem>().RevolverRewardItemSO;
                
                //var rewardItemSO = GetRewardItemSo(roundedZRotation);
                Debug.Log(rewardItemSO + "reward item so" + " before dorotate " + rewardItemSO.name);
                revolverWheelTransform.DORotate(new Vector3(0f, 0f, roundedZRotation), 0.5f).OnComplete(() =>
                {
                    RevolverSpinPanelManager.Instance.ScaleDown();//scales down the revolver spin panel
                    rewardWinCardPanel.SetActive(true);
                    //win condition is true if the reward item is not death bomb
                    Debug.Log(rewardItemSO + "reward item so" + " after dorotate" + rewardItemSO.name + "  itemtype " +
                              rewardItemSO.itemProperties.ItemType);
                    var winCondition = !rewardItemSO.itemProperties.ItemType.Equals(ItemTypes.DeathBomb);
                    rewardWinCardPanel.GetComponent<RewardWinCardPanelManager>().
                        SetRewardWinCardPanelProperties(rewardItemSO.itemProperties.ItemSprite, 
                            rewardItemSO.itemProperties.DefaultItemValue.ToString(), 
                            winCondition, rewardItemSO.itemProperties.ItemType);
                });
            }
        }
    }

    //gets the reward item so according to the rounded z rotation
    private ItemSO GetRewardItemSo(float roundedZRotation)
    {
        int rewardIndex = Mathf.RoundToInt(roundedZRotation / angleForEachRewardPoint) %
                          revolverRewardPointsNumber;
        ItemSO rewardItemSO = revolverWheelTransform.GetChild(0).GetChild(rewardIndex).GetChild(0)
            .GetComponent<RevolverRewardItem>().RevolverRewardItemSO;
        return rewardItemSO;
    }

    //calculates the rounded z rotation
    private float CalculateRoundedZRotation()
    {
        float zRotation = revolverWheelTransform.eulerAngles.z;
        float remainder = zRotation % angleForEachRewardPoint;
        float roundedZRotation =
            zRotation - remainder + (remainder < angleForEachRewardPoint / 2 ? 0 : angleForEachRewardPoint);
        return roundedZRotation;
    }

    //resets the values to their default values
    private void ResetToDefaultValues()
    {
        spinning = false;
        SetInteractabilityOfButton(!spinning);
        elapsedTime = 0;
    }
}
