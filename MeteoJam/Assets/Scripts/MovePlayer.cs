using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Vector2 movement;
    private bool grounded = false;
    private int platformLayer;
    private int playerIndex ;

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
        platformLayer = 1 << LayerMask.NameToLayer("Platform");
    }

    // Update is called once per frame
    void Update()
    {
        movement = Vector2.zero;

        if (Input.GetAxis("Horizontal"+ playerIndex) > 0)
            movement += Vector2.right;
        else if (Input.GetAxis("Horizontal"+ playerIndex) < 0)
            movement += Vector2.left;

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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up , 0.5f, platformLayer);
        Debug.DrawLine(transform.position, transform.position - Vector3.up * 0.5f,Color.red);
        if (hit.collider != null)
        {
            grounded = true;
        }
    }
}
