using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager; // Temporary initialization in the inspector / Временно инициализация в инспекторе
    [SerializeField] private Car _carScript; // Temporary initialization in the inspector / Временно инициализация в инспекторе
    [Space]
    [SerializeField] private TextMeshProUGUI _carSpeed;
    [SerializeField] private Button _respawnButton;

    private void Start()
    {
        _respawnButton?.onClick.RemoveAllListeners();
        _respawnButton?.onClick.AddListener(_gameManager.RestartLevel);
    }
    private void LateUpdate()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        _carSpeed.text = $"Speed: {Mathf.RoundToInt(_carScript.GetCarController.GetRb.linearVelocity.magnitude)} km/h";
    }
}
