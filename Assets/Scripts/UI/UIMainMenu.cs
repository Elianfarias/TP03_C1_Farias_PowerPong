using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public static UIMainMenu Instance { get; private set; }
    public bool isPause = false;

    [SerializeField] private GameObject panelMainMenu;
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private GameObject panelCredits;

    [Header("Buttons Main Menu")]
    [SerializeField] private Button btnStart;
    [SerializeField] private Button btnSettings;
    [SerializeField] private Button btnCredits;
    [SerializeField] private Button btnExit;
    [SerializeField] private Button btnBackCredits;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        btnStart.onClick.AddListener(TogglePause);
        btnSettings.onClick.AddListener(OnSettingClicked);
        btnCredits.onClick.AddListener(OnCreditClicked);
        btnExit.onClick.AddListener(OnExitClicked);
        btnBackCredits.onClick.AddListener(OnBackCredits);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!panelMainMenu.activeSelf && isPause)
                ToggleUIMainMenu();
            else
                TogglePause();
        }
    }

    private void OnDestroy()
    {
        btnStart.onClick.RemoveAllListeners();
        btnSettings.onClick.RemoveAllListeners();
        btnCredits.onClick.RemoveAllListeners();
        btnBackCredits.onClick.RemoveAllListeners();
    }

    public void TogglePause()
    {
        isPause = !isPause;

        if (isPause)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;

        ToggleUIMainMenu();
    }

    public void ToggleUIMainMenu()
    {
        if (panelCredits.activeSelf)
            panelCredits.SetActive(false);
        if (panelSettings.activeSelf)
            panelCredits.SetActive(false);

        panelMainMenu.SetActive(!panelMainMenu.activeSelf);
    }

    private void OnSettingClicked()
    {
        ToggleUIMainMenu();
        panelSettings.SetActive(true);
    }

    private void OnCreditClicked()
    {
        ToggleUIMainMenu();
        panelCredits.SetActive(true);
    }

    private void OnExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnBackCredits()
    {
        ToggleUIMainMenu();
    }
}
