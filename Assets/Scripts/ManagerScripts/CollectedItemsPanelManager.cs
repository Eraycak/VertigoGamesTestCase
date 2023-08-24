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

        internal void AddNewCollectedItem(string prizeValue, Sprite prizeSprite)
        {
            Transform newPrize = Instantiate(prizePrefab, contentTransform).transform;
            newPrize.GetChild(0).GetComponent<TextMeshProUGUI>().text = prizeValue;
            newPrize.GetChild(1).GetComponent<Image>().sprite = prizeSprite;
        }
    }
}