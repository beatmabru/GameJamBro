﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager: MonoBehaviour {

    public List<Clothes> clothesList;
    private GameObject _attackHitbox;
    private float lifePoints = 100f;

    public PlayerHud hud;
    [HideInInspector]
    public MovePlayer movePlayer;
    public int playerIndex = 0;


    // Use this for initialization
    void Start () {
        _attackHitbox = transform.Find("HitBoxAttack").gameObject;
        movePlayer = GetComponentInParent<MovePlayer>();
        SetFirstClothes();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHud();

        if (Input.GetButtonDown("Throw" + playerIndex) && clothesList.Count > 0) {
            Clothes thrownClothes = PickClothesFromList(clothesList);
            Vector2 throwOrientation = new Vector2(Input.GetAxis("Horizontal" + playerIndex), Input.GetAxis("Vertical" + playerIndex));
            ClothesFly(thrownClothes,throwOrientation, GameManager.instance.throwForce,true);
        }
            
        if (Input.GetButtonDown("Push" + playerIndex))
            LaunchAttack();
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

    void ClothesFly(Clothes thrownClothes, Vector2 throwOrientation, float force,bool isFacingRight)
    {
        Vector2 orientation = throwOrientation;

        if (!isFacingRight)
            orientation.x *= -1;

        //Clothes thrownClothes = PickClothesFromList(clothesList);
        thrownClothes.transform.position = transform.position;
        thrownClothes.gameObject.SetActive(true);

        //Vector2 throwOrientation = new Vector2(Input.GetAxis("Horizontal" + playerIndex), Input.GetAxis("Vertical" + playerIndex));
        thrownClothes.transform.SetParent(null);
        thrownClothes.transform.localScale = Vector3.one;
        thrownClothes.Throw(orientation, force, this);
    }

    // Ajoute un vêtement à la liste du Player
    public void AddClothesToPlayerList(Clothes clothes)
    {
        clothes.transform.SetParent(transform);
        clothes.transform.localScale = Vector3.one;
        clothes.ResetUndetectable();
        clothes.gameObject.SetActive(false);
        clothesList.Add(clothes);
    }

    Clothes PickClothesFromList(List<Clothes> clothesList)
    {
        if(clothesList.Count > 0)
        {
            int clothesIndex = Random.Range(0, clothesList.Count - 1);
            Clothes clothes = clothesList[clothesIndex];
            clothesList.Remove(clothes);
            return clothes;
        }

        return null ;
    }

    void LaunchAttack()
    {
        _attackHitbox.SetActive(true);
    }


    public void PushPlayer(PlayerManager pushedPlayer)
    {
        pushedPlayer.movePlayer.PlayerIsPushed(movePlayer.isFacingRight);
        EjectClothesOnPush(pushedPlayer);
    }

    public void EjectClothesOnPush(PlayerManager pushedPlayer)
    {
        Clothes clothesLost = PickClothesFromList(pushedPlayer.clothesList);
        if (clothesLost)
        {
            pushedPlayer.ClothesFly(clothesLost, GameManager.instance.pushClothesOrientation,GameManager.instance.pushClothesForce,movePlayer.isFacingRight);
            clothesLost.DisableOnPush();
        }
    }
}
