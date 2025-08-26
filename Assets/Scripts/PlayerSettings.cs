using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/Player")]
public class PlayerSettings : ScriptableObject
{
    [SerializeField] private string playerName;
    [SerializeField] private float speedMovement;
    [SerializeField] private int speedRotation;

    public string PlayerName { get { return playerName; } }

    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
    }

    public float SpeedMovement { get { return speedMovement; } }

    public void SetSpeedMovement (float speedMovement)
    {
        this.speedMovement = speedMovement;
    }

    public int SpeedRotation { get { return speedRotation; } }
}
