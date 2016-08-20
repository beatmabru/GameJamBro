using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager: MonoBehaviour {

    public List<Clothes> clothesList;
    private float lifePoints = 100f;

    public PlayerHud hud;
    public int playerIndex = 0;

    // Use this for initialization
    void Start () {
        SetFirstClothes();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateHud();

        if (Input.GetButtonDown("Throw" + playerIndex)) {
            ThrowClothes();
        }
    }

    void SetFirstClothes()
    {
        AddClothesToPlayerList(PickClothesFromList(GameManager.instance.gameClothes));
        AddClothesToPlayerList(PickClothesFromList(GameManager.instance.gameClothes));
    }

    void UpdateHud()
    {
        hud.UpdateHud(clothesList.Count);
    }

    void ThrowClothes()
    {
        if(clothesList.Count > 0)
        {
            Clothes thrownClothes = PickClothesFromList(clothesList);
            thrownClothes.transform.position = transform.position;
            thrownClothes.gameObject.SetActive(true);

            Vector2 throwOrientation = new Vector2(Input.GetAxis("Horizontal" + playerIndex), Input.GetAxis("Vertical" + playerIndex));
            thrownClothes.transform.SetParent(null);
            thrownClothes.Throw(throwOrientation,this);
        }
    }

    // Ajoute un vêtement à la liste du Player
    public void AddClothesToPlayerList(Clothes clothes)
    {
        clothes.transform.SetParent(transform);
        clothes.ResetUndetectable();
        clothes.gameObject.SetActive(false);
        clothesList.Add(clothes);
    }

    Clothes PickClothesFromList(List<Clothes> clothesList)
    {
        int clothesIndex = Random.Range(0, clothesList.Count - 1);
        Clothes clothes = clothesList[clothesIndex];
        clothesList.Remove(clothes);

        return clothes;
    }
}
