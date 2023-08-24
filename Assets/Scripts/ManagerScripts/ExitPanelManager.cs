using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ManagerScripts
{
    public class ExitPanelManager : MonoBehaviour
    {
        [SerializeField] private Button exitLeaveButton;
        [SerializeField] private Button exitReturnButton;

        private void OnValidate()
        {
            exitLeaveButton ??= transform.GetChild(1).GetComponent<Button>();
            exitReturnButton ??= transform.GetChild(2).GetComponent<Button>();
        }

        private void Awake()
        {
            RemoveListeners();
            AddListeners();
        }

        private void AddListeners()
        {
            exitLeaveButton.onClick.AddListener(ExitLeaveButtonOnClick);
            exitReturnButton.onClick.AddListener(ExitReturnButtonOnClick);
        }

        private void ExitLeaveButtonOnClick()
        {
            RemoveListeners();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void RemoveListeners()
        {
            exitLeaveButton.onClick.RemoveAllListeners();
            exitReturnButton.onClick.RemoveAllListeners();
        }

        private void ExitReturnButtonOnClick()
        {
            gameObject.SetActive(false);
        }
    }
}