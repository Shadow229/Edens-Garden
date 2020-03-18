using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState
{
    AWAKE,
    INIT,
    CORE,
    END,
    COMPLETE,
}

public class MiniGame : IMiniGame
{
    private GameState _currentState = GameState.AWAKE;
    protected bool _restartGame;
    protected bool _lockStateUpdate;

    void IMiniGame.RunMiniGame()
    {
        OnRunMiniGame();
    }

    //main run switch
    protected virtual void OnRunMiniGame()
    {
        //if the states aren't locked to not update
        if (MiniGameManager.instance.GameStateLock == false)
        {
            //run the state switch for the mini game
            switch (_currentState)
            {
                case GameState.AWAKE:
                    OnAwake();
                    break;
                case GameState.INIT:
                    RunInitialise();
                    break;
                case GameState.CORE:
                    RunUpdate();
                    break;
                case GameState.END:
                    RunExit();
                    break;
                case GameState.COMPLETE:
                    CompleteState();
                    break;
                default:
                    break;
            }
        }

    }



    //Awake - unlike the other functions that can loop - this will only run once!

    void IMiniGame.Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        _currentState = GameState.INIT;
        CheckLockState();
    }


    //Initialisation//
    private bool RunInitialise()
    {
        if (OnInitialise() && !MiniGameManager.instance.GameStateForceLoop)
        {
            _currentState = GameState.CORE;
            CheckLockState();
            return true;
        }

        return false;
    }

    bool IMiniGame.Initialise()
    {
        return RunInitialise();
    }

    protected virtual bool OnInitialise()
    {
        return true;
    }

    //Update function//
    private bool RunUpdate()
    {
        if (Update() && !MiniGameManager.instance.GameStateForceLoop)
        {
            _currentState = GameState.END;
            CheckLockState();
            return true;
        }

        return false;
    }

    bool IMiniGame.OnUpdate()
    {
        return RunUpdate();
    }

    protected virtual bool Update()
    {
        return true;
    }

    //Exit function//
    private bool RunExit()
    {
        if (OnExit() && !MiniGameManager.instance.GameStateForceLoop)
        {
            _currentState = GameState.COMPLETE;
            CheckLockState();
            return true;
        }
        return false;
    }

    bool IMiniGame.Exit()
    {
        return RunExit();
    }

    protected virtual bool OnExit()
    {
        //allow mini game selection again
        Camera.main.GetComponentInParent<CameraMove>().SelectableState = true;
        return true;
    }

    //Complete state
    private void CompleteState()
    {
        if (_restartGame)
        {
            _currentState = GameState.INIT;
        }
        else
        {
            MiniGameManager.instance.GameName = "";
            MiniGameManager.instance.StopMiniGame();
        }
    }

    private void CheckLockState()
    {
        if (_lockStateUpdate)
        {
            MiniGameManager.instance.LockGameState();
            _lockStateUpdate = false;
        }
    }



    //Win function//
    bool IMiniGame.Won()
    {
        return Win();
    }

    protected virtual bool Win()
    {
        OnExit();

        return true;
    }


    //Additional shared functions
    public void FadeText(GameObject[] Texts, bool FadeIn, float duration)
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

            LeanTween.value(text, a => tmpText.color = a, alphaStart, alphaEnd, duration).setEase(LeanTweenType.easeOutElastic);
        }
    }
    public void FadeText(GameObject Text, bool FadeIn, float duration)
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

        LeanTween.value(Text, a => tmpText.color = a, alphaStart, alphaEnd, duration).setEase(LeanTweenType.easeOutElastic);
    }


    public void FadeObject(GameObject obj, float fadeTime, bool fadeIn, float minAlpha = 0, float maxAlpha = 1)
    {
        //get the mateiral
        Material mat = obj.GetComponent<MeshRenderer>().material;

        Color alphaStart = mat.color;
        Color alphaEnd = alphaStart;

        if (fadeIn)
        {
            alphaEnd.a = maxAlpha;
            LeanTween.value(obj, a => mat.color = a, alphaStart, alphaEnd, fadeTime).setEase(LeanTweenType.easeInQuad);
        }
        else
        {
            alphaEnd.a = minAlpha;
            LeanTween.value(obj, a => mat.color = a, alphaStart, alphaEnd, fadeTime).setEase(LeanTweenType.easeOutQuad);
        }
    }


}
