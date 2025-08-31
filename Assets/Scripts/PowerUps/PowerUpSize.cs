using Assets.Scripts.Enums;
using System.Collections;
using UnityEngine;

public class PowerUpSize : MonoBehaviour, IPowerUp
{
    public bool isActive = false;

    public bool IsActive()
    {
        return isActive;
    }

    public void ApplyPowerUp()
    {
        StartCoroutine(nameof(SizeUp));
    }

    private IEnumerator SizeUp()
    {
        isActive = true;
        Transform playerTransform;
        if (BallMovement.Instance.lastTouchPlayer == PlayersEnum.Player1)
            playerTransform = GameObject.FindGameObjectWithTag("Player1").transform;
        else
            playerTransform = GameObject.FindGameObjectWithTag("Player2").transform;

        Vector3 lastScale = playerTransform.localScale;
        playerTransform.localScale = new Vector3(1f, lastScale.y + 2f, 1f);
        //Oculto Power Up
        transform.localScale = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(7f);
        isActive = false;
        playerTransform.localScale = lastScale;
        gameObject.SetActive(false);
    }

}
