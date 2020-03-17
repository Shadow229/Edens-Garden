using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMiniGame
{
    bool Initialise();
    bool OnUpdate();
    bool Won();
    bool Exit();
}
