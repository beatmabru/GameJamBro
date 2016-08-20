using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    // Ajout d'une température fixe pour tester la perte
    // de points de vie pour les joueurs
    public int temperature = 4;
    public float baseLifepoints = 100f;
    public float baseHeatDamage = 2f;
    public float baseColdDamage = 1f;
    public float baseRegen = 3f;
    // TODO : intégrer les 3 facteurs de calcul des points de vie
    // + la formule.



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
