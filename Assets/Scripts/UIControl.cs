using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
    public GameObject FadeScreen;
    public AudioClip ButtonHighlight;
    public AudioClip ClickSelect;

    public void GoToMenu()
    {

        FadeScreen.GetComponent<Animator>().Play("FadeOut");

        StartCoroutine(LoadMenuWithDelay());
    }


    IEnumerator LoadMenuWithDelay()
    {
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Main Menu");
    }

    public void ButtonHighlighted()
    {
        MiniGameManager.instance.GetComponent<AudioSource>().PlayOneShot(ButtonHighlight);
    }
    public void ButtonClickSelect()
    {
        MiniGameManager.instance.GetComponent<AudioSource>().PlayOneShot(ClickSelect);
    }
}
