using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IngameManager : MonoBehaviour {

    public static IngameManager instance;

    private List<GameObject> players = new List<GameObject>();
    public Text winText;
    public Text gameTimeText;
    public GameObject _endScreen;
    public bool gameOver;
    float timerSurviveAlone;
    bool surviveAloneStarted = false;

    // On gère deux compteurs de temps, dont l'un
    // qu'on arrête au moment du game over.
    public float gameTime;
    public float timeAtGameOver;

    // Use this for initialization
    void Start () {
        instance = this;
        _endScreen.SetActive(false);
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

        // Restart possible après game over + 4 secondes
        bool aPlayerPressesJump = Input.GetButtonDown("Jump1") || Input.GetButtonDown("Jump2") || Input.GetButtonDown("Jump3") || Input.GetButtonDown("Jump4");
        bool fourSecondsHavePassedSinceGameOver = (gameTime - timeAtGameOver) > 4f;
        if (fourSecondsHavePassedSinceGameOver && aPlayerPressesJump)
        {
            SceneManager.LoadScene("IngameScene");
        }

        gameTime += Time.deltaTime;
        if (!gameOver)
            timeAtGameOver += Time.deltaTime;
        // Vérification du nombre de joueurs en vie
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
                PlayerManager winner = alivePlayers[0].GetComponent<PlayerManager>();
                winner.VoiceSource.clip = AudioClipManager.instance.GetPlayerWin();
                winner.VoiceSource.Play();
                winner.LaunchWinAnimation();
                gameOver = true;
                winText.text = alivePlayers[0].name+" wins";
                string truncatedTime = (timeAtGameOver + "").Substring(0, 5);
                gameTimeText.text = "You survived for "+ truncatedTime + " seconds";
                _endScreen.SetActive(true);
            }
            else if (alivePlayers.Count == 0)
            {
                gameOver = true;
                winText.text = "Sadness and Sorrow ";
                
                string truncatedTime = (timeAtGameOver+"").Substring(0, 5);
                gameTimeText.text = "You've played for " + truncatedTime + " seconds";
                _endScreen.SetActive(true);

            }
        }
	}
   
}
