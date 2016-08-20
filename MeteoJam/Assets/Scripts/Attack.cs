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

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            _attacker.PushPlayer(col.GetComponent<PlayerManager>());
        }
    }
}
