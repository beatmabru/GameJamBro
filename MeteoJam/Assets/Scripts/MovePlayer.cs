using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Vector2 movement;
    private bool grounded = false;

	// Use this for initialization
	void Start ()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        movement = Vector2.zero;

        if (Input.GetAxis("Horizontal") > 0)
            movement += Vector2.right;
        else if (Input.GetAxis("Horizontal") < 0)
            movement += Vector2.left;

        if (Input.GetButtonDown("Fire1"))
            Jump();
    }

    void FixedUpdate()
    {
        //_rigidbody.velocity = Vector2.zero;
        Move(movement);
    }

    void Move(Vector2 move)
    {
        //_rigidbody.velocity += movement * GameManager.instance.playerSpeed * Time.deltaTime;
        //_rigidbody.MovePosition(_rigidbody.position + move);
        _rigidbody.position += move * GameManager.instance.playerSpeed * Time.deltaTime; 
    }

    void Jump()
    {
        _rigidbody.AddForce(Vector2.up*GameManager.instance.jumpForce, ForceMode2D.Impulse);
    }
}

