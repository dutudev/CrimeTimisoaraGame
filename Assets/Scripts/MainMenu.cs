using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] 
    private GameObject settingsPanel;
    [SerializeField] 
    private TMP_Dropdown dropdown;
    [SerializeField] 
    private Transition transition;

    private bool _toggledSettings = false;

    private void Start()
    {
        dropdown.ClearOptions();
        dropdown.options = ControllerManager.instance.GetPorts();
    }
    
    public void Play()
    {
        transition.TransitionGo(() =>
        {
            SceneManager.LoadScene("Game");
        });
    }

    public void ToggleSettings()
    {
        _toggledSettings = !_toggledSettings;
        settingsPanel.SetActive(_toggledSettings);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OnDropdownChange(int index)
    {
        ControllerManager.instance.SetSerialPort(dropdown.options[index].text);
    }
}
