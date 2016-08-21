using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovePlayer : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Vector2 movement;
    private bool grounded = false;
    private int playerIndex ;
    [HideInInspector]
    public bool isFacingRight = true;

    private PlayerManager _playerManager;

    private Animator _playerAnimator;

    void OnGUI()
    {
        //GUILayout.Label("grounded : " + grounded);
    }

    // Use this for initialization
    void Start()
    {
        _playerManager = GetComponent<PlayerManager>();
        playerIndex = _playerManager.playerIndex;
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = Vector2.zero;
        if (_playerManager.deathTriggered) return;

        if (Input.GetAxis("Horizontal"+ playerIndex) > 0)
        {
            movement += Vector2.right;
            transform.localScale = Vector3.one;
            isFacingRight = true;
            _playerAnimator.SetBool("moving", true);

        }
        else if (Input.GetAxis("Horizontal"+ playerIndex) < 0)
        {
            movement += Vector2.left;
            transform.localScale = new Vector3(-1,1,1);
            isFacingRight = false;
            _playerAnimator.SetBool("moving", true);
        }
        else
            _playerAnimator.SetBool("moving", false);


        if (Input.GetButtonDown("Jump"+ playerIndex) && grounded)
            Jump();

        DetectFloor();
    }

    void FixedUpdate()
    {
        Move(movement);
    }

    void Move(Vector2 move)
    {
        _rigidbody.position += move * GameManager.instance.playerSpeed * Time.deltaTime;
    }

    void Jump()
    {
        _playerAnimator.SetTrigger("Jump");
        _rigidbody.AddForce(Vector2.up * GameManager.instance.jumpForce, ForceMode2D.Impulse);
    }

    void DetectFloor()
    {
        grounded = false;

        // Avant : 1 raycast sous le centre du player
        //RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -Vector2.up , 1f);
        // Après : 2 raycasts, pour pouvoir sauter quand on a un pied dans le vide
        //Debug.DrawLine(transform.position, transform.position - Vector3.up * 1f, Color.red);

        float playerWidth = _rigidbody.gameObject.GetComponent<BoxCollider2D>().size.x;
        RaycastHit2D[] hitsLeft = Physics2D.RaycastAll(new Vector2(transform.position.x - 0.45f*playerWidth, transform.position.y), -Vector2.up, 0.05f);
        RaycastHit2D[] hitsRight = Physics2D.RaycastAll(new Vector2(transform.position.x + 0.45f*playerWidth, transform.position.y), -Vector2.up, 0.05f);
        //        List<RaycastHit2D> hits;
        RaycastHit2D[] hits = new RaycastHit2D[hitsLeft.Length + hitsRight.Length];
        hitsLeft.CopyTo(hits, 0);
        hitsRight.CopyTo(hits, hitsLeft.Length);

        //Debug.DrawLine(new Vector3(transform.position.x - 0.45f * playerWidth, transform.position.y, 0f), new Vector3(transform.position.x - 0.45f * playerWidth, transform.position.y, 0f) - Vector3.up * 0.02f, Color.red);
        //Debug.DrawLine(new Vector3(transform.position.x + 0.45f * playerWidth, transform.position.y, 0f), new Vector3(transform.position.x + 0.45f * playerWidth, transform.position.y, 0f) - Vector3.up * 0.02f, Color.red);

        if (hits.Length>0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.tag == "Platform")
                    grounded = true;
                else if (hit.collider.tag == "Player" && hit.collider.GetComponent<PlayerManager>() != _playerManager)
                    grounded = true;
            }
        }

        _playerAnimator.SetBool("grounded", grounded);
    }

    public void PlayerIsPushed(bool pushRight)
    {
        Vector2 pushOrientation = GameManager.instance.pushPlayerOrientation;

        if (!pushRight)
            pushOrientation.x *= -1;

        _rigidbody.AddForce(pushOrientation * GameManager.instance.pushPlayerForce, ForceMode2D.Impulse);
        _playerAnimator.SetTrigger("Hit");
    }
}
