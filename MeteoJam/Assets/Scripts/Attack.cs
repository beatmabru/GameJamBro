using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{
    private PlayerManager _attacker;
    private float _duration = 0.1f;
    private float _currentDuration = 0.1f;

    // Use this for initialization
    void Start () {
        _attacker = GetComponentInParent<PlayerManager>();
    }
	
	// Update is called once per frame
	void Update () {
        _currentDuration -= Time.deltaTime;
        if(_currentDuration < 0)
        {
            _currentDuration = _duration;
            gameObject.SetActive(false);
        }
	}

    // FIXME : tentative de gérer les backstabs avec
    // if (col.tag == "HitBoxAttack")
    // mais sans succès... à creuser (ou à faire autrement).
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            _attacker.SFXSource.clip = AudioClipManager.instance.attackSFX;
            _attacker.SFXSource.Play();

            // Si le joueur adverse n'est pas en train de push face à son attaquant
            PlayerManager attackedPlayer = col.GetComponent<PlayerManager>();
            if (!attackedPlayer._attackHitbox.activeSelf)
            {
                _attacker.PushPlayer(attackedPlayer, true);
            }
            // Sinon, les deux joueurs trébuchent, sans déclencher de perte de vêtement.
            else
            {
                _attacker.PushPlayer(col.GetComponent<PlayerManager>(), false);
                attackedPlayer.PushPlayer(_attacker, false);
            }
        }
        else
        {
            int random = Mathf.CeilToInt(Random.Range(0.01f, 5)) - 1;
            if (random > 2 || _attacker.attackWithoutSound >= 2)
            {
                _attacker.attackWithoutSound = 0;
                _attacker.VoiceSource.clip = AudioClipManager.instance.GetPlayerPush();
                AudioClipManager.instance.playSound(_attacker.VoiceSource);
            }
            else
            {
                _attacker.attackWithoutSound++;
            }
        }
    }
}
