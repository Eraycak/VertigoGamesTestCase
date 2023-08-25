using System;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ManagerScripts
{
    [Serializable]
    internal struct RewardWinCardPanelProperties
    {
        [SerializeField] private Image rewardWinCardPanelImage;
        [SerializeField] private TextMeshProUGUI rewardWinCardPanelText;
        [SerializeField] private TextMeshProUGUI rewardWinCardPanelResultText;
        [SerializeField] private Button rewardWinCardPanelResultButton;
        [SerializeField] private Sprite rewardWinCardPanelResultButtonWinSprite;
        [SerializeField] private Sprite rewardWinCardPanelResultButtonLoseSprite;

        public Image Image
        {
            get => rewardWinCardPanelImage;
            set => rewardWinCardPanelImage = value;
        }

        public TextMeshProUGUI Text
        {
            get => rewardWinCardPanelText;
            set => rewardWinCardPanelText = value;
        }

        public TextMeshProUGUI ResultText
        {
            get => rewardWinCardPanelResultText;
            set => rewardWinCardPanelResultText = value;
        }

        public Button ResultButton
        {
            get => rewardWinCardPanelResultButton;
            set => rewardWinCardPanelResultButton = value;
        }

        public Sprite RewardWinCardPanelResultButtonWinSprite => rewardWinCardPanelResultButtonWinSprite;

        public Sprite RewardWinCardPanelResultButtonLoseSprite => rewardWinCardPanelResultButtonLoseSprite;
    }

    public class RewardWinCardPanelManager : MonoBehaviour
    {
        [SerializeField] internal RewardWinCardPanelProperties rewardWinCardPanelProperties;
        [SerializeField]
        internal string congratsYouWonPlayMoreToWınMore = "Congrats! You won!\nPlay more to wın more!!!";
        [SerializeField] internal string sorryYouLostTryAgainToWın = "Sorry! You lost!\nTry again to wın!!!";
        [SerializeField] internal string playMore = "Play More";
        [SerializeField] internal string tryAgain = "Try Again";

        //finds and assigns the result button if it is not assigned
        private void OnValidate()
        {
            rewardWinCardPanelProperties.ResultButton ??= transform.GetChild(2).GetComponent<Button>();
        }

        //sets the properties of the reward win card panel
        //sets the result button's properties according to the win or lose
        //removes the listeners from the result button and adds them again
        //this is done to prevent the multiple calls of the listeners
        internal void SetRewardWinCardPanelProperties(Sprite rewardWinCardPanelSprite, string rewardWinCardPanelText,
            bool win = true, ItemTypes itemType = ItemTypes.None)
        {
            rewardWinCardPanelProperties.Image.sprite = rewardWinCardPanelSprite;
            rewardWinCardPanelProperties.Text.text = win ? rewardWinCardPanelText : "Death Bomb";
            rewardWinCardPanelProperties.ResultText.text =
                win ? congratsYouWonPlayMoreToWınMore : sorryYouLostTryAgainToWın;
            rewardWinCardPanelProperties.ResultButton.image.sprite = win
                ? rewardWinCardPanelProperties.RewardWinCardPanelResultButtonWinSprite
                : rewardWinCardPanelProperties.RewardWinCardPanelResultButtonLoseSprite;
            rewardWinCardPanelProperties.ResultButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                win ? playMore : tryAgain;
            rewardWinCardPanelProperties.ResultButton.onClick.RemoveAllListeners();
            rewardWinCardPanelProperties.ResultButton.onClick.AddListener(() => ClosePanel(win, itemType));
        }

        //closes the panel according to the win or lose
        //if the player wins, it adds the new collected item to the content in the panel
        //if the player loses, it loads the current scene
        private void ClosePanel(bool win = true, ItemTypes itemType = ItemTypes.None)
        {
            if (win)
            {
                CollectedItemsPanelManager.Instance.AddNewCollectedItem(rewardWinCardPanelProperties.Text.text,
                    rewardWinCardPanelProperties.Image.sprite, itemType);
                ZonesPanelManager.Instance.MoveToNextZone();
                gameObject.SetActive(false);
            }
            else
            {
                rewardWinCardPanelProperties.ResultButton.onClick.RemoveAllListeners();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}