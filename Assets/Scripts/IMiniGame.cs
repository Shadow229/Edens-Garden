using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMiniGame
{
    void RunMiniGame();
    void Awake();
    bool Initialise();
    bool OnUpdate();
    bool Won();
    bool Exit();
}
