using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements / UI Элементы")]
    [Header("Base Interface")]
    [SerializeField] private GameObject _baseInterface;

    [Space, SerializeField] private TextMeshProUGUI _carSpeed;
    [SerializeField] private Scrollbar _progressBar;
    private ProgressTracker _progressTracker;
    [SerializeField] private Button _respawnButton;

    [Space, SerializeField] private AccelerationPedal _accelerationPedal;
    [SerializeField] private BrakePedal _brakePedal;

    [Header("Start UI")]
    [SerializeField] private GameObject _startInterface;
    [SerializeField] private Button[] _selectCarButtons;

    [Header("Finish UI")]
    [SerializeField] private GameObject _finishInterface;

    [Space, SerializeField] private Image _blackout;
    [SerializeField] private Button _restartButton;
    private Transform _restartButtonTransform;

    private GameManager _gameManager;
    private Car _carScript;
    private CanvasGroup _blackoutCanvasGroup;
    private bool _isFinishUIActive = false;

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;


        ToggleUI(true, false, false);

        // Base Interface Initialization / Инициализация базового интерфейса
        _respawnButton?.onClick.RemoveAllListeners();
        _respawnButton?.onClick.AddListener(_gameManager.RestartLevel);
        _progressTracker = _progressBar.GetComponent<ProgressTracker>();
        _progressTracker.Init(_gameManager);

        _blackoutCanvasGroup = _blackout.GetComponent<CanvasGroup>();
        if (_blackoutCanvasGroup == null)
        {
            _blackoutCanvasGroup = _blackout.gameObject.AddComponent<CanvasGroup>();
        }

        // Start UI Initialization / Инциализация стартового интерфейса

        foreach (var b in _selectCarButtons)
        {
            var btn = b;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
                _gameManager.CarInitialization(btn.GetComponent<SelectCarButton>().GetCarPrefab));
            btn.onClick.AddListener(() => ToggleUI(false, true, false));
        }

        // Finish UI Initialization / Инциализация финишного интерфейса
        _restartButton?.onClick.RemoveAllListeners();
        _restartButton?.onClick.AddListener(_gameManager.RestartLevel);
        _restartButtonTransform = _restartButton?.transform;
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

    private void ToggleUI(bool startInterface, bool baseInterface, bool finishInterface)
    {
        _startInterface?.gameObject.SetActive(startInterface);
        _baseInterface?.gameObject.SetActive(baseInterface);
        _finishInterface?.gameObject.SetActive(finishInterface);
    }
    private void UpdateProgressBarUI(float progress)
    {
        if (_gameManager != null && _carScript != null)
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
            ToggleUI(false, false, true);

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

                ToggleUI(false, true, false);
            }
        }
    }
}
