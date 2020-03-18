using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TruckGame : MiniGame
{
    private GameObject _truckGame;
    private TruckGameInit _initialiser;

    private string _gameName = "Truck Game";
    private int _attempts;
    private bool hasWon;
    private int _completedGames;
    private int _answerPart;
    private bool _ansFadeIn;
    private bool _inPosition;
    private bool _IdleStateWaiting;

    private float _fadeTimer;

    //the 2 required answers
    private int _RequiredAns1, _RequiredAns2;


    //runs once at creation - will not be part of a replayed game
    protected override void OnAwake()
    {
        MiniGameManager.instance.GameName = _gameName;

        _truckGame = GameObject.Find("TruckGame");

        //initialiser
        _initialiser = _truckGame.GetComponent<TruckGameInit>();

        //fade in helptexts
        FadeText(_initialiser.HelpTexts, true, 10f);

        //van starts in position
        _inPosition = true;

        base.OnAwake();
    }

    protected override bool OnInitialise()
    {
        //clear any old panels
        ClearAllPanels();


        //populate numbers
        InitialiseNumbers();

        //start with the first answer
        _answerPart = 1;

        //start with a fade in on the answer boxes
        _ansFadeIn = true;

        //drive in
        if (_inPosition == false)
        {
            _inPosition = true;
            //drive in
            _initialiser.Truck.GetComponent<Animator>().Play("DriveIn");
            //lock the state for the animation to run
            _lockStateUpdate = true;
        }
        return base.OnInitialise();
    }

    protected override bool Update()
    {
        //flash the answer box
        FlashAnswerBox();

        //Check for an answer
        return SelectAnswer();

    }

    protected override bool OnExit()
    {
        //drive away
        _initialiser.Truck.GetComponent<Animator>().Play("DriveAway");
        //no longer in position
        _inPosition = false;
        //lock the state for the animation to run
        _lockStateUpdate = true;

        return base.OnExit();   
    }

    private void InitialiseNumbers()
    {
        //get random answer number
        int centre = Random.Range(10, 89);
        _RequiredAns1 = centre - 1;
        _RequiredAns2 = centre + 1;
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
        availableNumbers.Remove(_RequiredAns1);
        availableNumbers.Remove(_RequiredAns2);


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
                textField.text = i == frameAnswer1 ? _RequiredAns1.ToString() : _RequiredAns2.ToString();
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
        FadeText(_initialiser.AnswerFrame, true, 10f);
        FadeText(_initialiser.Frames, true, 10f);

    }

    private void FlashAnswerBox()
    {
        if (_fadeTimer <= 0f)
        {
            _fadeTimer = _initialiser.AnsBoxBlinkTime;

            GameObject frameObject = null;

            if (_answerPart == 1)
            {
                frameObject = _initialiser.Answers[0];
            }
            else if (_answerPart == 2)
            {
                frameObject = _initialiser.Answers[1];
            }

            //get the highlight box
            GameObject highlightbox = frameObject.transform.Find("Highlight").gameObject;
            highlightbox.SetActive(true);

            //fade object
            FadeObject(highlightbox, _initialiser.AnsBoxBlinkTime, _ansFadeIn,0,0.7f);

            //flip the fade for next time
            _ansFadeIn = !_ansFadeIn;
        }
        else
        {
            _fadeTimer -= Time.deltaTime;
        }
    }

    public bool SelectAnswer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //select whats being clicked on the screen
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f))
            {
                GameObject frame;
                //check its a frame on the ground to be selected
                if (hit.transform.CompareTag("Frame"))
                {
                    frame = hit.transform.gameObject;
                    int numberSelected = int.Parse(frame.GetComponentInChildren<TextMeshPro>().text);

                    return CheckAnswer(numberSelected);
                }
            }
        }
        return false;
    }

    private bool CheckAnswer(int ans)
    {
        Debug.Log("You selected number: " + ans.ToString()); // ensure you picked right object

        //get the right answer
        int answer = _answerPart == 1 ? _RequiredAns1 : _RequiredAns2;

        //check selected against answer
        if (ans == answer)
        {
            Debug.Log("You are right!");
            
            //get the frame
            GameObject ansFrame = _answerPart == 1 ? _initialiser.Answers[0] : _initialiser.Answers[1];
            //show answer on answer frame
            ansFrame.GetComponentInChildren<TextMeshPro>().text = answer.ToString();

            //reset the highlight box
            GameObject highlightbox = ansFrame.transform.Find("Highlight").gameObject;
            //Material mat = highlightbox.GetComponent<MeshRenderer>().material;
            //Color col = mat.color;
            //col.a = 0f;
            //mat.color = col;
            highlightbox.SetActive(false);

            //Go to next answer part
            if (_answerPart == 1)
            {
                _answerPart = 2;
                //answer correct, but stay in update for second part
                return false;
            }
            //or complete the game
            else
            {
                Animator anim = _initialiser.Truck.GetComponent<Animator>();
                //reset the game if total runs havent been played
                if (_completedGames >= _initialiser.RequiredCompletions)
                {
                    anim.SetBool("Completed", true);
                    _restartGame = false;
                    //end update loop
                    return true;
                }
                else
                {
                    //reset the game
                    _restartGame = true;
                    //end update loop
                    return true;
                }
            }
        }
        else
        {
            if (++_attempts >= _initialiser.MaxAttempts)
            {
                Debug.Log("Too many attempts - resetting game");
                _attempts = 0;
                //drive off
                Animator anim = _initialiser.Truck.GetComponent<Animator>();
                anim.Play("DriveAway");
                //reset the game
                _restartGame = true;
                return true;
            }
            //got it wrong
            Debug.Log("Stupid Motherfucker...");
            //stay in the game
            return false;
        }   
    }

    public void ClearAllPanels()
    {
        //fade everything out
        FadeText(_initialiser.AnswerFrame, false, 10f);
        FadeText(_initialiser.Frames, false, 10f);

        //centre answer
        _initialiser.AnswerFrame.GetComponentInChildren<TextMeshPro>().text = "";
        //chosen answers
        _initialiser.Answers[0].GetComponentInChildren<TextMeshPro>().text = "";
        _initialiser.Answers[1].GetComponentInChildren<TextMeshPro>().text = "";
        //ground panels
        for (int i = 0; i < _initialiser.Frames.Length; i++)
        {
            _initialiser.Frames[i].GetComponentInChildren<TextMeshPro>().text = "";
        }

        _initialiser.Answers[1].transform.Find("Highlight").gameObject.SetActive(false);
        _initialiser.Answers[0].transform.Find("Highlight").gameObject.SetActive(false);
    }
}
