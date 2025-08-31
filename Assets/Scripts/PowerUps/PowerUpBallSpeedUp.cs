using UnityEngine;

public class PowerUpBallSpeedUp : MonoBehaviour, IPowerUp
{
    public bool isActive = false;

    public bool IsActive()
    {
        return isActive;
    }

    public void ApplyPowerUp()
    {
        isActive = true;
        GameObject ballGO = GameObject.FindGameObjectWithTag("Ball");
        Rigidbody2D ballRB = ballGO.GetComponent<Rigidbody2D>();

        ballRB.AddForce(ballRB.velocity.normalized * 3f, ForceMode2D.Impulse);

        gameObject.SetActive(false);
        isActive = false;
    }
}
