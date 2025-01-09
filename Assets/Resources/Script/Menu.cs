using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject PCredit;
    public void Play()
    {
        SceneManager.LoadScene("Game");
    }
    public void ExitGame()
    {
      
        Application.Quit();
    }
    public void PanelCredits()
    {
        PCredit.SetActive(true);
    }
    public void PanelCreditsOff()
    {
        PCredit.SetActive(false);
    }
}
