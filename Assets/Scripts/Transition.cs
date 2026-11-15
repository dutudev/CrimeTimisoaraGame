using System;
using UnityEngine;

public class Transition : MonoBehaviour
{
    void Start()
    {
        transform.localScale = new Vector3(1f, 22f, 1f);
        LeanTween.scale(gameObject, new Vector3(1f, 0f, 1f), 0.8f).setEaseOutExpo();
        
    }

    public void TransitionGo(Action callback)
    {
        LeanTween.scale(gameObject, new Vector3(1f,22f, 1f), 0.8f).setEaseOutExpo().setOnComplete(() =>
        {
            callback?.Invoke();
        });
    }
}
