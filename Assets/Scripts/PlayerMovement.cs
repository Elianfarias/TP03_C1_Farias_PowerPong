using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] PlayerSettingsSO playerSettings;

    [Header("Keys Movement Configuration")]
    [SerializeField] private KeyCode keyUp = KeyCode.W;
    [SerializeField] private KeyCode keyDown = KeyCode.S;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector3(1, playerSettings.SizePlayer, 1);
        GetComponent<SpriteRenderer>().color = playerSettings.ColorPlayer;
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        if (Input.GetKey(keyUp))
            MoveAxisY(Vector2.up);
        else if (Input.GetKey(keyDown))
            MoveAxisY(Vector2.down);
    }

    private void MoveAxisY(Vector2 axisY)
    {
        rb.AddForce(axisY * (playerSettings.SpeedMovement * Time.fixedDeltaTime));
    }
}
