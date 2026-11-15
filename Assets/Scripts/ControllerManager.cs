using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.IO.Ports;
using TMPro;
using UnityEngine.UIElements;

public class ControllerManager : MonoBehaviour
{
    private string _comPort;
    [SerializeField] private SerialPort serial;
    [SerializeField] private List<BuildingSerial> bInfos = new List<BuildingSerial>();
    public static ControllerManager instance { private set; get; }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetSerialPort(string port)
    {
        _comPort = port;
        if (serial != null)
        {
            if (serial.IsOpen)
            {
                serial.Close();
            }
        }
        if (_comPort != String.Empty)
        {
            serial = new SerialPort(_comPort, 9600);
            serial.Open();
        }
    }

    public List<TMP_Dropdown.OptionData> GetPorts()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        options.Clear();
        foreach (var port in SerialPort.GetPortNames())
        {
            var data = new TMP_Dropdown.OptionData();
            data.text = port;
                //print(port);
            options.Add(data);
        }

        return options;
    }

    public void OnSceneChange(Scene scene1, Scene scene2)
    {
        if (scene2.name == "Game")
        {
            ControllerManagerSetter setter = GameObject.FindWithTag("Setter").GetComponent<ControllerManagerSetter>();
            bInfos = setter.GetList();
        }
        else
        {
            bInfos.Clear();
        }
    }

    public void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    public void Update()
    {
        string input = string.Empty;
        if (serial != null)
        {


            if (serial.IsOpen && serial.BytesToRead > 0)
            {
                try
                {
                    input = serial.ReadLine().Trim();
                }
                catch (System.TimeoutException)
                {

                }
            }
        }

        if (input != string.Empty && input.Length >= 3)
        {
            if (input[0] == 'A')
            {
                switch (input[2])
                {
                    case '1':
                        GameManager.instance.ChangeToolDirect(0);
                        break;
                    case '2':
                        GameManager.instance.ChangeToolDirect(2);
                        break;
                    case '3':
                        GameManager.instance.ChangeToolDirect(1);
                        break;
                }
                Debug.Log(input[2]);

                BuildingInteractable interactable = new BuildingInteractable();
                switch (input[1])
                {
                    case '1':
                        GameManager.instance.SetCurrentBuilding(bInfos[0].info);
                        interactable = bInfos[0].interactable;
                        break;
                    case '2':
                        GameManager.instance.SetCurrentBuilding(bInfos[1].info);
                        interactable = bInfos[1].interactable;
                        break;
                    case '3':
                        GameManager.instance.SetCurrentBuilding(bInfos[2].info);
                        interactable = bInfos[2].interactable;
                        break;
                    case '4':
                        GameManager.instance.SetCurrentBuilding(bInfos[3].info);
                        interactable = bInfos[3].interactable;
                        break;
                    case '5':
                        GameManager.instance.SetCurrentBuilding(bInfos[4].info);
                        interactable = bInfos[4].interactable;
                        break;
                }
                
                
                if (interactable.GetBuildingName() == GameManager.instance.GetCurrentLocationName())
                {
                    //check here tool and act on it
                    interactable.SnapToBuilding();
                    GameManager.instance.UseTool();
                }
                else
                {
                    interactable.ChangeBuilding(false);
                }
                
                GameManager.instance.UseTool();
            }
            else
            {
                GameManager.instance.ExitTool(true);
            }
        }
    }
}

[System.Serializable]
public class BuildingSerial
{
    public BuildingInteractable interactable;
    public BuildingInfo info;
}
