using UnityEngine;
using UnityEngine.UI;

public class MM_UIManager : MonoBehaviour
{
    [Header("UI Interfaces / UI Интерфейсы")]
    [SerializeField] private Canvas _mainMenuCanvas;
    [SerializeField] private Canvas _garageCanvas;
    [SerializeField] private Canvas _levelsCanvas;
    [SerializeField] private Canvas _settingsCanvas;

    [Header("Main Buttons / Основные кнопки")]
    [Header("Main Menu")]
    [SerializeField] private Button _openGarageButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;

    [Header("Garage UI")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _exitGarageButton;
    [SerializeField] private Button[] _selectCarButtons;

    [Header("Levels UI")]
    [SerializeField] private Button[] _selectLevelButtons;

    [Header("Settings")]
    [SerializeField] private Button _exitSettignsButton;

    private GameData _gameData;
    private LevelManager _levelManager;

    private void Start() // требует доработки
    {
        ToggleInterface(true, false, false, false);

        _openGarageButton?.onClick.RemoveAllListeners();
        _openGarageButton?.onClick.AddListener(() => ToggleInterface(false, false, true, false)); 

        _settingsButton?.onClick.RemoveAllListeners();
        _settingsButton?.onClick.AddListener(() => ToggleInterface(false, true, false, false));

        _exitButton?.onClick.RemoveAllListeners();
        _exitButton?.onClick.AddListener(() => _levelManager?.QuitGame());

        _playButton?.gameObject.SetActive(false); // перенести в отдельный метод

        foreach (var b in _selectCarButtons)
        {
            var btn = b;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => _gameData.CarPrefab = btn.GetComponent<SelectCarButton>().GetCarPrefab); 
            btn.onClick.AddListener(() => _playButton?.gameObject.SetActive(true)); // перенести в отдельный метод
        }

        _playButton?.onClick.RemoveAllListeners();
        _playButton?.onClick.AddListener(() => ToggleInterface(false, false, false, true));

        _exitGarageButton?.onClick.RemoveAllListeners();
        _exitGarageButton?.onClick.AddListener(() => ToggleInterface(true, false, false, false));

        foreach (var b in _selectLevelButtons)
        {
            var btn = b;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => _levelManager?.Load(btn.GetComponent<SelectLevelButton>().GetLevelIndex));
        }

        _exitButton?.onClick.RemoveAllListeners();
        _exitSettignsButton?.onClick.AddListener(() => ToggleInterface(true, false, false, false));
    }

    public void Init(GameData gameData, LevelManager sceneManager)
    {
        _gameData = gameData;
        _levelManager = sceneManager;
    }

    private void ToggleInterface(bool mainMenu, bool settings, bool garage, bool levels)
    {
        if (_mainMenuCanvas == null || _settingsCanvas == null || _garageCanvas == null || _levelsCanvas == null) return;
        
        _mainMenuCanvas.gameObject.SetActive(mainMenu);
        _settingsCanvas.gameObject.SetActive(settings);
        _garageCanvas.gameObject.SetActive(garage);
        _levelsCanvas.gameObject.SetActive(levels);
    }
}
