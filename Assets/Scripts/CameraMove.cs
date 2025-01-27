﻿using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Animator _anim;
    public AudioClip GameSelection;
    public Eden eden;


    public bool SelectableState = true;

    private void Start()
    {
        _anim = GetComponent<Animator>(); 
    }


    private void Update()
    {
        //disable is Eden is narrating
        if (eden.isNarrating)
        {
            return;
        }

        if (SelectableState)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    Debug.Log("You selected the " + hit.transform.name); // check object selection

                    if (hit.transform.CompareTag("MiniGame"))
                    {
                        //get the camera animation from the collider
                        AnimationClip camMove = hit.transform.GetComponent<FocusBox>().CameraFocusAnimation;

                        //cue audio
                        GetComponent<AudioSource>().PlayOneShot(GameSelection);

                        MoveToGame(camMove);
                        SelectableState = false;
                    }
                }
            }
        }
        else
        {
            //currently a place holder for a GUI revert button
            if (Input.GetMouseButtonDown(1))
            {
                //if there is a game set - end it
                if (MiniGameManager.instance.GetMiniGameReference() != null)
                {
                    MiniGameManager.instance.StopMiniGame();
                }
                //otherwise return the camera and reset
                else
                {
                    SelectableState = true;
                    ResetCamera();
                }
            }
        }
    }


    private void MoveToGame(AnimationClip clip)
    { 
        _anim.Play(clip.name, -1, 0);
    }


    public void ResetCamera()
    {
        _anim.SetTrigger("ResetCamera");
    }


}
