using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CarController _carScript;
    [SerializeField] private TextMeshProUGUI _carSpeed;

    private void LateUpdate()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        _carSpeed.text =  $"{Mathf.RoundToInt(_carScript.GetSpeed)} km/h";
    }
}
