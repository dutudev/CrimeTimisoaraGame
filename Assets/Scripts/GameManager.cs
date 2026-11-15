using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

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
    private List<string> _knownHints = new List<string>();
    
    
    //murder mystery part
    [SerializeField] 
    private List<Character> characters = new List<Character>();
    [SerializeField]
    private List<AnswerTemplate> answerTemplates = new List<AnswerTemplate>();
    [SerializeField]
    private List<AnswerTemplateLocation> answerTemplatesLoc = new List<AnswerTemplateLocation>();
    [SerializeField] 
    private List<string> killerShit = new List<string>();
    
    private Character _killer;
    private int _killerIndex;
    
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
        SetMurderGame();
        CanvasManager.instance.UpdateLocation("Select Starting Location");
    }

    private void SetMurderGame()
    {
        _killerIndex = Random.Range(0, characters.Count);
        _killer = characters[_killerIndex];

        List<string> randomHints = new List<string>();
        for (int i = 0; i < 4; ++i)
        {
            for (int j = 0; j < 2; ++j)
            {
                randomHints.Add(_killer.characterSo.traits[i]);
            }
        }

        AnswerTemplate ats = new AnswerTemplate();
        int killerIndexy = Random.Range(0, answerTemplates.Count);
        ats.question = answerTemplates[killerIndexy].question;
        string chosenKillerYo = killerShit[Random.Range(0, killerShit.Count)];
        ats.answer = answerTemplates[killerIndexy].answer.Replace("$", chosenKillerYo);
        ats.hint = chosenKillerYo;
        characters[_killerIndex].answer = ats;
        
        for (int i = randomHints.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (randomHints[i], randomHints[j]) = (randomHints[j], randomHints[i]);
        }

        int indexy = 0;
        foreach (var question in answerTemplatesLoc)
        {
            int rand = Random.Range(0, 3);
            int startIndex = characters.FindIndex(character => character.characterSo.located == question.info);
            while ((rand + startIndex) == _killerIndex || characters[rand + startIndex].answer.question != "")
            {
                rand = Random.Range(0, 3);
            }

            AnswerTemplate a = new AnswerTemplate();
            a.question = question.question;
            a.answer = question.answer.Replace("$", randomHints[indexy]);
            a.hint = randomHints[indexy];
            characters[rand + startIndex].answer = a;
            indexy++;
        }

        indexy = 0;
        foreach (var question in answerTemplates)
        {
            int rand = Random.Range(0, characters.Count);
            while (rand == _killerIndex || characters[rand].answer.question != "")
            {
                rand = Random.Range(0, characters.Count);
            }

            AnswerTemplate a = new AnswerTemplate();
            a.question = question.question;
            a.answer = question.answer.Replace("$", randomHints[indexy]);
            a.hint = randomHints[indexy];
            characters[rand].answer = a;
            indexy++;
        }
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
                CanvasManager.instance.HideInterogate();
                break;
            case 1:
                CanvasManager.instance.HideInvestigate();
                break;
            case 2:
                CanvasManager.instance.HideAccuse();
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
                CanvasManager.instance.ShowInterogate(_currentBuilding);
                break;
            case 1:
                CanvasManager.instance.ShowInvestigate(_currentBuilding);
                break;
            case 2:
                CanvasManager.instance.ShowAccuse(_currentBuilding);
                break;
        }
    }

    public List<Character> GetCharactersFromLocation(BuildingInfo info)
    {
        return characters.FindAll(character => character.characterSo.located == info);
    }

    public BuildingInfo GetCurrentLocation()
    {
        return _currentBuilding;
    }

    public bool HaveHint(string search)
    {
        return _knownHints.Contains(search);
    }

    public void AddHint(string hint)
    {
        _knownHints.Add(hint);
        CanvasManager.instance.RewriteHintsList();
    }

    public string GetHints()
    {
        string hints = "";
        for (int i = 0; i < _knownHints.Count; i++)
        {
            hints += "- " + _knownHints[i] + "\n";
        }

        return hints;
    }
}

[System.Serializable]
public class Building
{
    public BuildingInteractable script;
    public GameObject triangle;
}

[System.Serializable]
public class AnswerTemplate
{
    public string question;
    public string answer;
    public string hint;
}

[System.Serializable]
public class AnswerTemplateLocation
{
    public string question;
    public string answer;
    public string hint;
    public BuildingInfo info;
}