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

        //finds and assigns the exit buttons if they are not assigned
        private void OnValidate()
        {
            exitLeaveButton ??= transform.GetChild(1).GetComponent<Button>();
            exitReturnButton ??= transform.GetChild(2).GetComponent<Button>();
        }

        //removes the listeners from the buttons and adds them again
        //this is done to prevent the multiple calls of the listeners
        private void Awake()
        {
            RemoveListeners();
            AddListeners();
        }

        //adds the listeners to the buttons
        private void AddListeners()
        {
            exitLeaveButton.onClick.AddListener(ExitLeaveButtonOnClick);
            exitReturnButton.onClick.AddListener(ExitReturnButtonOnClick);
        }

        //loads the main menu scene and removes the listeners from the buttons
        //if the player wants to leave the game
        private void ExitLeaveButtonOnClick()
        {
            RemoveListeners();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        //removes the listeners from the buttons
        private void RemoveListeners()
        {
            exitLeaveButton.onClick.RemoveAllListeners();
            exitReturnButton.onClick.RemoveAllListeners();
        }

        //deactivates the exit panel if the player wants to return to the game
        private void ExitReturnButtonOnClick()
        {
            gameObject.SetActive(false);
        }
    }
}