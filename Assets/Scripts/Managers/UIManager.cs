using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager; // Temporary initialization in the inspector / Временно инициализация в инспекторе
    [SerializeField] private Car _carScript; // Temporary initialization in the inspector / Временно инициализация в инспекторе
    [Space]
    [Header("UI Elements / UI Элементы")]
    [Header("Base Interfase")]
    [SerializeField] private TextMeshProUGUI _carSpeed;
    [SerializeField] private Scrollbar _progressBar;
    [SerializeField] private Button _respawnButton;

    [Header("Finish UI")]
    [SerializeField] private Image _blackout;
    [SerializeField] private Button _restartButton;

    private void Start()
    {
        // Base Interface Initialization
        // Инициализация базового интерфейса
        _respawnButton?.onClick.RemoveAllListeners();
        _respawnButton?.onClick.AddListener(_gameManager.RestartLevel);

        // Finish UI Initialization
        // Инициализация UI финиша
        ToggleFinishUI(false);
    }
    private void LateUpdate()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        _carSpeed.text = $"Speed: {Mathf.RoundToInt(_carScript.GetCarController.GetRb.linearVelocity.magnitude)} km/h";
        _progressBar.size = _gameManager.CalculateLevelProgress();
    }
    
    public void ToggleFinishUI(bool toggle)
    {
        _blackout?.gameObject.SetActive(toggle);
        _restartButton?.gameObject.SetActive(toggle);
    }
}
