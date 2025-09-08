using Assets.Scripts;
using Assets.Scripts.Enums;
using System.Collections;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public static BallMovement Instance { get; private set; }
    public float ballSpeed = 1.5f;
    public PlayersEnum lastTouchPlayer;

    [Header("Speed settings")]
    [SerializeField] private float initialBallSpeed = 2f;

    [Header("Audio clips")]
    [SerializeField] private AudioClip audioClipPowerUp;
    [SerializeField] private AudioClip audioClipBounce;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        KickOff();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionGO = collision.gameObject;

        if (collisionGO.CompareTag("Player1")
            || collisionGO.CompareTag("Player2")
            || collisionGO.CompareTag("Wall")
            || collisionGO.CompareTag("Wall"))
            AudioController.Instance.PlaySoundEffect(audioClipBounce);

        if (collisionGO.CompareTag("Player1"))
        {
            lastTouchPlayer = PlayersEnum.Player1;
            rb.AddForce(Vector2.right * ballSpeed);
        }

        if (collisionGO.CompareTag("Player2"))
        {
            lastTouchPlayer = PlayersEnum.Player2;
            rb.AddForce(Vector2.left * ballSpeed);
        }

        if (collisionGO.CompareTag("ScorePlayer1"))
        {
            ScoreManager.Instance.RemoveLifePlayer1();
            Reset();
        }

        if (collisionGO.CompareTag("ScorePlayer2"))
        {
            ScoreManager.Instance.RemoveLifePlayer2();
            Reset();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            AudioController.Instance.PlaySoundEffect(audioClipPowerUp);
            collision.GetComponent<IPowerUp>().ApplyPowerUp();
        }
    }

    private void KickOff()
    {
        StartCoroutine(nameof(KickOffMovement));
    }

    private IEnumerator KickOffMovement()
    {
        int x = Random.Range(-1f, 1f) > 0 ? 1 : -1;
        int y = Random.Range(-1f, 1f) > 0 ? 1 : -1;

        yield return new WaitForSeconds(1f);

        rb.AddForce(new Vector2(x, y) * initialBallSpeed, ForceMode2D.Impulse);
    }

    private void Reset()
    {
        transform.position = Vector3.zero;
        rb.velocity = Vector3.zero;

        KickOff();
    }
}
