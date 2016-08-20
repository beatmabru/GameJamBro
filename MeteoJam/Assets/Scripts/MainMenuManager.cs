using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public Text mainText;
    private bool[] playerReady = new bool[4];
    public Text[] playerText = new Text[4];
    int countdown = 3;
    bool loadStarted = false;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        for (int i = 1; i <= 4; i++)
        {
            if (Input.GetButtonDown("Jump1"))
            {
                playerReady[i-1] = true;
                playerText[i-1].text = "READY !";
            }
        }

        if(playerReady[0] && playerReady[1] && playerReady[2] && playerReady[3] && !loadStarted)
        {
            loadStarted = true;
            StartCoroutine(LoadIngame());
        }

        if(countdown == 0)
        {
            SceneManager.LoadScene("IngameScene");
        }
    }
    
    IEnumerator LoadIngame()
    {
        while (countdown > 0)
        {
            mainText.text = countdown.ToString();
            yield return new WaitForSeconds(1);
            countdown--;
        }
        yield return null;
    }
    
}
