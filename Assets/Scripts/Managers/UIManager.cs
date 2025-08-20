using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Car _carScript;
    [SerializeField] private TextMeshProUGUI _carSpeed;

    private void LateUpdate()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        _carSpeed.text =  $"{Mathf.RoundToInt(_carScript.GetCarController.GetRb.linearVelocity.magnitude)} km/h";
    }
}
