using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        exitLeaveButton.onClick.RemoveAllListeners();
        exitLeaveButton.onClick.AddListener(ExitLeaveButtonOnClick);
        exitReturnButton.onClick.RemoveAllListeners();
        exitReturnButton.onClick.AddListener(ExitReturnButtonOnClick);
    }
    
    private void ExitLeaveButtonOnClick()
    {
        exitLeaveButton.onClick.RemoveAllListeners();
        exitReturnButton.onClick.RemoveAllListeners();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void ExitReturnButtonOnClick()
    {
        gameObject.SetActive(false);
    }
}
