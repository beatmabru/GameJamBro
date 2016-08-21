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

    public int attackWithoutSound;

    private WeatherVariation _weather;

    void Awake()
    {
         AudioSource[] audiosources = GetComponents<AudioSource>();
        VoiceSource = audiosources[0];
        SFXSource = audiosources[1];
        GameObject variationHolder = GameObject.Find("WeatherManager");
        Debug.Assert(variationHolder != null);
        _weather = variationHolder.GetComponent<WeatherVariation>();
    }

	public List<SpriteRenderer> listSpriteScarf = new List<SpriteRenderer>();
	public List<SpriteRenderer> listSpriteGlove = new List<SpriteRenderer>();
	public List<SpriteRenderer> listSpriteHat = new List<SpriteRenderer>();
	public List<SpriteRenderer> listSpriteBody = new List<SpriteRenderer>();
	public List<SpriteRenderer> listSpriteLeftArm = new List<SpriteRenderer>();
	public List<SpriteRenderer> listSpriteRightArm = new List<SpriteRenderer>();
	public List<SpriteRenderer> listSpriteLeftLegs = new List<SpriteRenderer>();
	public List<SpriteRenderer> listSpriteRightLegs = new List<SpriteRenderer>();
	public List<SpriteRenderer> listSpritePants = new List<SpriteRenderer>();
	//public List<SpriteRenderer> listSpriteHair = new List<SpriteRenderer>();

    // Use this for initialization
    void Start () {
        attackWithoutSound = 0;
        _attackHitbox = transform.Find("HitBoxAttack").gameObject;
        EventDispatcher.instance.listeners.Add(this);
        movePlayer = GetComponentInParent<MovePlayer>();
		InitClotheColor ();
		SetFirstClothes();
        lifePoints = GameManager.instance.baseLifepoints;
        _playerAnimator = GetComponent<Animator>();

    }

	void InitClotheColor()
	{
		foreach (SpriteRenderer renderer in listSpriteScarf) {
			renderer.color = GameManager.instance.PlayerColor [playerIndex-1];
		}
		foreach (SpriteRenderer renderer in listSpriteGlove) {
			renderer.color = GameManager.instance.PlayerColor [playerIndex-1];
		}
		foreach (SpriteRenderer renderer in listSpriteHat) {
			renderer.color = GameManager.instance.PlayerColor [playerIndex-1];
		}
		foreach (SpriteRenderer renderer in listSpriteBody) {
			renderer.color = GameManager.instance.PlayerColor [playerIndex-1];
		}
		foreach (SpriteRenderer renderer in listSpriteLeftArm) {
			renderer.color = GameManager.instance.PlayerColor [playerIndex-1];
		}
		foreach (SpriteRenderer renderer in listSpriteRightArm) {
			renderer.color = GameManager.instance.PlayerColor [playerIndex-1];
		}
		foreach (SpriteRenderer renderer in listSpriteLeftLegs) {
			renderer.color = GameManager.instance.PlayerColor [playerIndex-1];
		}
		foreach (SpriteRenderer renderer in listSpriteRightLegs) {
			renderer.color = GameManager.instance.PlayerColor [playerIndex-1];
		}
		foreach (SpriteRenderer renderer in listSpritePants) {
			renderer.color = GameManager.instance.PlayerColor [playerIndex-1];
		}

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

		UpdateClotheSprite (thrownClothes, false);

        SFXSource.clip = AudioClipManager.instance.throwCloth;
        SFXSource.Play();

    }

    // Ajoute un vêtement à la liste du Player
    public void AddClothesToPlayerList(Clothes clothes)
    {
        VoiceSource.clip = AudioClipManager.instance.GetPlayerItemGet();
        playVoiceAccordingToOtherPlayer();
        SFXSource.clip = AudioClipManager.instance.equipSFX;
        SFXSource.Play();
        clothes.transform.SetParent(transform);
        clothes.transform.localScale = Vector3.one;
        clothes.ResetUndetectable();
        clothes.gameObject.SetActive(false);
        clothesList.Add(clothes);

        _playerAnimator = GetComponent<Animator>();
        _playerAnimator.SetTrigger("Dress");

		UpdateClotheSprite (clothes, true);

        EventDispatcher.Event getClothes = new EventDispatcher.Event(EventDispatcher.EventId.CLOTHES_GET, null);
        EventDispatcher.instance.ThrowEvent(getClothes);
    }

    void playVoiceAccordingToOtherPlayer()
    {
        if(AudioClipManager.instance.numberPlayerPlayingVoice() < 2)
        {
            VoiceSource.Play();
        }
    }

	void UpdateClotheSprite(Clothes clothe, bool added)
	{
		
		//Debug.Log ("player : " + name + " clothes : " + clothe.clotheName + " - " + added);
		switch (clothe.type) {
		case Clothes.ClothesType.ACCESSORY:
			if (clothe.clotheName == "scarf") {
				listSpriteScarf [0].gameObject.SetActive (added);
			} else {
				listSpriteGlove [0].gameObject.SetActive (added);
				listSpriteGlove [1].gameObject.SetActive (added);
			}
			break;
		case Clothes.ClothesType.BODY:
			if (clothe.clotheName == "body_01") {
				listSpriteBody [0].gameObject.SetActive (added);
				listSpriteLeftArm [0].gameObject.SetActive (added);
				listSpriteRightArm [0].gameObject.SetActive (added);
			} else if (clothe.clotheName == "body_02") {
				listSpriteBody [1].gameObject.SetActive (added);
				listSpriteLeftArm [1].gameObject.SetActive (added);
				listSpriteRightArm [1].gameObject.SetActive (added);
			}
			break;
		case Clothes.ClothesType.HEAD:
			if (clothe.clotheName == "Hat_01") {
				listSpriteHat [0].gameObject.SetActive (added);
			} else if (clothe.clotheName == "Hat_02") {
				listSpriteHat [1].gameObject.SetActive (added);
			}
			break;
		case Clothes.ClothesType.LEG:
			if (clothe.clotheName == "leg_01") {
				listSpritePants [0].gameObject.SetActive (added);
				listSpriteLeftLegs [0].gameObject.SetActive (added);
				listSpriteRightLegs [0].gameObject.SetActive (added);
			} else if (clothe.clotheName == "leg_02") {
				listSpritePants [1].gameObject.SetActive (added);
				listSpriteLeftLegs [1].gameObject.SetActive (added);
				listSpriteRightLegs [1].gameObject.SetActive (added);
			}
			break;
		}
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
        _playerAnimator.SetTrigger("Push");
        _attackHitbox.SetActive(true);
    }

    // TODO : intégrer le calcul de perte de point de vie tel que spécifié dans le GDD
    // (pour l'instant, simple delta à chaque frame)
    void UpdateLifepoints()
    {
        // Si période d'accalmie, la vie ne bouge pas.
        if (_weather.state == WeatherVariation.State.RESPITE) return; 

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
            if (AudioClipManager.instance.numberPlayerPlayingVoice() < 2) { pushedPlayer.VoiceSource.Play(); }
           

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
