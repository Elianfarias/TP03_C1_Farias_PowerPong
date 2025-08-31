using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/Player")]
public class PlayerSettingsSO : ScriptableObject
{
    [SerializeField] private string playerName;
    [SerializeField] private float speedMovement;
    [SerializeField] private float sizePlayer = 1f;
    [SerializeField] private Color colorPlayer = Color.white;

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

    public float SizePlayer { get { return sizePlayer; } }

    public void SetSizePlayer(float sizePlayer)
    {
        this.sizePlayer = sizePlayer;
    }

    public Color ColorPlayer { get { return colorPlayer; } }

    public void SetColorPlayer(Color color)
    {
        this.colorPlayer = color;
    }
}
