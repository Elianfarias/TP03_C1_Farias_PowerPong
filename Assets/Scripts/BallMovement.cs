using Assets.Scripts;
using Assets.Scripts.Enums;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float ballSpeed = 2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        KickOff();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
            rb.AddForce(Vector2.right * ballSpeed, ForceMode2D.Impulse);

        if (collision.gameObject.CompareTag("Player2"))
            rb.AddForce(Vector2.left * ballSpeed, ForceMode2D.Impulse);

        if (collision.gameObject.CompareTag("ScorePlayer1"))
        {
            ScoreManager.Instance.RemoveLifePlayer1();
            Reset();
        }

        if (collision.gameObject.CompareTag("ScorePlayer2"))
        {
            ScoreManager.Instance.RemoveLifePlayer2();
            Reset();
        }
    }

    private void KickOff()
    {
        int x = Random.Range(-1f, 1f) > 0 ? 1 : -1;
        int y = Random.Range(-1f, 1f) > 0 ? 1 : -1;

        rb.AddForce(new Vector2(x, y) * ballSpeed, ForceMode2D.Impulse);
    }

    private void Reset()
    {
        transform.position = Vector3.zero;
        rb.velocity = Vector3.zero;

        KickOff();
    }
}
