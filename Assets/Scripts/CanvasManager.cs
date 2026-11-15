using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] 
    private List<RectTransform> rectNotClick = new List<RectTransform>();
    [SerializeField] 
    private GameObject warningObject, hintsPanel;
    [SerializeField] 
    private TMP_Text timerText, locationText, hintsText;
    [SerializeField]
    private CanvasGroup locationTextAlpha;

    [Header("Toolbar")] 
    [SerializeField]
    private Image toolImage;
    [SerializeField] 
    private List<Sprite> toolImages = new List<Sprite>();
    [SerializeField]
    private TMP_Text toolTitle, toolDesc;
    [SerializeField] 
    private List<string> toolTitles, toolDescs;

    [Header("Investigate")] 
    [SerializeField]
    private CanvasGroup investigateAlpha;
    [SerializeField] 
    private TMP_Text investigateTitle, investigateSmalldesc, investigateDesc;
    
    [Header("Interogate")] 
    [SerializeField]
    private CanvasGroup interogateAlpha;
    [SerializeField] 
    private TMP_Text interogateTitle, interogatePers1, interogatePers2, interogatePers3, askText, ansText;
    [SerializeField]
    private Image interogatePers1Img, interogatePers2Img, interogatePers3Img;
    [SerializeField] 
    private CanvasGroup interogatePers1Alpha, interogatePers2Alpha, interogatePers3Alpha, askTextAlpha, ansTextAlpha;
    
    [Header("Accuse")] 
    [SerializeField]
    private CanvasGroup accuseAlpha;
    [SerializeField] 
    private TMP_Text accuseTitle, accusePers1, accusePers2, accusePers3;
    [SerializeField]
    private Image accusePers1Img, accusePers2Img, accusePers3Img;
    [SerializeField] 
    private CanvasGroup accusePers1Alpha, accusePers2Alpha, accusePers3Alpha;
    public static CanvasManager instance { get; private set; }

    private Action _warningConfirm;
    private bool _hintsShown = false;
    private bool _currentlyAsking = false;
    
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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public bool IsCursorOnUI(Vector2 mousePos)
    {
        foreach (var recty in rectNotClick)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(recty, mousePos, null) && recty.gameObject.activeInHierarchy)
            {
                return true;
            }
        }

        return false;
    }

    public void ShowWarning(int time, Action callback)
    {
        //set text
        warningObject.transform.localScale = Vector3.zero;
        warningObject.SetActive(true);
        _warningConfirm = callback;
        
        LeanTween.cancel(warningObject);
        LeanTween.scale(warningObject, Vector3.one, 0.5f).setEaseOutExpo();
    }

    public void ConfirmWarning()
    {
        LeanTween.cancel(warningObject);
        LeanTween.scale(warningObject, Vector3.zero, 0.5f).setEaseOutExpo().setOnComplete(() =>
        {
            warningObject.SetActive(false);
        });
        _warningConfirm?.Invoke();
        _warningConfirm = null;
    }

    public void CloseWarning()
    {
        LeanTween.cancel(warningObject);
        LeanTween.scale(warningObject, Vector3.zero, 0.5f).setEaseOutExpo().setOnComplete(() =>
        {
            warningObject.SetActive(false);
        });
    }

    public void ToggleHints()
    {
        if (_hintsShown)
        {
            _hintsShown = false;
            HideHints();
        }
        else
        {
            _hintsShown = true;
            ShowHints();
        }
        
    }

    public void ShowHints()
    {
        LeanTween.cancel(hintsPanel);
        LeanTween.moveLocalX(hintsPanel, 615, 0.5f).setEaseOutExpo();
    }

    public void HideHints()
    {
        LeanTween.cancel(hintsPanel);
        LeanTween.moveLocalX(hintsPanel, 960, 0.5f).setEaseOutExpo();
    }

    public void UpdateTimer(int time)
    {
        timerText.text = (time / 60).ToString() + " : " + (time % 60).ToString("D2");
        LeanTween.cancel(timerText.gameObject);
        timerText.transform.localScale = 1.1f * Vector3.one;
        LeanTween.scale(timerText.gameObject, Vector3.one, 0.3f).setEaseOutExpo();
    }
    
    public void UpdateTimerBig(int time)
    {
        timerText.text = (time / 60).ToString() + " : " + (time % 60).ToString("D2");
        LeanTween.cancel(timerText.gameObject);
        timerText.transform.localScale = 1.3f * Vector3.one;
        LeanTween.scale(timerText.gameObject, Vector3.one, 0.5f).setEaseOutExpo();
    }

    public void UpdateLocation(string text)
    {
        LeanTween.cancel(locationText.gameObject);
        locationText.text = text;
        locationTextAlpha.alpha = 0;
        locationText.transform.localPosition = new Vector3(locationText.transform.localPosition.x, -600f,
            locationText.transform.localPosition.z);
        LeanTween.moveLocalY(locationText.gameObject, -477.8f, .8f).setEaseOutExpo();
        LeanTween.alphaCanvas(locationTextAlpha, 1f, .8f).setEaseOutExpo();
    }

    public void ChangeTool(int index)
    {
        LeanTween.cancel(toolImage.gameObject);
        toolImage.sprite = toolImages[index];
        toolTitle.text = toolTitles[index];
        toolDesc.text = toolDescs[index];
        toolImage.rectTransform.localScale = 1.1f * Vector3.one;
        LeanTween.scale(toolImage.rectTransform, Vector3.one, .5f).setEaseOutExpo();
    }

    public void ShowInvestigate(BuildingInfo info)
    {
        investigateAlpha.gameObject.SetActive(true);
        LeanTween.cancel(investigateAlpha.gameObject);
        investigateAlpha.alpha = 0;
        investigateTitle.text = info.buildingName;
        investigateSmalldesc.text = info.buildingSmallDesc;
        investigateDesc.text = info.buildingDesctription;
        LeanTween.alphaCanvas(investigateAlpha, 1, 0.5f).setEaseOutExpo().setOnComplete(() =>
        {
            investigateAlpha.alpha = 1f;
        });
    }

    public void HideInvestigate()
    {
        LeanTween.cancel(investigateAlpha.gameObject);
        LeanTween.alphaCanvas(investigateAlpha, 0, 0.5f).setEaseOutExpo().setOnComplete(() =>
        {
            investigateAlpha.gameObject.SetActive(false);
        });
    }
    
    public void ShowInterogate(BuildingInfo info)
    {
        interogateAlpha.gameObject.SetActive(true);
        LeanTween.cancel(interogateAlpha.gameObject);
        interogateAlpha.alpha = 0;
        interogateTitle.text = info.buildingName;
        List<Character> characters = GameManager.instance.GetCharactersFromLocation(info);
        interogatePers1.text = characters[0].characterSo.characterName;
        interogatePers2.text = characters[1].characterSo.characterName;
        interogatePers3.text = characters[2].characterSo.characterName;
        interogatePers1Img.sprite = characters[0].characterSo.characterSprite;
        interogatePers2Img.sprite = characters[1].characterSo.characterSprite;
        interogatePers3Img.sprite = characters[2].characterSo.characterSprite;
        LeanTween.alphaCanvas(interogateAlpha, 1, 1f).setEaseOutExpo().setOnComplete(() =>
        {
            interogateAlpha.alpha = 1f;
        });
    }

    public void HideInterogate()
    {
        if (_currentlyAsking)
        {
            _currentlyAsking = false;
            EndInterogate();
        }
        else
        {
            LeanTween.cancel(interogateAlpha.gameObject);
            LeanTween.alphaCanvas(interogateAlpha, 0, 0.5f).setEaseOutExpo().setOnComplete(() =>
            {
                interogateAlpha.gameObject.SetActive(false);
            });  
        }
        
    }
    
    public void ShowAccuse(BuildingInfo info)
    {
        accuseAlpha.gameObject.SetActive(true);
        LeanTween.cancel(accuseAlpha.gameObject);
        accuseAlpha.alpha = 0;
        accuseTitle.text = info.buildingName;
        List<Character> characters = GameManager.instance.GetCharactersFromLocation(info);
        accusePers1.text = characters[0].characterSo.characterName;
        accusePers2.text = characters[1].characterSo.characterName;
        accusePers3.text = characters[2].characterSo.characterName;
        accusePers1Img.sprite = characters[0].characterSo.characterSprite;
        accusePers2Img.sprite = characters[1].characterSo.characterSprite;
        accusePers3Img.sprite = characters[2].characterSo.characterSprite;
        LeanTween.alphaCanvas(accuseAlpha, 1, 1f).setEaseOutExpo().setOnComplete(() =>
        {
            accuseAlpha.alpha = 1f;
        });
    }

    public void HideAccuse()
    {

        LeanTween.cancel(accuseAlpha.gameObject);
        LeanTween.alphaCanvas(accuseAlpha, 0, 0.5f).setEaseOutExpo().setOnComplete(() => { accuseAlpha.gameObject.SetActive(false); });  
        
    }

    public bool IsCurrentlyAsking()
    {
        return _currentlyAsking;
    }

    public void Interrogate(int index)
    {
        if (_currentlyAsking)
        {
            return;
        }
        
        _currentlyAsking = true;
        bool haveHint = false;
        
        List<Character> characters = GameManager.instance.GetCharactersFromLocation(GameManager.instance.GetCurrentLocation());
        if (!GameManager.instance.HaveHint(characters[index].answer.hint) && characters[index].answer.hint != "")
        {
            GameManager.instance.AddHint(characters[index].answer.hint);
            haveHint = true;
        }
        LeanTween.cancel(interogatePers1Img.gameObject);
        LeanTween.cancel(interogatePers2Img.gameObject);
        LeanTween.cancel(interogatePers3Img.gameObject);
        LeanTween.cancel(askText.gameObject);
        LeanTween.cancel(ansText.gameObject);
        askText.text = characters[index].answer.question;
        string anss = characters[index].answer.answer;
        string hintt = characters[index].answer.hint;
        if (haveHint)
        {
            ansText.text = anss.Insert(anss.IndexOf(hintt) + hintt.Length, "</color>").Insert(anss.IndexOf(hintt), "<color=\"yellow\">");
        }
        else
        {
            ansText.text = anss;
        }
        LeanTween.alphaCanvas(askTextAlpha, 1f, 0.6f).setEaseOutExpo();
        LeanTween.alphaCanvas(ansTextAlpha, 1f, 0.6f).setEaseOutExpo();
        switch (index)
        {
            case 0:
                LeanTween.moveLocalX(interogatePers1Img.gameObject, 0, 0.5f).setEaseOutExpo();
                LeanTween.alphaCanvas(interogatePers2Alpha, 0f, 0.4f).setEaseOutExpo();
                LeanTween.alphaCanvas(interogatePers3Alpha, 0f, 0.4f).setEaseOutExpo();
                break;
            case 1:
                LeanTween.moveLocalX(interogatePers2Img.gameObject, 0, 0.5f).setEaseOutExpo();
                LeanTween.alphaCanvas(interogatePers1Alpha, 0f, 0.4f).setEaseOutExpo();
                LeanTween.alphaCanvas(interogatePers3Alpha, 0f, 0.4f).setEaseOutExpo();
                break;
            case 2:
                LeanTween.moveLocalX(interogatePers3Img.gameObject, 0, 0.5f).setEaseOutExpo();
                LeanTween.alphaCanvas(interogatePers1Alpha, 0f, 0.4f).setEaseOutExpo();
                LeanTween.alphaCanvas(interogatePers2Alpha, 0f, 0.4f).setEaseOutExpo();
                break;
        }
    }

    public void EndInterogate()
    {
        LeanTween.cancel(interogatePers1Img.gameObject);
        LeanTween.cancel(interogatePers2Img.gameObject);
        LeanTween.cancel(interogatePers3Img.gameObject);
        LeanTween.cancel(askText.gameObject);
        LeanTween.cancel(ansText.gameObject);
        LeanTween.alphaCanvas(askTextAlpha, 0f, 0.6f).setEaseOutExpo();
        LeanTween.alphaCanvas(ansTextAlpha, 0f, 0.6f).setEaseOutExpo();
        LeanTween.moveLocalX(interogatePers1Img.gameObject, -500, 0.5f).setEaseOutExpo();
        LeanTween.moveLocalX(interogatePers2Img.gameObject, 0, 0.5f).setEaseOutExpo();
        LeanTween.moveLocalX(interogatePers3Img.gameObject, 500, 0.5f).setEaseOutExpo();
        LeanTween.alphaCanvas(interogatePers1Alpha, 1f, 0.6f).setEaseOutExpo();
        LeanTween.alphaCanvas(interogatePers2Alpha, 1f, 0.6f).setEaseOutExpo();
        LeanTween.alphaCanvas(interogatePers3Alpha, 1f, 0.6f).setEaseOutExpo();
    }


    public void RewriteHintsList()
    {
        hintsText.text = GameManager.instance.GetHints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
