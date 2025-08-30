using TMPro;
using UnityEngine;

public class UIInterface : MonoBehaviour
{
    [SerializeField] private PlayerSettingsSO player1Settings;
    [SerializeField] private PlayerSettingsSO player2Settings;
    [SerializeField] private TMP_Text player1Name;
    [SerializeField] private TMP_Text player2Name;

    private void Update()
    {
        player1Name.text = player1Settings.PlayerName;
        player2Name.text = player2Settings.PlayerName;
    }
}
