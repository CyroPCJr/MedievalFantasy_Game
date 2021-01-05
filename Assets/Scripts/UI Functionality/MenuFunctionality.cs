using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuFunctionality : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Main_Scene");
    }

    public void ExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
