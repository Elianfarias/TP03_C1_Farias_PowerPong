using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] PlayerSettingsSO playerSettings;
    private SpriteRenderer spriteRenderer;

    [Header("Keys Movement Configuration")]
    [SerializeField] private KeyCode keyUp = KeyCode.W;
    [SerializeField] private KeyCode keyDown = KeyCode.S;
    [SerializeField] private KeyCode keyLeft = KeyCode.A;
    [SerializeField] private KeyCode keyRight = KeyCode.D;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector3(1, playerSettings.SizePlayer, 1);
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().color = playerSettings.ColorPlayer;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
            playerSettings.SetColorPlayer(new Color(Random.value, Random.value, Random.value));
    }

    private void PlayerMove()
    {
        if (Input.GetKey(keyUp))
            MoveAxisY(Vector2.up);
        else if (Input.GetKey(keyDown))
            MoveAxisY(Vector2.down);
        else if (Input.GetKey(keyLeft))
            MoveAxisX(Vector2.left);
        else if (Input.GetKey(keyRight))
            MoveAxisX(Vector2.right);
    }

    private void MoveAxisY(Vector2 axisY)
    {
        rb.AddForce(axisY * (playerSettings.SpeedMovement * Time.fixedDeltaTime));
    }

    private void MoveAxisX(Vector2 axisX)
    {
        rb.AddForce(axisX * ((playerSettings.SpeedMovement / 2) * Time.fixedDeltaTime));
    }
}
