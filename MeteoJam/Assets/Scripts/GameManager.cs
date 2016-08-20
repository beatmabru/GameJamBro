using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float jumpForce = 4.0f;
    public float playerSpeed = 4.0f;
    public float throwForce = 4.0f;
    public float pushPlayerForce = 4.0f;
    public float pushClothesForce = 4.0f;
    public float distanceMinBeforeDetection = 1.0f;
    public float pushCooldown = 0.5f;
    public float throwCooldown = 0.5f;
    public Vector2 pushPlayerOrientation = Vector2.right;
    public Vector2 pushClothesOrientation = Vector2.right;

    
    public float baseLifepoints = 100f;
    public float baseHeatDamage = 2f;
    public float baseColdDamage = 1f;
    public float baseRegen = 3f;

    

    public List<Clothes> gameClothes;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

	// Use this for initialization
	void Start () {
        // Commenter si on utilise la scène "Main"
        if(SceneManager.GetActiveScene().name != "Main")
        {
            SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
        }
           
	}

    // Update is called once per frame
    void Update()
    {

    }

    public void InitIngame()
    {

    }
	    
}
