using System;
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

        private void OnValidate()
        {
            rewardWinCardPanelProperties.ResultButton ??= transform.GetChild(2).GetComponent<Button>();
        }

        internal void SetRewardWinCardPanelProperties(Sprite rewardWinCardPanelSprite, string rewardWinCardPanelText,
            bool win = true)
        {
            rewardWinCardPanelProperties.Image.sprite = rewardWinCardPanelSprite;
            rewardWinCardPanelProperties.Text.text = rewardWinCardPanelText;
            rewardWinCardPanelProperties.ResultText.text =
                win ? congratsYouWonPlayMoreToWınMore : sorryYouLostTryAgainToWın;
            rewardWinCardPanelProperties.ResultButton.image.sprite = win
                ? rewardWinCardPanelProperties.RewardWinCardPanelResultButtonWinSprite
                : rewardWinCardPanelProperties.RewardWinCardPanelResultButtonLoseSprite;
            rewardWinCardPanelProperties.ResultButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                win ? playMore : tryAgain;
            rewardWinCardPanelProperties.ResultButton.onClick.RemoveAllListeners();
            rewardWinCardPanelProperties.ResultButton.onClick.AddListener(() => ClosePanel(win));
        }

        private void ClosePanel(bool win = true)
        {
            if (win)
            {
                CollectedItemsPanelManager.Instance.AddNewCollectedItem(rewardWinCardPanelProperties.Text.text,
                    rewardWinCardPanelProperties.Image.sprite);
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