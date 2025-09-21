using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements / UI Ёлементы")]
    [SerializeField] private Canvas _baseInterface;
    [SerializeField] private Canvas _finishInterface;

    [Header("Base Interface")]

    [Space, SerializeField] private TextMeshProUGUI _carSpeed;
    [SerializeField] private Scrollbar _progressBar;
    [SerializeField] private Button _respawnButton;
    [SerializeField] private Button _returnMainMenuButton;

    [Space, SerializeField] private AccelerationPedal _accelerationPedal;
    [SerializeField] private BrakePedal _brakePedal;

    [Header("Finish UI")]
    [Space, SerializeField] private Image _blackout;
    [SerializeField] private Button _restartButton;

    private Transform _restartButtonTransform;

    private ProgressTracker _progressTracker;

    private LevelManager _levelManager;
    private GameManager _gameManager;

    private Car _carScript;
    private CanvasGroup _blackoutCanvasGroup;

    private bool _isFinishUIActive = false;

    public void Init(GameManager gameManager, LevelManager levelManager)
    {
        _gameManager = gameManager;
        _levelManager = levelManager;

        ToggleUI(true, false);

        // Base Interface Initialization / »нициализаци€ базового интерфейса
        _respawnButton?.onClick.RemoveAllListeners();
        _respawnButton?.onClick.AddListener(_levelManager.RestartLevel);
        _progressTracker = _progressBar.GetComponent<ProgressTracker>();
        _progressTracker.Init(_gameManager, _levelManager);

        _blackoutCanvasGroup = _blackout.GetComponent<CanvasGroup>();
        if (_blackoutCanvasGroup == null)
        {
            _blackoutCanvasGroup = _blackout.gameObject.AddComponent<CanvasGroup>();
        }

        _returnMainMenuButton?.onClick.RemoveAllListeners();
        _returnMainMenuButton?.onClick.AddListener(() => _levelManager.Load(0));

        // Finish UI Initialization / »нциализаци€ финишного интерфейса
        _restartButton?.onClick.RemoveAllListeners();
        _restartButton?.onClick.AddListener(_levelManager.RestartLevel);
        _restartButtonTransform = _restartButton?.transform;
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
        var controller = _carScript.GetCarController;

        controller.OnSpeedChanged += UpdateSpeedText;
        _gameManager.OnProgressChanged += UpdateProgressBarUI;

        _accelerationPedal?.Init(controller);
        _brakePedal?.Init(controller);
    }

    public void UnsubscribeFromCar(Car car)
    {
        if (car == null) return;
        var controller = car.GetCarController;
        if (controller == null) return;

        controller.OnSpeedChanged -= UpdateSpeedText;
        _gameManager.OnProgressChanged -= UpdateProgressBarUI;
    }

    private void ToggleUI(bool baseInterface, bool finishInterface)
    {
        _baseInterface?.gameObject.SetActive(baseInterface);
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
        if (_carScript != null && _carScript.GetCarController != null)
        {
            _carSpeed.text = $"Speed: {Mathf.RoundToInt(speed)} km/h";
        }
    }

    public void ToggleFinishUI(bool toggle, bool instant = false)
    {
        if (_isFinishUIActive == toggle) return;
        _isFinishUIActive = toggle;

        _blackoutCanvasGroup.DOKill();
        _restartButtonTransform.DOKill();

        if (toggle)
        {
            ToggleUI(false, true);

            _blackoutCanvasGroup?.gameObject.SetActive(true);
            _restartButton?.gameObject.SetActive(true);

            if (instant)
            {
                _blackoutCanvasGroup.alpha = 1f;
                _restartButtonTransform.localScale = Vector3.one;
            }
            else
            {
                _blackoutCanvasGroup.alpha = 0f;
                _restartButtonTransform.localScale = Vector3.zero;

                _blackoutCanvasGroup?.DOFade(1f, 0.1f).SetEase(Ease.InOutQuad);
                _restartButtonTransform.DOScale(1f, 0.6f).SetEase(Ease.OutBack);
            }
        }
        else
        {
            if (instant)
            {
                _blackoutCanvasGroup.alpha = 0f;
                _blackoutCanvasGroup?.gameObject.SetActive(false);

                _restartButtonTransform.localScale = Vector3.zero;
                _restartButton?.gameObject.SetActive(false);
            }
            else
            {
                _blackoutCanvasGroup?.DOFade(0f, 0.5f)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() => _blackoutCanvasGroup.gameObject.SetActive(false));

                _restartButtonTransform.DOScale(0f, 0.4f)
                    .SetEase(Ease.InBack)
                    .OnComplete(() => _restartButton.gameObject.SetActive(false));

                ToggleUI(true, false);
            }
        }
    }
}
