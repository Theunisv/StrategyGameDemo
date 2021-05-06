using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FadeTooltip : MonoBehaviour
{
    private Color gameObjectBaseColor;
    
    void Start()
    {
       // gameObjectBaseColor = 
        //gameObject.SetActive(true);
        //LeanTween.alpha(gameObject, 0.1f, 0.1f);
        FadeStart();
    }
    void FadeStart()
    {
        GameObject textFade = GameObject.Find("ErrorText");
        LeanTween.alpha (textFade, 1f, 1f).setEase (LeanTweenType.linear).setOnComplete( FadeFinished );
        LeanTween.alpha (this.gameObject, 1f, 1f).setEase (LeanTweenType.linear).setOnComplete( FadeFinished );
    }
    void FadeFinished()
    {
        GameObject textFade = GameObject.Find("ErrorText");
        LeanTween.alpha(textFade, 0.1f, 1f).setEase(LeanTweenType.linear);
        LeanTween.alpha (this.gameObject, 0.1f, 1f).setEase (LeanTweenType.linear).setOnComplete( DestroyThis );
    }

    void DestroyThis()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        FadeStart();
    }
}
