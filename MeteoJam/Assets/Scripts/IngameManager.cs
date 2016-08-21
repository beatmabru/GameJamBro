using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IngameManager : MonoBehaviour {

    public static IngameManager instance;

    private List<GameObject> players = new List<GameObject>();
    public Text winText ;
    public bool gameOver;
    float timerSurviveAlone;
    bool surviveAloneStarted = false;
    public float gameTime;
	// Use this for initialization
	void Start () {
        instance = this;
        winText.gameObject.SetActive(false);
        gameTime = 0f;
        for (int i = 1; i < 5; i++)
        {
            players.Add(GameObject.Find("Player"+i));
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("ResetGame"))
        {
            SceneManager.LoadScene("IngameScene");
        }

        if (!gameOver)
            gameTime += Time.deltaTime;
        // Vérification du nombre d ejoueurs en vie
        //int deathCount = 0;
        List<GameObject> alivePlayers = new List<GameObject>();
        foreach(GameObject player in players)
        {
            if (player.activeSelf) alivePlayers.Add(player);
        }


        if(alivePlayers.Count == 1 && !surviveAloneStarted)
        {
            timerSurviveAlone = 0f;
            surviveAloneStarted = true;
        }
        if(surviveAloneStarted && !gameOver)
        {
            timerSurviveAlone += Time.deltaTime;
            if(timerSurviveAlone > GameManager.instance.timeSurviveAlone && alivePlayers.Count == 1)
            {
                alivePlayers[0].GetComponent<PlayerManager>().audioSource.clip = AudioClipManager.instance.GetPlayerWin();
                alivePlayers[0].GetComponent<PlayerManager>().audioSource.Play();
                gameOver = true;
                winText.text = "Congratulations " + alivePlayers[0].name;
                winText.gameObject.SetActive(true);
            }
            else if (alivePlayers.Count == 0)
            {
                gameOver = true;
                winText.text = "Sadness and Sorrow ";
                winText.gameObject.SetActive(true);
            }
        }
	}

    void OnGUI()
    {
        GUI.Label(new Rect(100, 20, 100, 20), gameTime.ToString());
    }

   
}
