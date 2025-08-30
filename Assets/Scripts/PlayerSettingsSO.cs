using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/Player")]
public class PlayerSettingsSO : ScriptableObject
{
    [SerializeField] private string playerName;
    [SerializeField] private float speedMovement;

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
}
