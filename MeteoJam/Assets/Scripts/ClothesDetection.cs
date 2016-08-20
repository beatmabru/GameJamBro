using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//script on Clothes
public class ClothesDetection : MonoBehaviour {

    private Clothes _clothes;

    [HideInInspector]
    public List<PlayerManager> ignoredPlayers = new List<PlayerManager>();

    private BoxCollider2D _hitbox;

    // Use this for initialization
    void Start ()
    {
        _clothes = GetComponentInParent<Clothes>();
        _hitbox = GetComponent<BoxCollider2D>();
    }

    public void SetUndetectablePlayer(PlayerManager playerManager)
    {
        StartCoroutine(UndetectableCoroutine(playerManager));
    }

    public void DisableOnPush()
    {
        StartCoroutine(DisableCoroutine());
    }

    IEnumerator UndetectableCoroutine(PlayerManager playerManager)
    {
        ignoredPlayers.Add(playerManager);

        float delay = 0.3f;

        while (delay > 0)
        {
            delay -= Time.deltaTime;
            yield return 0;
        }

        ResetUndetectable();
    }

    IEnumerator DisableCoroutine()
    {
        _hitbox.enabled = false;

        float delay = 0.5f;

        while (delay > 0)
        {
            delay -= Time.deltaTime;
            yield return 0;
        }

        _hitbox.enabled = true;
    }

    public void ResetUndetectable()
    {
        ignoredPlayers = new List<PlayerManager>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            PlayerManager playerManager = col.GetComponent<PlayerManager>();

            if (ignoredPlayers.IndexOf(playerManager) != 0)
            {
                playerManager.AddClothesToPlayerList(_clothes);
                playerManager.StartThrowCooldown();
            }
        }
    }
}
