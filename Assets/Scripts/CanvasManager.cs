using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] 
    private List<RectTransform> rectNotClick = new List<RectTransform>();
    [SerializeField] 
    private GameObject warningObject, hintsPanel;
    [SerializeField] 
    private TMP_Text timerText, locationText;
    public static CanvasManager instance { get; private set; }

    private Action _warningConfirm;
    private bool _hintsShown = false;
    private CanvasGroup _locationTextAlpha;
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
        _locationTextAlpha = locationText.gameObject.GetComponent<CanvasGroup>();
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
            if (RectTransformUtility.RectangleContainsScreenPoint(recty, mousePos, null) && recty.gameObject.activeSelf)
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
        _locationTextAlpha.alpha = 0;
        locationText.transform.localPosition = new Vector3(locationText.transform.localPosition.x, -600f,
            locationText.transform.localPosition.z);
        LeanTween.moveLocalX(locationText.gameObject, -477.8f, .8f).setEaseOutExpo();
        LeanTween.alphaCanvas(_locationTextAlpha, 1f, .8f).setEaseOutExpo();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
