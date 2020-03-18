using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckGameInit : MonoBehaviour
{
    [Header("Game Attributes")]
    public int MaxAttempts = 3;
    public float AnsBoxBlinkTime = 2.5f;
    public int RequiredCompletions = 10;

    [Header("TMPro Help Texts")]
    public GameObject[] HelpTexts;

    [Header("The Truck")]
    public GameObject Truck;

    [Header("Center Answer Board")]
    public GameObject AnswerFrame;

    [Header("Answer Frames")]
    public GameObject[] Answers = new GameObject[2];

    [Header("Other Frames")]
    public GameObject[] Frames = new GameObject[10];

}
