using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField, Tooltip("Car Speed (km/h) / —корость машинки (км/ч)")] private float _speed;
    [SerializeField, Tooltip("Ground Check Trigger / “риггер проверки земли")] private GroundCheck _groundCheck;
    [Space]

    [Header("Air Speed Effect")]

    [SerializeField, Tooltip("Deceleration of speed during ascent (when the car is flying up) / «амедление скорости при подъЄме (когда машина летит вверх)")] private float _airSlowEffect = 5f;
    [SerializeField, Tooltip("Acceleration of speed during descent (when the car is flying down) / ”скорение скорости при спуске (когда машина летит вниз)")] private float _airBoostEffect = 12f;
    [Space]

    [Header("Suspension Settings")]
    [SerializeField, Range(3, 12), Tooltip("Stiffness of the front suspension (range 3-12) / ∆есткость передней подвески (диапазон 3Ц12)")] private float _frontSuspensionStiffness = 5f;
    [SerializeField, Range(3, 12), Tooltip("Stiffness of the back suspension (range 3-12) / ∆есткость задней подвески (диапазон 3Ц12)")] private float _backSuspensionStiffness = 5f;

    private CarController _carController;
    public CarController GetCarController => _carController;
    private void Awake()
    {
        // Car Controller Initialization / »нициализаци€ контроллера машинки

        _carController = GetComponentInChildren<CarController>();
        _carController.Init(_groundCheck, _speed, _airSlowEffect, _airBoostEffect, _frontSuspensionStiffness, _backSuspensionStiffness);
    }
}
