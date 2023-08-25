using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ManagerScripts
{
    public class CollectedItemsPanelManager : MonoBehaviour
    {
        [SerializeField] private Button exitButton;
        [SerializeField] private GameObject exitPanel;
        [SerializeField] private Transform contentTransform;
        [SerializeField] private GameObject prizePrefab;
        private static CollectedItemsPanelManager _instance;

        public static CollectedItemsPanelManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CollectedItemsPanelManager>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("CollectedItemsPanelManager");
                        _instance = obj.AddComponent<CollectedItemsPanelManager>();
                    }
                }

                return _instance;
            }
        }

        //finds and assigns the exit button if it is not assigned
        private void OnValidate()
        {
            exitButton ??= transform.GetChild(1).GetComponent<Button>();
        }

        private void Awake()
        {
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(ExitButtonOnClick);
        }

        private void ExitButtonOnClick()
        {
            exitPanel.SetActive(true);
        }

        //adds the new collected item to the content in the panel
        internal void AddNewCollectedItem(string prizeValue, Sprite prizeSprite, ItemTypes itemType = ItemTypes.None)
        {
            if (SearchForAlreadyAddedItem(prizeValue, itemType)) return;

            SetNewCollectedItemProperties(prizeValue, prizeSprite, itemType);
        }

        //searches for the already added item in the content
        //if it finds, it adds the new prize value to the existing one
        private bool SearchForAlreadyAddedItem(string prizeValue, ItemTypes itemType)
        {
            for (int i = 0; i < contentTransform.childCount; i++)
            {
                if (contentTransform.GetChild(i).name.Equals(itemType.ToString()))
                {
                    TextMeshProUGUI tmpText = contentTransform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();
                    if (int.TryParse(tmpText.text, out int tmpInt))
                    {
                        tmpInt += int.Parse(prizeValue);
                        tmpText.text = tmpInt.ToString();
                    }

                    return true;
                }
            }

            return false;
        }

        //sets the properties of the new collected item
        private void SetNewCollectedItemProperties(string prizeValue, Sprite prizeSprite, ItemTypes itemType)
        {
            Transform newPrize = Instantiate(prizePrefab, contentTransform).transform;
            newPrize.GetChild(0).GetComponent<TextMeshProUGUI>().text = prizeValue;
            newPrize.GetChild(1).GetComponent<Image>().sprite = prizeSprite;
            newPrize.name = itemType.ToString();
        }

        //disables the exit button when the player is in the bronze zone or revolver is spinning
        internal void DisableButton()
        {
            exitButton.interactable = false;
        }
        
        //enables the exit button when the player is in the silver or gold zone or revolver is not spinning
        internal void EnableButton()
        {
            exitButton.interactable = true;
        }
    }
}