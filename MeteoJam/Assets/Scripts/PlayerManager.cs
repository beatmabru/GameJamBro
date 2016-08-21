using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager: MonoBehaviour, EventDispatcher.IEventListener
{

    public List<Clothes> clothesList;
    [HideInInspector]
    public GameObject _attackHitbox;
    public float lifePoints;

    public PlayerHud hud;
    [HideInInspector]
    public MovePlayer movePlayer;
    public int playerIndex = 0;
    private bool canPush = true;
    private bool canThrow = true;
    [HideInInspector]
    public bool deathTriggered = false ;

    public AudioSource VoiceSource;
    public AudioSource SFXSource;
    private Animator _playerAnimator;
    private WeatherVariation _weather;

    void Awake()
    {
        GameObject variationHolder = GameObject.Find("WeatherManager");
        Debug.Assert(variationHolder != null);
        _weather = variationHolder.GetComponent<WeatherVariation>();
    }

    // Use this for initialization
    void Start () {
        AudioSource[] audiosources = GetComponents<AudioSource>();
        VoiceSource = audiosources[0];
        SFXSource = audiosources[1];
        _attackHitbox = transform.Find("HitBoxAttack").gameObject;
        EventDispatcher.instance.listeners.Add(this);
        movePlayer = GetComponentInParent<MovePlayer>();
        SetFirstClothes();
        lifePoints = GameManager.instance.baseLifepoints;
        _playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IngameManager.instance.gameOver) return; 

        UpdateHud();

        // Throw : lancer un vêtement
        if (Input.GetButtonDown("Throw" + playerIndex) && clothesList.Count > 0 && canThrow)
        {
            _playerAnimator.SetTrigger("Throw");
            Clothes thrownClothes = PickClothesFromList(clothesList);
            EventDispatcher.Event throwClothes = new EventDispatcher.Event(EventDispatcher.EventId.CLOTHES_THROW, this);
            EventDispatcher.instance.ThrowEvent(throwClothes);
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
        AddClothesToPlayerList(PickClothesFromList(ClothesManager.instance.gameClothes));
        AddClothesToPlayerList(PickClothesFromList(ClothesManager.instance.gameClothes));
        hud.changeColor(clothesList.Count);
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

        SFXSource.clip = AudioClipManager.instance.throwCloth;
        SFXSource.Play();

    }

    // Ajoute un vêtement à la liste du Player
    public void AddClothesToPlayerList(Clothes clothes)
    {
        VoiceSource.clip = AudioClipManager.instance.GetPlayerItemGet();
        VoiceSource.Play();
        SFXSource.clip = AudioClipManager.instance.equipSFX;
        SFXSource.Play();
        clothes.transform.SetParent(transform);
        clothes.transform.localScale = Vector3.one;
        clothes.ResetUndetectable();
        clothes.gameObject.SetActive(false);
        clothesList.Add(clothes);

        _playerAnimator = GetComponent<Animator>();
        _playerAnimator.SetTrigger("Dress");

        EventDispatcher.Event getClothes = new EventDispatcher.Event(EventDispatcher.EventId.CLOTHES_GET, null);
        EventDispatcher.instance.ThrowEvent(getClothes);
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
        VoiceSource.clip = AudioClipManager.instance.GetPlayerPush();
        VoiceSource.Play();
        _playerAnimator.SetTrigger("Push");
        _attackHitbox.SetActive(true);
    }

    // TODO : intégrer le calcul de perte de point de vie tel que spécifié dans le GDD
    // (pour l'instant, simple delta à chaque frame)
    void UpdateLifepoints()
    {
        // Si période d'accalmie, la vie ne bouge pas.
        //if (WeatherVariation.instance.state == WeatherVariation.State.RESPITE) return; 

        int temperature = (int)_weather.weatherIndex;
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
        AudioClip clip;
        if ((int)_weather.weatherIndex < 3)
        {
            clip = AudioClipManager.instance.GetPlayerDeathByHot(); 
        }
        else
        {
            clip = AudioClipManager.instance.GetPlayerDeathByCold();
        }

        VoiceSource.clip = clip;
        VoiceSource.Play();
        for (int i = 0; i < clothesList.Count; i++)
        {
            Clothes clothes = clothesList[i];
            ClothesFly(clothes, GameManager.instance.pushClothesOrientation, GameManager.instance.pushClothesForce,(i%2 == 0));
            yield return new WaitForSeconds(0.2f);
        }
        _playerAnimator.SetTrigger("Death");
        hud.gameObject.SetActive(false);
        while (VoiceSource.isPlaying)
            yield return new WaitForEndOfFrame();

        gameObject.SetActive(false);
        hud.gameObject.SetActive(false);
        EventDispatcher.instance.listeners.Remove(this);
    }

    public void PushPlayer(PlayerManager pushedPlayer, bool loseClothes)
    {
        pushedPlayer.movePlayer.PlayerIsPushed(movePlayer.isFacingRight);
        if (loseClothes)
        {
            EjectClothesOnPush(pushedPlayer);
        }
        else
        {
            SFXSource.clip = AudioClipManager.instance.attackMissedSFX;
            SFXSource.Play();
        }
    }

    public void EjectClothesOnPush(PlayerManager pushedPlayer)
    {
        Clothes clothesLost = PickClothesFromList(pushedPlayer.clothesList);
        if (clothesLost)
        {
            pushedPlayer.VoiceSource.clip = AudioClipManager.instance.GetPlayerPushed();
            pushedPlayer.VoiceSource.Play();
            SFXSource.clip = AudioClipManager.instance.attackSFX;
            SFXSource.Play();

            pushedPlayer.ClothesFly(clothesLost, GameManager.instance.pushClothesOrientation,GameManager.instance.pushClothesForce,movePlayer.isFacingRight);
            clothesLost.DisableOnPush();
            EventDispatcher.Event throwClothes = new EventDispatcher.Event(EventDispatcher.EventId.CLOTHES_THROW, (object)pushedPlayer);
            EventDispatcher.instance.ThrowEvent(throwClothes);
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

    public bool HandleEvent(EventDispatcher.Event e)
    {
        if( e.id == EventDispatcher.EventId.WEATHER_RESPITE_CHANGE )
        {
            hud.changeColor(clothesList.Count);
        }
        else if (e.id == EventDispatcher.EventId.CLOTHES_THROW || e.id == EventDispatcher.EventId.CLOTHES_GET )
        {
            if( (PlayerManager) e.data == this )
            {
                hud.changeColor(clothesList.Count);
            }
        }

        return false;
    }

    public void LaunchWinAnimation()
    {
        _playerAnimator.SetTrigger("Win");
    }
}
