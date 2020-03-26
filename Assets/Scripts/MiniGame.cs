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
    protected bool _exitOut;

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
            MiniGameManager.instance.ResetGameSelection();
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
    protected bool CheckExitTrigger()
    {
            //early out on exit
        if (MiniGameManager.instance.ExitMiniGame)
        {
            _exitOut = true;
            return true;
        }
        return false;
    }


    public void FadeText(GameObject[] Texts, bool FadeIn, float duration, bool clearText = false, float minAlpha = 0, float maxAlpha = 1)
    {
        //show texts
        foreach (GameObject text in Texts)
        {
            FadeText(text, FadeIn, duration, clearText);
        }
    }
    public void FadeText(GameObject Text, bool FadeIn, float duration, bool clearText = false, float minAlpha = 0, float maxAlpha = 1)
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

        if (FadeIn)
        {
            alphaEnd.a = maxAlpha;
            LeanTween.value(Text, a => tmpText.color = a, alphaStart, alphaEnd, duration).setEase(LeanTweenType.easeInCubic).setOnComplete(() => tmpText.color = alphaEnd);
        }
        else
        {
            alphaEnd.a = minAlpha;

            if (clearText)
            {
                LeanTween.value(Text, a => tmpText.color = a, alphaStart, alphaEnd, duration).setEase(LeanTweenType.easeOutCubic).setOnComplete(() => ClearAlphaText(tmpText, alphaEnd));
            }
            else
            {
                LeanTween.value(Text, a => tmpText.color = a, alphaStart, alphaEnd, duration).setEase(LeanTweenType.easeOutCubic).setOnComplete(() => tmpText.color = alphaEnd);
            }
        }
    }
    
    private void ClearAlphaText(TextMeshPro TMP, Color alphaEnd)
    {
        TMP.color = alphaEnd; 
        TMP.text = "";
    }


    public void FadeObjects(GameObject[] objs, float fadeTime, bool fadeIn, bool FadeChildren = false, bool DestroyOnFadeOut = false, float minAlpha = 0, float maxAlpha = 1)
    {
        //show texts
        foreach (GameObject obj in objs)
        {
            FadeObject(obj, fadeTime, fadeIn, FadeChildren, DestroyOnFadeOut, minAlpha, maxAlpha);
        }
    }

    public void FadeObject(GameObject obj, float fadeTime, bool fadeIn, bool FadeChildren = false, bool DestroyOnFadeOut = false, float minAlpha = 0, float maxAlpha = 1)
    {
        List<Material> materials = new List<Material>();

        if (FadeChildren)
        {
            MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mesh in meshes)
            {
                materials.Add(mesh.material);
            }
        }
        else
        {
            //get the mateiral
            materials.Add(obj.GetComponent<MeshRenderer>().material);
        }


        if (materials.Count > 0)
        {
            foreach (Material mat in materials)
            {
                //skip if we're in an object without color
                if (mat.HasProperty("_Color") == false)
                {
                    continue;
                };

                Color alphaStart = mat.color;
                Color alphaEnd = alphaStart;

                if (fadeIn)
                {
                    alphaEnd.a = maxAlpha;
                    LeanTween.value(obj, a => mat.color = a, alphaStart, alphaEnd, fadeTime).setEase(LeanTweenType.easeInQuad).setOnComplete(() => mat.color = alphaEnd);
                }
                else
                {
                    alphaEnd.a = minAlpha;

                    if (DestroyOnFadeOut)
                    {
                        LeanTween.value(obj, a => mat.color = a, alphaStart, alphaEnd, fadeTime).setEase(LeanTweenType.easeOutQuad).setOnComplete(() => GameObject.Destroy(obj));
                    }
                    else
                    {
                        LeanTween.value(obj, a => mat.color = a, alphaStart, alphaEnd, fadeTime).setEase(LeanTweenType.easeOutQuad).setOnComplete(() => mat.color = alphaEnd);
                    }
                }
            } 
        }      
    }
}
