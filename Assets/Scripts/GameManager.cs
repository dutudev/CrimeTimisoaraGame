using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    // Choose the killer from the characters available for this game.
    if (characters.Count == 0)
        return;

    _killerIndex = Random.Range(0, characters.Count);
    _killer = characters[_killerIndex];

    List<string> randomHints = new List<string>();
    for (int i = 0; i < 4; ++i)
    {
        for (int j = 0; j < 3; ++j)
            randomHints.Add(_killer.characterSo.traits[i]);
    }

    // Shuffle the hints.
    for (int i = randomHints.Count - 1; i > 0; i--)
    {
        int j = Random.Range(0, i + 1);
        (randomHints[i], randomHints[j]) = (randomHints[j], randomHints[i]);
    }

    AnswerTemplate killerAnswer = new AnswerTemplate();
    int templateIndex = Random.Range(0, answerTemplates.Count);
    killerAnswer.question = answerTemplates[templateIndex].question;

    string killerHint = killerShit[Random.Range(0, killerShit.Count)];
    killerAnswer.answer = answerTemplates[templateIndex].answer.Replace("$", killerHint);
    killerAnswer.hint = killerHint;
    _killer.answer = killerAnswer;

    int hintIndex = 0;

    // Assign location questions only to eligible characters at that location.
    foreach (var question in answerTemplatesLoc)
    {
        List<Character> eligible = characters.FindAll(character =>
            character != _killer &&
            character.answer.question == "" &&
            character.characterSo.located == question.info);

        if (eligible.Count == 0)
            continue;

        Character chosen = eligible[Random.Range(0, eligible.Count)];
        AnswerTemplate anst = new AnswerTemplate();
        anst.question = question.question;
        anst.answer = question.answer;
        chosen.answer = MakeAnswer(anst, randomHints[hintIndex++]);
    }

    // Assign remaining questions only to characters still eligible.
    foreach (var question in answerTemplates)
    {
        List<Character> eligible = characters.FindAll(character =>
            character != _killer &&
            character.answer.question == "");

        if (eligible.Count == 0)
            break;

        Character chosen = eligible[Random.Range(0, eligible.Count)];
        chosen.answer = MakeAnswer(question, randomHints[hintIndex++]);
    }
    
    foreach (var character in characters)
    {
        if (character != _killer && character.answer.question == "")
        {
            var question = answerTemplates[Random.Range(0, answerTemplates.Count)];

            character.answer = new AnswerTemplate
            {
                question = question.question,
                answer = "i dont know",
                hint = ""
            };
        }
    }

}

    public string GetKillerName()
    {
        return _killer.characterSo.characterName;
    }

private AnswerTemplate MakeAnswer(AnswerTemplate question, string hint)
{
    return new AnswerTemplate
    {
        question = question.question,
        answer = question.answer.Replace("$", hint),
        hint = hint
    };
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
            if (_timer <= 0)
            {
                EndGame(false);
            }
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

    public void ChangeToolDirect(int index)
    {
        _currentTool = index;
        CanvasManager.instance.ChangeTool(_currentTool);
    }

    public void ExitTool(bool skipAsk)
    {
        switch (_currentTool)
        {
            case 0:
                CanvasManager.instance.HideInterogate(skipAsk);
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

    public void EndGame(bool win)
    {
        if (win)
        {
            SceneManager.LoadScene("Win");
        }
        else
        {
            SceneManager.LoadScene("Lose");
        }
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