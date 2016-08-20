using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClothesManager : MonoBehaviour {

    public static ClothesManager instance;
    public List<Clothes> gameClothes;

	// Use this for initialization
	void Start () {
        instance = this ;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
