using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Elements / UI Элементы")]
    [SerializeField] private Canvas _baseInterface;
    [SerializeField] private Canvas _defeatInterface;
    [SerializeField] private Canvas _finishInterface;

    [Header("Base Interface")]

    [Space, SerializeField] private TextMeshProUGUI _carSpeed;
    [SerializeField] private Scrollbar _progressBar;
    [SerializeField] private Button _respawnButton;
    [SerializeField] private Button _returnMainMenuButton;

    [Space, SerializeField] private AccelerationPedal _accelerationPedal;
    [SerializeField] private BrakePedal _brakePedal;

    [Header("Defeat UI")]
    [Space, SerializeField] private Image _defeatBlackout;
    [SerializeField] private Button _restartDefeatButton;

    [Header("Finish UI")]
    [Space, SerializeField] private Image _finishBlackout;
    [SerializeField] private Button _restartFinishButton;

    private Transform _restartDefeatButtonTransform;
    private Transform _restartFinishButtonTransform;

    private ProgressTracker _progressTracker;

    private LevelManager _levelManager;
    private GameManager _gameManager;

    private Car _carScript;

    private CanvasGroup _blackoutDefeatCanvasGroup;
    private CanvasGroup _blackoutFinishCanvasGroup;

    private bool _isFinishUIActive = false;
    private bool _isDefeatUIActive = false;
    public void Init(GameManager gameManager, LevelManager levelManager)
    {
        _gameManager = gameManager;
        _levelManager = levelManager;

        ToggleUI(true, false, false);

        // Base Interface Initialization / Инициализация базового интерфейса
        _respawnButton?.onClick.RemoveAllListeners();
        _respawnButton?.onClick.AddListener(_levelManager.RestartLevel);
        _progressTracker = _progressBar.GetComponent<ProgressTracker>();
        _progressTracker.Init(_gameManager, _levelManager);

        _blackoutFinishCanvasGroup = _finishBlackout.GetComponent<CanvasGroup>();
        if (_blackoutFinishCanvasGroup == null)
        {
            _blackoutFinishCanvasGroup = _finishBlackout.gameObject.AddComponent<CanvasGroup>();
        }

        _blackoutDefeatCanvasGroup = _defeatBlackout.GetComponent<CanvasGroup>();
        if (_blackoutDefeatCanvasGroup == null)
        {
            _blackoutDefeatCanvasGroup = _defeatBlackout.gameObject.AddComponent<CanvasGroup>();
        }

        _returnMainMenuButton?.onClick.RemoveAllListeners();
        _returnMainMenuButton?.onClick.AddListener(() => _levelManager.Load(0));

        // Defeat UI Initialization / Инциализация проигрышного интерфейса
        _restartDefeatButton?.onClick.RemoveAllListeners();
        _restartDefeatButton?.onClick.AddListener(_levelManager.RestartLevel);
        _restartDefeatButtonTransform = _restartDefeatButton?.transform;

        // Finish UI Initialization / Инциализация финишного интерфейса
        _restartFinishButton?.onClick.RemoveAllListeners();
        _restartFinishButton?.onClick.AddListener(_levelManager.RestartLevel);
        _restartFinishButtonTransform = _restartFinishButton?.transform;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _levelManager.RestartLevel();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _levelManager.Load(0);
        }
    }
    public void CarUIInitialization(Car carScript)
    {
        _carScript = carScript;
        var controller = _carScript.CarController;

        controller.OnSpeedChanged += UpdateSpeedText;
        controller.OnCarDefeated += ShowDefeatUI;
        _gameManager.OnProgressChanged += UpdateProgressBarUI;

        _accelerationPedal?.Init(controller);
        _brakePedal?.Init(controller);
    }

    public void UnsubscribeFromCar(Car car)
    {
        if (car == null) return;
        var controller = car.CarController;
        if (controller == null) return;

        controller.OnSpeedChanged -= UpdateSpeedText;
        controller.OnCarDefeated -= ShowDefeatUI;
        _gameManager.OnProgressChanged -= UpdateProgressBarUI;
    }

    private void ToggleUI(bool baseInterface, bool defeatInterface, bool finishInterface)
    {
        _baseInterface?.gameObject.SetActive(baseInterface);
        _defeatInterface?.gameObject.SetActive(defeatInterface);
        _finishInterface?.gameObject.SetActive(finishInterface);
    }
    private void UpdateProgressBarUI(float progress)
    {
        if (_levelManager != null && _carScript != null)
        {
            _progressBar.size = progress;
        }
    }
    private void UpdateSpeedText(float speed)
    {
        if (_carScript != null && _carScript.CarController != null)
        {
            _carSpeed.text = $"Speed: {Mathf.RoundToInt(speed)} km/h";
        }
    }

    private void ShowDefeatUI()
    {
        ToggleDefeatUI(true);
    }

    private void TogglePanel(bool toggle,bool instant, ref bool isActiveFlag, CanvasGroup blackout, Transform restartButtonTransform, Button restartButton, bool showAsFinish)
    {
        if (isActiveFlag == toggle) return;
        isActiveFlag = toggle;

        blackout.DOKill();
        restartButtonTransform.DOKill();

        if (toggle)
        {
            ToggleUI(false, !showAsFinish, showAsFinish);

            blackout.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);

            if (instant)
            {
                blackout.alpha = 1f;
                restartButtonTransform.localScale = Vector3.one;
            }
            else
            {
                blackout.alpha = 0f;
                restartButtonTransform.localScale = Vector3.zero;

                blackout.DOFade(1f, 0.1f).SetEase(Ease.InOutQuad);
                restartButtonTransform.DOScale(1f, 0.6f).SetEase(Ease.OutBack);
            }
        }
        else
        {
            if (instant)
            {
                blackout.alpha = 0f;
                blackout.gameObject.SetActive(false);

                restartButtonTransform.localScale = Vector3.zero;
                restartButton.gameObject.SetActive(false);
            }
            else
            {
                blackout.DOFade(0f, 0.5f)
                       .SetEase(Ease.InOutQuad)
                       .OnComplete(() => blackout.gameObject.SetActive(false));

                restartButtonTransform.DOScale(0f, 0.4f)
                                      .SetEase(Ease.InBack)
                                      .OnComplete(() => restartButton.gameObject.SetActive(false));

                ToggleUI(true, false, false);
            }
        }
    }
    public void ToggleDefeatUI(bool toggle, bool instant = false) => TogglePanel(toggle, instant, ref _isDefeatUIActive,_blackoutDefeatCanvasGroup, _restartDefeatButtonTransform, _restartDefeatButton, showAsFinish: false);
    public void ToggleFinishUI(bool toggle, bool instant = false) => TogglePanel(toggle, instant, ref _isFinishUIActive, _blackoutFinishCanvasGroup, _restartFinishButtonTransform, _restartFinishButton, showAsFinish: true);

}
