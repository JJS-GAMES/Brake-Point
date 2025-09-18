using UnityEngine;
using UnityEngine.UI;

public class MM_UIManager : MonoBehaviour
{
    [Header("UI Interfaces / UI Интерфейсы")]
    [SerializeField] private Canvas _mainMenuCanvas;
    [SerializeField] private Canvas _garageCanvas;
    [SerializeField] private Canvas _settingsCanvas;

    [Header("Main Buttons / Основные кнопки")]
    [Header("Main Menu")]
    [SerializeField] private Button _openGarageButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;

    [Header("Garage UI")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button[] _selectCarButtons;

    [Header("Settings")]
    [SerializeField] private Button _exitSettignsButton;

    private GameData _gameData;
    private LevelManager _levelManager;

    private void Start()
    {
        ToggleInterface(true, false, false);

        _openGarageButton?.onClick.AddListener(() => ToggleInterface(false, false, true));
        _settingsButton?.onClick.AddListener(() => ToggleInterface(false, true, false));
        _exitButton?.onClick.AddListener(() => _levelManager?.QuitGame());

        _playButton?.onClick.AddListener(() => _levelManager?.Load(1));
        foreach (var b in _selectCarButtons)
        {
            var btn = b;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => _gameData.CarPrefab = btn.GetComponent<SelectCarButton>().GetCarPrefab);
            btn.onClick.AddListener(() => ToggleInterface(false, false, true));
        }

        _exitSettignsButton?.onClick.AddListener(() => ToggleInterface(true, false, false));

    }

    public void Init(GameData gameData, LevelManager sceneManager)
    {
        _gameData = gameData;
        _levelManager = sceneManager;
    }

    private void ToggleInterface(bool mainMenu, bool settings, bool garage)
    {
        if (_mainMenuCanvas == null || _settingsCanvas == null || _garageCanvas == null) return;
        
        _mainMenuCanvas.gameObject.SetActive(mainMenu);
        _settingsCanvas.gameObject.SetActive(settings);
        _garageCanvas.gameObject.SetActive(garage);
    }
}
