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
    public float gameTime;
    public float timeAtGameOver;
    // Use this for initialization
    void Start () {
        instance = this;
        //winText.gameObject.SetActive(false);
        //gameTimeText.gameObject.SetActive(false);
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
                //winText.gameObject.SetActive(true);
                string truncatedTime = (timeAtGameOver + "").Substring(0, 5);
                gameTimeText.text = "You survived for "+ truncatedTime + " seconds";
                _endScreen.SetActive(true);
                //gameTimeText.gameObject.SetActive(true);
            }
            else if (alivePlayers.Count == 0)
            {
                gameOver = true;
                winText.text = "Sadness and Sorrow ";
                //winText.gameObject.SetActive(true);

                // On utilise un new string au moment du game over pour continuer à compter le temps
                string truncatedTime = (timeAtGameOver+"").Substring(0, 5);
                gameTimeText.text = "You've played for " + truncatedTime + " seconds";
                _endScreen.SetActive(true);
                //gameTimeText.gameObject.SetActive(true);
            }
        }
	}

    /*
    void OnGUI()
    {
        GUIStyle ballRun = new GUIStyle();
        ballRun.normal.textColor = Color.black;
        GUI.Label(new Rect(50, 10, 100, 20), gameTime.ToString(), ballRun);
    }
    */
   
}
