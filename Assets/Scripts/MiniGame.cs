using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
