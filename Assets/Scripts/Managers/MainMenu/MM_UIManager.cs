using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MM_UIManager : MonoBehaviour
{
    [Header("UI Interfaces / UI Интерфейсы")]
    [SerializeField] private Canvas _mainMenuCanvas;
    [SerializeField] private Canvas _garageCanvas;
    [SerializeField] private Canvas _levelsCanvas;
    [SerializeField] private Canvas _settingsCanvas;
    [SerializeField] private Canvas _loadingCanvas;

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

    [Header("Loading")]
    [SerializeField] private TextMeshProUGUI _loadingMessage_TMP;
    [SerializeField] private string[] _loadingMessages;

    [Space, SerializeField] private  TextMeshProUGUI _loadingProgress;
    [SerializeField] private Image _loadingEclipse;

    private GameData _gameData;
    private LevelManager _levelManager;

    private void Start()
    {
        ToggleInterface(true);

        _openGarageButton?.onClick.RemoveAllListeners();
        _openGarageButton?.onClick.AddListener(() => ToggleInterface(false, false, true)); 

        _settingsButton?.onClick.RemoveAllListeners();
        _settingsButton?.onClick.AddListener(() => ToggleInterface(false, true));

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
        _exitGarageButton?.onClick.AddListener(() => ToggleInterface(true));

        foreach (var b in _selectLevelButtons)
        {
            var btn = b;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => LoadingLevel(btn.GetComponent<SelectLevelButton>().GetLevelIndex));
        }

        _exitSettignsButton?.onClick.RemoveAllListeners();
        _exitSettignsButton?.onClick.AddListener(() => ToggleInterface(true));
    }

    public void Init(GameData gameData, LevelManager sceneManager)
    {
        _gameData = gameData;
        _levelManager = sceneManager;
    }

    private void ToggleInterface(bool mainMenu = false, bool settings = false, bool garage = false, bool levels = false, bool loading = false)
    {
        if (_mainMenuCanvas == null || _settingsCanvas == null || _garageCanvas == null || _levelsCanvas == null || _loadingCanvas == null)
        {
            Debug.LogError("Not all Canvas's are initialized!");
            return;
        }
        
        _mainMenuCanvas.gameObject.SetActive(mainMenu);
        _settingsCanvas.gameObject.SetActive(settings);
        _garageCanvas.gameObject.SetActive(garage);
        _levelsCanvas.gameObject.SetActive(levels);
        _loadingCanvas.gameObject.SetActive(loading);
    }

    private void LoadingLevel(int index)
    {
        ToggleInterface(false, false, false, false, true);

        _levelManager.Load(index);
        StartCoroutine(UpdateLoadingUI());
    }

    private IEnumerator UpdateLoadingUI()
    {
        if (_loadingMessages != null && _loadingMessages.Length > 0)
        {
            int msgIndex = Random.Range(0, _loadingMessages.Length);
            _loadingMessage_TMP.text = _loadingMessages[msgIndex];
        }

        while (_levelManager.LoadSceneAsyncOperation != null && !_levelManager.LoadSceneAsyncOperation.isDone)
        {
            float progress = _levelManager.Progress;
            _loadingEclipse.fillAmount = progress;
            _loadingProgress.text = $"{Mathf.RoundToInt(progress * 100f)}%";

            yield return null;
        }
    }
}
