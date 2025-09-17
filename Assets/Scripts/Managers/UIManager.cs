using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements / UI Ёлементы")]
    [Header("Base Interface")]
    [SerializeField] private GameObject _baseInterface;

    [Space, SerializeField] private TextMeshProUGUI _carSpeed;
    [SerializeField] private Scrollbar _progressBar;
    [SerializeField] private Button _respawnButton;

    [Space, SerializeField] private AccelerationPedal _accelerationPedal;
    [SerializeField] private BrakePedal _brakePedal;

    [Header("Finish UI")]
    [SerializeField] private GameObject _finishInterface;

    [Space, SerializeField] private Image _blackout;
    [SerializeField] private Button _restartButton;
    private Transform _restartButtonTransform;

    private GameManager _gameManager;
    private Car _carScript;
    private CanvasGroup _blackoutCanvasGroup;
    private bool _isFinishUIActive = false;

    public void Init(GameManager gameManager, Car carScript)
    {
        _carScript = carScript;
        _gameManager = gameManager;

        // Base Interface Initialization / »нициализаци€ базового интерфейса
        _respawnButton?.onClick.RemoveAllListeners();
        _respawnButton?.onClick.AddListener(_gameManager.RestartLevel);

        _blackoutCanvasGroup = _blackout.GetComponent<CanvasGroup>();
        if (_blackoutCanvasGroup == null)
        {
            _blackoutCanvasGroup = _blackout.gameObject.AddComponent<CanvasGroup>();
        }

        _accelerationPedal?.Init(_carScript.GetCarController);
        _brakePedal?.Init(_carScript.GetCarController);

        // Finish UI Initialization / »нциализаци€ финишного интерфейса
        _restartButton?.onClick.RemoveAllListeners();
        _restartButton?.onClick.AddListener(_gameManager.RestartLevel);
        _restartButtonTransform = _restartButton?.transform;
        ToggleFinishUI(false, true);
    }

    private void LateUpdate()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_carScript != null && _carScript.GetCarController != null)
        {
            _carSpeed.text = $"Speed: {Mathf.RoundToInt(_carScript.GetCarController.GetRb.linearVelocity.magnitude)} km/h";
        }

        if (_gameManager != null)
        {
            _progressBar.size = _gameManager.CalculateLevelProgress();
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
            _baseInterface.gameObject.SetActive(false);
            _finishInterface?.gameObject.SetActive(true);

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

                _finishInterface?.gameObject.SetActive(false);
                _baseInterface?.gameObject.SetActive(true);
            }
        }
    }
}
