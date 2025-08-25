using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Car _carScript; // Temporary initialization in the inspector / Временно инициализация в инспекторе
    [SerializeField] private TextMeshProUGUI _carSpeed;

    private void LateUpdate()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        _carSpeed.text =  $"Speed: {Mathf.RoundToInt(_carScript.GetCarController.GetRb.linearVelocity.magnitude)} km/h";
    }
}
