using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float jumpForce = 4.0f;
    public float playerSpeed = 4.0f;
    public float throwForce = 4.0f;
    public float pushForce = 4.0f;
    public float distanceMinBeforeDetection = 1.0f;

    public List<Clothes> gameClothes;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
