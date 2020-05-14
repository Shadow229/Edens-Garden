using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eden : MonoBehaviour
{
    private Animator anim;
    private AudioSource aud;

    public bool isNarrating { get; private set; }

    public AudioClip Greeting;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();

        //greet the player
        StartCoroutine(PlayGreeting());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator PlayGreeting()
    {
        //narration lock on
        isNarrating = true;

        yield return new WaitForSeconds(2);

        //wave
        anim.Play("Greeting");
        //audio
        float delay = 0.5f;
        aud.clip = Greeting;
        aud.PlayDelayed(delay);

        //narration lock off
        StartCoroutine(WaitAudio(aud.clip.length + delay));
    }

    private IEnumerator WaitAudio(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        isNarrating = false;
    }
}