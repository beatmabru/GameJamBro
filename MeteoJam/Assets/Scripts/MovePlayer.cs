using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Vector2 movement;
    private bool grounded = false;
    private int playerIndex ;
    [HideInInspector]
    public bool isFacingRight = true;

    private PlayerManager _playerManager;

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
    }

    // Update is called once per frame
    void Update()
    {
        movement = Vector2.zero;

        if (Input.GetAxis("Horizontal"+ playerIndex) > 0)
        {
            movement += Vector2.right;
            transform.localScale = Vector3.one;
            isFacingRight = true;
        }
        else if (Input.GetAxis("Horizontal"+ playerIndex) < 0)
        {
            movement += Vector2.left;
            transform.localScale = new Vector3(-1,1,1);
            isFacingRight = false;
        }

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
        _rigidbody.AddForce(Vector2.up * GameManager.instance.jumpForce, ForceMode2D.Impulse);
    }

    void DetectFloor()
    {
        grounded = false;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -Vector2.up , 1f);
        //Debug.DrawLine(transform.position, transform.position - Vector3.up * 1f,Color.red);

        if(hits.Length>0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.tag == "Platform")
                    grounded = true;
                else if (hit.collider.tag == "Player" && hit.collider.GetComponent<PlayerManager>() != _playerManager)
                    grounded = true;
            }
        }
    }

    public void PlayerIsPushed(bool pushRight)
    {
        Vector2 pushOrientation = GameManager.instance.pushPlayerOrientation;

        if (!pushRight)
            pushOrientation.x *= -1;

        _rigidbody.AddForce(pushOrientation * GameManager.instance.pushPlayerForce, ForceMode2D.Impulse);
    }
}
