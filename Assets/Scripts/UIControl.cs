using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
    public GameObject FadeScreen;

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
}
