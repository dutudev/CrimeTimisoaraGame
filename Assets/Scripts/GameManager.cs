using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private float gameTime;
    
    private float _timer;
    private int _oldTimer;
    private int _currentTool; // 0 - interogate, 1 - investigate, 2 - accuse
    private bool _timerToggle = false;
    private BuildingInfo _currentBuilding = null;
    private Controls _controls;
    
    
    
    //murder mystery part
    [SerializeField] 
    private List<Character> characters = new List<Character>();
    
    private Character _killer;
    
    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        _controls = new Controls();
        _controls.Enable();
        _controls.Player.Next.performed += Next;
        _controls.Player.Next.canceled += Next;
        _controls.Player.Previous.performed += Previous;
        _controls.Player.Previous.canceled += Previous;
    }


    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public void StartGame()
    {
        _timer = gameTime;
        _oldTimer = Mathf.FloorToInt(gameTime);
        _timerToggle = false;
        _currentBuilding = null;
        CanvasManager.instance.UpdateLocation("Select Starting Location");
    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerToggle)
        {
            _timer -= Time.deltaTime; // check here if <0
            if (_oldTimer > Mathf.Ceil(_timer))
            {
                _oldTimer = Mathf.CeilToInt(_timer);
                CanvasManager.instance.UpdateTimer(_oldTimer);
            }
        }
    }

    private void Next(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            ChangeTool(1);
        }
    }

    private void Previous(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            ChangeTool(-1);
        }
    }

    private void ChangeTool(int changeIndex)
    {
        _currentTool += changeIndex;
        if (_currentTool < 0)
        {
            _currentTool = 2;
        }
        _currentTool %= 3;
        
        CanvasManager.instance.ChangeTool(_currentTool);
    }

    public void ExitTool()
    {
        switch (_currentTool)
        {
            case 0:
                break;
            case 1:
                CanvasManager.instance.HideInvestigate();
                break;
            case 2:
                break;
        }
    }

    public string GetCurrentLocationName()
    {
        if (_currentBuilding == null)
        {
            return "";
        }

        return _currentBuilding.buildingName;
    }

    public bool LocationSelected()
    {
        return _currentBuilding != null;
    }

    public void DecreaseTimer(int decrease)
    {
        _timer -= decrease;
        _timer = Mathf.Ceil(_timer);
        _oldTimer = Mathf.RoundToInt(_timer);
        CanvasManager.instance.UpdateTimerBig(_oldTimer);
    }

    public void SetCurrentBuilding(BuildingInfo set)
    {
        if (_currentBuilding == null)
        {
            _timerToggle = true;
        }
        _currentBuilding = set;
        CanvasManager.instance.UpdateLocation(_currentBuilding.buildingName);
    }

    public void UseTool()
    {
        switch (_currentTool)
        {
            case 0:
                break;
            case 1:
                CanvasManager.instance.ShowInvestigate(_currentBuilding);
                break;
            case 2:
                break;
        }
    }
}

[System.Serializable]
public class Building
{
    public BuildingInteractable script;
    public GameObject triangle;
}
