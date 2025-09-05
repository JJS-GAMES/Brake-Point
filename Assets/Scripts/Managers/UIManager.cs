using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Car _carScript;
    [Space]
    [Header("UI Elements / UI Ёлементы")]
    [Header("Base Interface")]
    [SerializeField] private GameObject _baseInterface;

    [Space, SerializeField] private TextMeshProUGUI _carSpeed;
    [SerializeField] private Scrollbar _progressBar;
    [SerializeField] private Button _respawnButton;

    [Header("Finish UI")]
    [SerializeField] private GameObject _finishInterface;

    [Space, SerializeField] private Image _blackout;
    [SerializeField] private Button _restartButton;

    private CanvasGroup _blackoutCanvasGroup;
    private bool _isFinishUIActive = false;

    private void Start()
    {
        // Base Interface Initialization
        _respawnButton?.onClick.RemoveAllListeners();
        _respawnButton?.onClick.AddListener(_gameManager.RestartLevel);

        _blackoutCanvasGroup = _blackout.GetComponent<CanvasGroup>();
        if (_blackoutCanvasGroup == null)
        {
            _blackoutCanvasGroup = _blackout.gameObject.AddComponent<CanvasGroup>();
        }

        // Finish UI Initialization
        _restartButton?.onClick.RemoveAllListeners();
        _restartButton?.onClick.AddListener(_gameManager.RestartLevel);

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
        _restartButton.transform.DOKill();

        if (toggle)
        {
            _baseInterface.gameObject.SetActive(false);
            _finishInterface?.gameObject.SetActive(true);

            _blackoutCanvasGroup?.gameObject.SetActive(true);
            _restartButton?.gameObject.SetActive(true);

            if (instant)
            {
                _blackoutCanvasGroup.alpha = 1f;
                _restartButton.transform.localScale = Vector3.one;
            }
            else
            {
                _blackoutCanvasGroup.alpha = 0f;
                _restartButton.transform.localScale = Vector3.zero;

                _blackoutCanvasGroup?.DOFade(1f, 0.1f).SetEase(Ease.InOutQuad);
                _restartButton?.transform.DOScale(1f, 0.6f).SetEase(Ease.OutBack);
            }
        }
        else
        {
            if (instant)
            {
                _blackoutCanvasGroup.alpha = 0f;
                _blackoutCanvasGroup?.gameObject.SetActive(false);

                _restartButton.transform.localScale = Vector3.zero;
                _restartButton?.gameObject.SetActive(false);
            }
            else
            {
                _blackoutCanvasGroup?.DOFade(0f, 0.5f)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() => _blackoutCanvasGroup.gameObject.SetActive(false));

                _restartButton?.transform.DOScale(0f, 0.4f)
                    .SetEase(Ease.InBack)
                    .OnComplete(() => _restartButton.gameObject.SetActive(false));

                _finishInterface?.gameObject.SetActive(false);
                _baseInterface?.gameObject.SetActive(true);
            }
        }
    }
}
