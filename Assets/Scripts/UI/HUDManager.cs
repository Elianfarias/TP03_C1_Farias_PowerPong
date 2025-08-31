using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("PlayersHUD")]
    [SerializeField] private PlayerSettingsSO player1Settings;
    [SerializeField] private PlayerSettingsSO player2Settings;
    [SerializeField] private TMP_Text textPlayer1Name;
    [SerializeField] private TMP_Text textPlayer2Name;
    [Header("PlayerWonHUD")]
    public GameObject panelPlayerWon;
    [SerializeField] private TMP_Text textPlayerWon;
    [SerializeField] private Button btnReset;
    [SerializeField] private Button btnBackToMenu;

    private void Awake()
    {
        Instance = this;
        btnBackToMenu.onClick.AddListener(BackToMenu);
        btnReset.onClick.AddListener(ResetGame);
    }

    private void Update()
    {
        textPlayer1Name.text = player1Settings.PlayerName;
        textPlayer2Name.text = player2Settings.PlayerName;
    }

    private void OnDestroy()
    {
        btnBackToMenu.onClick.RemoveAllListeners();
        btnReset.onClick.RemoveAllListeners();
    }

    public void ShowPanelPlayerWon(string playerName)
    {
        panelPlayerWon.SetActive(true);
        textPlayerWon.text = $"{playerName} Wins!";
    }

    private void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    private void ResetGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("InGame");
    }
}
