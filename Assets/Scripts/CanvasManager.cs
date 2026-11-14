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
    private TMP_Text timerText, locationText;
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
    public static CanvasManager instance { get; private set; }

    private Action _warningConfirm;
    private bool _hintsShown = false;
    
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
        LeanTween.alphaCanvas(investigateAlpha, 1, 0.5f).setEaseOutExpo();
    }

    public void HideInvestigate()
    {
        LeanTween.cancel(investigateAlpha.gameObject);
        LeanTween.alphaCanvas(investigateAlpha, 0, 0.5f).setEaseOutExpo().setOnComplete(() =>
        {
            investigateAlpha.gameObject.SetActive(false);
        });
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
