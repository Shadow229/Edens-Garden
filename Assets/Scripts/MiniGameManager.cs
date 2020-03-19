using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script to start up and update all mini games
public class MiniGameManager : MonoBehaviour
{        
    public static MiniGameManager instance = null;
    public string GameName = "";

    //stops gamestate from progressing while still ticking - all actions in current state go on hold
    public bool GameStateLock { get; private set; }
    
    //stops gamestate from progressing while still ticking - all actions in state are updated every tick
    public bool GameStateForceLoop { get; private set; }

    private IMiniGame _minigame;
    private bool _run = false;

    public IMiniGame GetMiniGameReference() { return _minigame; }
    public void LockGameState() { GameStateLock = true; }
    public void UnlockGameState() { GameStateLock = false; }

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    public void StartMiniGame(IMiniGame a_MG)
    {
        _minigame = a_MG;

        if (_minigame != null)
        {
           _run = true;
        }

    }

    public void StopMiniGame()
    {
        _run = false;
        _minigame = null;

        //return camera
        CameraMove camMove = Camera.main.GetComponentInParent<CameraMove>();
        camMove.ResetCamera();
        camMove.SelectableState = true;
    }

    private void Update()
    {
        if (_run)
        {
            _minigame.RunMiniGame();
        }

    }
}
