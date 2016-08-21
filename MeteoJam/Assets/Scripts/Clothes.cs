using UnityEngine;
using System.Collections;

public class Clothes : MonoBehaviour {
    
    private Rigidbody2D _rigidBody;
    private BoxCollider2D _hitboxDetection;
    private ClothesDetection _clothesDetection;
	//[HideInInspector]
	public string clotheName;

    public enum ClothesType
    {
        HEAD,
        ACCESSORY,
        BODY,
        LEG,
		COUNT
    }

    public ClothesType type; 

	// Use this for initialization
	void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();
        _hitboxDetection = transform.Find("HitBox").Find("Detection").GetComponent<BoxCollider2D>();
        _clothesDetection = _hitboxDetection.GetComponent<ClothesDetection>();
    }
	

    public void Throw(Vector2 orientation, float force, PlayerManager playerManager)
    {
        _clothesDetection.SetUndetectablePlayer(playerManager);
        _rigidBody.AddForce(orientation * force, ForceMode2D.Impulse);
    }

    public void SetUndetectablePlayer(PlayerManager playerManager)
    {
        _clothesDetection.SetUndetectablePlayer(playerManager);
    }

    public void ResetUndetectable()
    {
        _clothesDetection.ResetUndetectable();
    }

    public void DisableOnPush()
    {
        _clothesDetection.DisableOnPush();
    }



}
