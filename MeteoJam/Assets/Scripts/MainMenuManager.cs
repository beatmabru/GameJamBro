using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour
{

    private bool[] playerReady = new bool[4];
    public int countdown = 3;
    bool loadStarted = false;
    public Animator menuAnimator;
    public List<Animator> readyAnimators;

    enum Screen
    {
        TITLE,
        WAIT,
        CREDITS
    }

    Screen currentScreen;

    // Use this for initialization
    void Start ()
    {
        currentScreen = Screen.TITLE;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (currentScreen == Screen.TITLE)
        {
            for (int i = 1; i <= 4; i++)
            {
                if (Input.GetButtonDown("Jump" + i))
                {
                    GotoWait();
                }
                else if (Input.GetButtonDown("Throw" + i))
                {
                    GotoCredits();
                }
            }
        }
        else if (currentScreen == Screen.WAIT)
        {
            for (int i = 1; i <= 4; i++)
            {
                if (Input.GetButtonDown("Jump" + i))
                {
                    playerReady[i - 1] = true;
                    readyAnimators[i].SetBool("ready", true);
                }
                else if (Input.GetButtonDown("Push" + i))
                {
                    GotoTitle();
                }
            }

            if (playerReady[0] && playerReady[1] && playerReady[2] && playerReady[3] && !loadStarted)
            {
                loadStarted = true;
                SceneManager.LoadScene("IngameScene");
            }
        }
        else //credits
        {
            for (int i = 1; i <= 4; i++)
            {
                if (Input.GetButtonDown("Push" + i))
                {
                    GotoTitle();
                }
            }
        }
    }

    private void GotoWait()
    {
        currentScreen = Screen.WAIT;
        menuAnimator.SetTrigger("Wait");
    }

    private void GotoCredits()
    {
        currentScreen = Screen.WAIT;
        menuAnimator.SetTrigger("Credits");
    }

    private void GotoTitle()
    {
        currentScreen = Screen.TITLE;
        menuAnimator.SetTrigger("Title");
        for (int i = 0; i <4; i++)
        {
            playerReady[i] = false;
            readyAnimators[i].SetBool("ready", false);
        }
    }
}
