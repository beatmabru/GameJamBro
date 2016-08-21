using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClothesManager : MonoBehaviour {


    public static ClothesManager instance;
    public List<Clothes> gameClothes;

	public List<Sprite> listHairSprite = new List<Sprite>();
	public List<Sprite> listHatSprite = new List<Sprite>();
	public List<Sprite> listAccessorySprite = new List<Sprite>();
	public List<Sprite> listBodySprite = new List<Sprite>();
	public List<Sprite> listLegsSprite = new List<Sprite>();
	public List<Sprite> listPantsSprite = new List<Sprite>();

	int indexHatSprite = 0;
	int indexAccessorySprite  = 0;
	int indexBodySprite = 0;
	int indexLegsSprite = 0;

	void Awake()
	{
		instance = this ;

	}
	// Use this for initialization
	void Start () {
		foreach (Clothes clothe in gameClothes) {
			//Debug.Log ("clothe name = " + clothe.name + " - " + clothe.type);
			switch (clothe.type) {
			case Clothes.ClothesType.ACCESSORY:
				clothe.clotheName = listAccessorySprite [indexAccessorySprite].name; 
				indexAccessorySprite++;
				break;
			case Clothes.ClothesType.BODY:
				clothe.clotheName = listBodySprite [indexBodySprite].name; 
				indexBodySprite++;
				break;
			case Clothes.ClothesType.HEAD:
				clothe.clotheName = listHatSprite [indexHatSprite].name; 
				indexHatSprite++;
				break;
			case Clothes.ClothesType.LEG:
				clothe.clotheName = listLegsSprite [indexLegsSprite].name;
				indexLegsSprite++;
				break;
			}

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
