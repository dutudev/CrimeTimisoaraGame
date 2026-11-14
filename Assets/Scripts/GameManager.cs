using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private float gameTime;
    
    private float _timer;
    private int _oldTimer;
    private bool _timerToggle = false;
    private BuildingInfo _currentBuilding = null;
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

    public string GetCurrentLocationName()
    {
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
        _currentBuilding = set;
    }
}

[System.Serializable]
public class Building
{
    public BuildingInteractable script;
    public GameObject triangle;
}
