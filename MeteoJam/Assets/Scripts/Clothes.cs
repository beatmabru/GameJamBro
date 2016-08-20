using UnityEngine;
using System.Collections;

public class Clothes : MonoBehaviour {
    
    private Rigidbody2D _rigidBody;
    private BoxCollider2D _hitboxDetection;
    private ClothesDetection _clothesDetection;

    public enum ClothesType
    {
        HEAD,
        NECK,
        BODY,
        LEG
    }

    public ClothesType type; 

	// Use this for initialization
	void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();
        _hitboxDetection = transform.Find("HitBox").Find("Detection").GetComponent<BoxCollider2D>();
        _clothesDetection = _hitboxDetection.GetComponent<ClothesDetection>();
    }
	

    public void Throw(Vector2 force, PlayerManager playerManager)
    {
        _clothesDetection.SetUndetectablePlayer(playerManager);
        _rigidBody.AddForce(force * GameManager.instance.throwForce, ForceMode2D.Impulse);
    }

    public void ResetUndetectable()
    {
        _clothesDetection.ResetUndetectable();
    }



}
