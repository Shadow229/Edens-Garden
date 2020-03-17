using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//animation events to start mini games
public class EventsHandler : MonoBehaviour
{
    public void StartTruckGame()
    {
        MiniGameManager.instance.StartMiniGame(new TruckGame());
    }
}
