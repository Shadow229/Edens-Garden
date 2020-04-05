using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eden : MonoBehaviour
{
    private Animator anim;
    private AudioSource aud;

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
        yield return new WaitForSeconds(2);

        //wave
        anim.Play("Greeting");
        //audio
        aud.clip = Greeting;
        aud.PlayDelayed(0.5f);
    }
}