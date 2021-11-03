using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseScreen; //Pause Screen

    public void RestartGame ()
    {
        SceneManager.LoadScene(1);
    }
    
    public void Back ()
    {
        PauseScreen.SetActive(false);
    }

    public void QuitGame ()
    {
        //Application.Quit();
        SceneManager.LoadScene(0);
    }
}
