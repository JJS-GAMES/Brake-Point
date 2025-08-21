using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField, Tooltip("Car mass (kg) / Масса машины (кг)")] private float _mass = 10f;
    [SerializeField, Tooltip("Car Speed (km/h) / Максимальная cкорость машинки (км/ч)")] private float _maxSpeed = 10f;
    [SerializeField, Tooltip("Acceleration (higher value = faster speed gain) / Ускорение (чем выше значение, тем быстрее разгон)")] private float _acceleration = 10f;

    [SerializeField, Tooltip("Ground Check Trigger / Триггер проверки земли")] private GroundCheck _groundCheck;
    [Space]

    [Header("Air Speed Effect")]

    [SerializeField, Tooltip("Deceleration of speed during ascent (when the car is flying up) / Замедление скорости при подъёме (когда машина летит вверх)")] private float _airSlowEffect = 5f;
    [SerializeField, Tooltip("Acceleration of speed during descent (when the car is flying down) / Ускорение скорости при спуске (когда машина летит вниз)")] private float _airBoostEffect = 12f;
    [Space]

    [Header("Suspension Settings")]
    [SerializeField, Range(3, 12), Tooltip("Stiffness of the front suspension (range 3-12) / Жесткость передней подвески (диапазон 3–12)")] private float _frontSuspensionStiffness = 5f;
    [SerializeField, Range(3, 12), Tooltip("Stiffness of the back suspension (range 3-12) / Жесткость задней подвески (диапазон 3–12)")] private float _backSuspensionStiffness = 5f;

    private CarController _carController;
    public CarController GetCarController => _carController;
    private void Awake()
    {
        // Car Controller Initialization / Инициализация контроллера машинки

        _carController = GetComponentInChildren<CarController>();
        _carController.Init(_groundCheck, _mass, _maxSpeed, _acceleration, _airSlowEffect, _airBoostEffect, _frontSuspensionStiffness, _backSuspensionStiffness);
    }
}
