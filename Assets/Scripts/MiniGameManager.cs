using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script to start up and update all mini games
public class MiniGameManager : MonoBehaviour
{        
    public static MiniGameManager instance = null;

    private IMiniGame _minigame;
    private bool _run = false;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    public void StartMiniGame(IMiniGame a_MG)
    {
        _minigame = a_MG;
        _minigame.Initialise();
        _run = true;
    }

    public void StopMiniGame()
    {
        _run = false;
        _minigame.Exit();
        _minigame = null;
    }

    private void Update()
    {
        if (_run)
        {
            _minigame.OnUpdate();
        }

    }
}
