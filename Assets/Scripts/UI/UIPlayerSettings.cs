using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerSettings : MonoBehaviour
{
    [SerializeField] private GameObject panelSettings;

    [Header("Settings player 1")]
    [SerializeField] private PlayerSettingsSO player1Settings;
    [SerializeField] private TMP_InputField inputName1;
    [SerializeField] private Slider sliderSpeedMovement1;
    [SerializeField] private TMP_Text txtSpeedValue1;

    [Header("Settings player 2")]
    [SerializeField] private PlayerSettingsSO player2Settings;
    [SerializeField] private TMP_InputField inputName2;
    [SerializeField] private Slider sliderSpeedMovement2;
    [SerializeField] private TMP_Text txtSpeedValue2;

    [Header("Buttons Setting")]
    [SerializeField] private Button btnBack;
    [SerializeField] private Button btnSave;


    private void Awake()
    {
        btnBack.onClick.AddListener(OnBackPause);
        btnSave.onClick.AddListener(OnSaveClicked);
        sliderSpeedMovement1.onValueChanged.AddListener(OnValueChangeSpeedPlayer1);
        sliderSpeedMovement2.onValueChanged.AddListener(OnValueChangeSpeedPlayer2);

    }

    private void Start()
    {
        inputName1.text = player1Settings.PlayerName;
        inputName2.text = player2Settings.PlayerName;

        sliderSpeedMovement1.value = player1Settings.SpeedMovement;
        sliderSpeedMovement2.value = player2Settings.SpeedMovement;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            panelSettings.SetActive(false);
    }

    private void OnDestroy()
    {
        btnBack.onClick.RemoveAllListeners();
        btnSave.onClick.RemoveAllListeners();
    }

    private void OnBackPause()
    {
        panelSettings.SetActive(false);
        UIMainMenu.Instance.ToggleUIMainMenu();
    }

    private void OnSaveClicked()
    {
        player1Settings.SetPlayerName(inputName1.text);
        player2Settings.SetPlayerName(inputName2.text);

        player1Settings.SetSpeedMovement(sliderSpeedMovement1.value * 100);
        player2Settings.SetSpeedMovement(sliderSpeedMovement2.value * 100);

        OnBackPause();
    }

    private void OnValueChangeSpeedPlayer1(float v)
    {
        txtSpeedValue1.text = v.ToString("0.00");
    }

    private void OnValueChangeSpeedPlayer2(float v)
    {
        txtSpeedValue2.text = v.ToString("0.00");
    }
}
