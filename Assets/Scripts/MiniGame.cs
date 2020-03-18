using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGame : IMiniGame
{
    bool IMiniGame.Initialise()
    {
        return OnInitialise();
    }

    protected virtual bool OnInitialise()
    {
        return true;
    }

    bool IMiniGame.OnUpdate()
    {
        return Update();
    }

    protected virtual bool Update()
    {
        return true;
    }

    bool IMiniGame.Won()
    {
        return Win();
    }

    protected virtual bool Win()
    {
        return true;
    }

    public bool Exit()
    {
        return OnExit();
    }

    protected virtual bool OnExit()
    {
        return true;
    }



    public void FadeText(GameObject[] Texts, bool FadeIn)
    {
        //show texts
        foreach (GameObject text in Texts)
        {
            TextMeshPro tmpText = text.GetComponent<TextMeshPro>();

            if (tmpText == null)
            {
                tmpText = text.GetComponentInChildren<TextMeshPro>();
            }

            if (tmpText == null)
            {
                return;
            }

            Color alphaStart = tmpText.color;
            Color alphaEnd = alphaStart;

            alphaEnd.a = FadeIn ? 1 : 0;

            LeanTween.value(text, a => tmpText.color = a, alphaStart, alphaEnd, 10f).setEase(LeanTweenType.easeOutElastic);
        }
    }
    public void FadeText(GameObject Text, bool FadeIn)
    {
        //show texts
        TextMeshPro tmpText = Text.GetComponent<TextMeshPro>();

        if (tmpText == null)
        {
            tmpText = Text.GetComponentInChildren<TextMeshPro>();
        }

        if (tmpText == null)
        {
            return;
        }

        Color alphaStart = tmpText.color;
        Color alphaEnd = alphaStart;

        alphaEnd.a = FadeIn ? 1 : 0;

        LeanTween.value(Text, a => tmpText.color = a, alphaStart, alphaEnd, 10f).setEase(LeanTweenType.easeOutElastic);
    }
}
