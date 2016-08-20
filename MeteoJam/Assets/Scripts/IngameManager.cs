using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class IngameManager : MonoBehaviour {

    public static IngameManager instance;

    private List<GameObject> players = new List<GameObject>();
    public Text winText ;
    public bool gameOver;

	// Use this for initialization
	void Start () {
        instance = this;
        winText.gameObject.SetActive(false);
        
        for (int i = 1; i < 5; i++)
        {
            players.Add(GameObject.Find("Player"+i));
        }
	}
	
	// Update is called once per frame
	void Update () {
        // Vérification du nombre d ejoueurs en vie
        //int deathCount = 0;
        List<GameObject> alivePlayers = new List<GameObject>();
        foreach(GameObject player in players)
        {
            if (player.activeSelf) alivePlayers.Add(player);
        }


        if(alivePlayers.Count == 1)
        {
            gameOver = true;
            winText.text = "Congratulations " + alivePlayers[0].name;
            winText.gameObject.SetActive(true);
        }
	}
}
