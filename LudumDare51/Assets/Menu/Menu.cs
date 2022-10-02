using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject imgButtonPlay;
    [SerializeField] GameObject imgButtonPlayHover;
    [SerializeField] GameObject imgButtonQuit;
    [SerializeField] GameObject imgButtonQuitHover;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenu();
        }
    }

    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OnMousePlayHover()
    {
        imgButtonPlay.SetActive(false);
        imgButtonPlayHover.SetActive(true);
    }

    public void onMousePlayExit()
    {
        imgButtonPlay.SetActive(true);
        imgButtonPlayHover.SetActive(false);
    }

    public void OnMouseQuitHover()
    {
        imgButtonQuit.SetActive(false);
        imgButtonQuitHover.SetActive(true);
    }

    public void onMouseQuitExit()
    {
        imgButtonQuit.SetActive(true);
        imgButtonQuitHover.SetActive(false);
    }
}
