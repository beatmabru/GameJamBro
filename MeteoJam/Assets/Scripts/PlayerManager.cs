using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager: MonoBehaviour {

    public List<Clothes> clothesList;
    private GameObject _attackHitbox;
    [SerializeField]
    private float lifePoints;

    public PlayerHud hud;
    [HideInInspector]
    public MovePlayer movePlayer;
    public int playerIndex = 0;
    private bool canPush = true;
    private bool canThrow = true;
    [HideInInspector]
    public bool deathTriggered = false ;


    // Use this for initialization
    void Start () {
        _attackHitbox = transform.Find("HitBoxAttack").gameObject;
        movePlayer = GetComponentInParent<MovePlayer>();
        SetFirstClothes();
        lifePoints = GameManager.instance.baseLifepoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (IngameManager.instance.gameOver) return; 

        UpdateHud();

        // Throw : lancer un vêtement
        if (Input.GetButtonDown("Throw" + playerIndex) && clothesList.Count > 0 && canThrow) {
            Clothes thrownClothes = PickClothesFromList(clothesList);
            Vector2 throwOrientation = new Vector2(Input.GetAxis("Horizontal" + playerIndex), Input.GetAxis("Vertical" + playerIndex));
            ClothesFly(thrownClothes,throwOrientation, GameManager.instance.throwForce,true);
        }
         
        // Push : pousser un joueur   
        if (Input.GetButtonDown("Push" + playerIndex) && canPush)
        {
            LaunchAttack();
            StartCoroutine(StartCooldownPushCoroutine());
        }

        // Mise à jour des points de vie
        UpdateLifepoints();

    }

    void SetFirstClothes()
    {
        AddClothesToPlayerList(PickClothesFromList(GameManager.instance.gameClothes));
        AddClothesToPlayerList(PickClothesFromList(GameManager.instance.gameClothes));
    }

    void UpdateHud()
    {
        hud.UpdateHud(clothesList.Count, lifePoints);
    }

    void ClothesFly(Clothes thrownClothes, Vector2 throwOrientation, float force,bool isFacingRight)
    {
        Vector2 orientation = throwOrientation;

        if (!isFacingRight)
        {
            orientation.x *= -1;
        }

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

    // TODO : intégrer le calcul de perte de point de vie tel que spécifié dans le GDD
    // (pour l'instant, simple delta à chaque frame)
    void UpdateLifepoints()
    {
        int temperature = (int) WeatherVariation.instance.weatherIndex;
        int ecartTemperatureVetement = Mathf.Abs(temperature - clothesList.Count);
        float facteurDegats = temperature > 3 ? GameManager.instance.baseHeatDamage : GameManager.instance.baseColdDamage;
        float damage = facteurDegats * ecartTemperatureVetement * Time.deltaTime;


        if (temperature == 3)
        {
            lifePoints = Mathf.Min(GameManager.instance.baseLifepoints, lifePoints + (GameManager.instance.baseRegen - ecartTemperatureVetement) * Time.deltaTime);
        }
        else if (lifePoints > 0 && lifePoints <= GameManager.instance.baseLifepoints)
        {
            lifePoints = Mathf.Max(0, lifePoints - damage);
        }

        if (lifePoints == 0 && !deathTriggered)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        deathTriggered = true;
        for (int i = 0; i < clothesList.Count; i++)
        {
            Clothes clothes = clothesList[i];
            ClothesFly(clothes, GameManager.instance.pushClothesOrientation, GameManager.instance.pushClothesForce,(i%2 == 0));
            yield return new WaitForSeconds(0.2f);
        }
        gameObject.SetActive(false);
        hud.gameObject.SetActive(false);
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

    // FIXME : on ne peut pas passer le bool en paramètre (sinon, c'est
    // le paramètre qui est modifié, et pas le flag du PlayerManager)
    // Pour le moment, on duplique les coroutines pour les deux flags...
    IEnumerator StartCooldownPushCoroutine()
    {
        canPush = false;

        float delay = GameManager.instance.pushCooldown;

        while (delay > 0)
        {
            delay -= Time.deltaTime;
            yield return 0;
        }

        canPush = true;
    }
    IEnumerator StartCooldownThrowCoroutine()
    {
        canThrow = false;

        float delay = GameManager.instance.pushCooldown;

        while (delay > 0)
        {
            delay -= Time.deltaTime;
            yield return 0;
        }

        canThrow = true;
    }

    public void StartThrowCooldown()
    {
        StartCoroutine(StartCooldownThrowCoroutine());
    }
}
