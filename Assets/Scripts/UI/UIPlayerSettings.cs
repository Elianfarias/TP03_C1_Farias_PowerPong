using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPlayerSettings : MonoBehaviour
{
    [SerializeField] private GameObject panelSettings;

    [Header("Settings player 1")]
    [SerializeField] private GameObject player1;
    [SerializeField] private PlayerSettingsSO player1Settings;
    [SerializeField] private TMP_InputField inputName1;
    [SerializeField] private Slider sliderSpeedMovement1;
    [SerializeField] private TMP_Text txtSpeedValue1;
    [SerializeField] private Slider sliderSize1;
    [SerializeField] private TMP_Text txtSizeValue1;

    [Header("Settings player 2")]
    [SerializeField] private GameObject player2;
    [SerializeField] private PlayerSettingsSO player2Settings;
    [SerializeField] private TMP_InputField inputName2;
    [SerializeField] private Slider sliderSpeedMovement2;
    [SerializeField] private TMP_Text txtSpeedValue2;
    [SerializeField] private Slider sliderSize2;
    [SerializeField] private TMP_Text txtSizeValue2;

    [Header("Buttons Setting")]
    [SerializeField] private Button btnSave;
    [SerializeField] private Button btnBack;


    private void Awake()
    {
        btnBack.onClick.AddListener(OnBackPause);
        btnSave.onClick.AddListener(OnSaveClicked);
        sliderSpeedMovement1.onValueChanged.AddListener(OnValueChangeSpeedPlayer1);
        sliderSize1.onValueChanged.AddListener(OnValueChangeSizePlayer1);
        sliderSpeedMovement2.onValueChanged.AddListener(OnValueChangeSpeedPlayer2);
        sliderSize2.onValueChanged.AddListener(OnValueChangeSizePlayer2);
    }

    private void Start()
    {
        float player1Speed = player1Settings.SpeedMovement / 100;
        float player2Speed = player2Settings.SpeedMovement / 100;
        float player1Size = player1Settings.SizePlayer;
        float player2Size = player2Settings.SizePlayer;

        inputName1.text = player1Settings.PlayerName;
        inputName2.text = player2Settings.PlayerName;

        sliderSpeedMovement1.value = player1Speed;
        txtSpeedValue1.text = player1Speed.ToString("0.00");
        sliderSpeedMovement2.value = player2Speed;
        txtSpeedValue2.text = player2Speed.ToString("0.00");

        sliderSize1.value = player1Size;
        txtSizeValue1.text = player1Size.ToString("0.00");
        sliderSize2.value = player2Size;
        txtSizeValue2.text = player2Size.ToString("0.00");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "InGame")
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

        player1Settings.SetSizePlayer(sliderSize1.value);
        player2Settings.SetSizePlayer(sliderSize2.value);

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

    private void OnValueChangeSizePlayer1(float v)
    {
        txtSizeValue1.text = v.ToString("0.00");
        if(player1 != null)
            player1.transform.localScale = new Vector3(1, v, 1);
    }

    private void OnValueChangeSizePlayer2(float v)
    {
        txtSizeValue2.text = v.ToString("0.00");
        if(player2 != null)
            player2.transform.localScale = new Vector3(1, v, 1);
    }

    public void UpdateColorPlayer1(Color color)
    {
        player1Settings.SetColorPlayer(color);

        if(player1 != null)
            player1.GetComponent<SpriteRenderer>().color = color;
    }

    public void UpdateColorPlayer2(Color color)
    {
        player2Settings.SetColorPlayer(color);

        if(player2 != null)
            player2.GetComponent<SpriteRenderer>().color = color;
    }
}
