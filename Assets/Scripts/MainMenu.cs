using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject FadeScreen;
    public AudioClip ButtonHighlight;
    public AudioClip ClickSelect;

    public void PlayGame()
    {
        FadeScreen.GetComponent<Animator>().Play("FadeOut");

        StartCoroutine(LoadGameWithDelay());
    }


    IEnumerator LoadGameWithDelay()
    {
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Garden Scene");
    }


    public void ExitGame()
    {
        Application.Quit();
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
