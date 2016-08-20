using UnityEngine;
using System.Collections;

//script on Clothes
public class ClothesDetection : MonoBehaviour {

    private Clothes _clothes;

    [HideInInspector]
    public PlayerManager playerThrower;

    // Use this for initialization
    void Start () {
        _clothes = GetComponentInParent<Clothes>();
    }

    public void SetUndetectablePlayer(PlayerManager playerManager)
    {
        StartCoroutine(UndetectableCoroutine(playerManager));
    }

    IEnumerator UndetectableCoroutine(PlayerManager playerManager)
    {
        playerThrower = playerManager;

        float delay = 0.3f;

        while (delay > 0)
        {
            delay -= Time.deltaTime;
            yield return 0;
        }

        playerThrower = null;
    }

    public void ResetUndetectable()
    {
        StopAllCoroutines();
        playerThrower = null;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            PlayerManager playerManager = col.GetComponent<PlayerManager>();

            if (playerManager != playerThrower)
                playerManager.AddClothesToPlayerList(_clothes);
        }
    }
}
