using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TruckGame : MiniGame
{
    private GameObject _truckGame;
    private TruckGameInit _initialiser;

    private int _attempts;
    private int _completedGames;

    protected override bool OnInitialise()
    {
        _truckGame = GameObject.Find("TruckGame");

        //initialiser
        _initialiser = _truckGame.GetComponent<TruckGameInit>();

        //fade in helptexts
        FadeText(_initialiser.HelpTexts, true);

        InitialiseNumbers();
      

        return base.OnInitialise();
    }

    protected override bool Update()
    {
        return base.Update();
    }

    protected override bool Win()
    {
        return base.Win();
    }


    public void InitialiseNumbers()
    {
        //get random answer number
        int centre = Random.Range(10, 89);
        int answer1 = centre - 1;
        int answer2 = centre + 1;
        int swingAmt = 10;

        //set answer number
        _initialiser.AnswerFrame.GetComponentInChildren<TextMeshPro>().text = centre.ToString();

        //generate a list of available numbers to choose from
        List<int> availableNumbers = new List<int>();

        int lowerBound = centre - swingAmt;
        int upperBound = centre + swingAmt;

        //add all available numbers to the list
        for (int i = lowerBound; i <= upperBound; i++)
        {
            availableNumbers.Add(i);
        }

        //then remove our current answers
        availableNumbers.Remove(centre);
        availableNumbers.Remove(answer1);
        availableNumbers.Remove(answer2);


        //set the frames//

        //2 different random frames to hold the correct answers
        int frameAnswer1 = Random.Range(0, 10);
        int frameAnswer2;
        do
        {
            frameAnswer2 = Random.Range(0, 10);
        } while (frameAnswer2 == frameAnswer1);


        //set the numbers for the frames on the floor
        for (int i = 0; i < _initialiser.Frames.Length; i++)
        {
            //ref to frame text
            TextMeshPro textField = _initialiser.Frames[i].GetComponentInChildren<TextMeshPro>();

            //check if its an answer frame and asign the correct answer
            if (i == frameAnswer1 || i == frameAnswer2)
            {
                textField.text = i == frameAnswer1 ? answer1.ToString() : answer2.ToString();
            }
            else
            {
                //pick a random number from the available numbers
                int randomNumber = availableNumbers[Random.Range(0, availableNumbers.Count)];

                //assign it to the text
                textField.text = randomNumber.ToString();

                //remove it from the list
                availableNumbers.Remove(randomNumber);
            }
        }

        //fade them all in
        FadeText(_initialiser.AnswerFrame, true);
        FadeText(_initialiser.Frames, true);

    }
}
