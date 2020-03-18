using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckAnimEvents : MonoBehaviour
{
    public void UnlockGameState()
    {
        MiniGameManager.instance.UnlockGameState();
    }
}
