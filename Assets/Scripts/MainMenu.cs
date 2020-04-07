using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject FadeScreen;

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
}
