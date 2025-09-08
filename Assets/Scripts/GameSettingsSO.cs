using UnityEngine;

[CreateAssetMenu(fileName = "GameSetting", menuName = "ScriptableObjects/GameSettings")]
public class GameSettingsSO : ScriptableObject
{
    [SerializeField] private float timeSetting;

    public float TimeSetting { get { return timeSetting; } }

    public void SetTimeSetting(float timeSetting)
    {
        this.timeSetting = timeSetting;
    }
}
