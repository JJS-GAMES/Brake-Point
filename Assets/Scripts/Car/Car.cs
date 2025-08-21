using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField, Tooltip("Car mass (kg) / ����� ������ (��)")] private float _mass = 10f;
    [SerializeField, Tooltip("Car Speed (km/h) / ������������ c������� ������� (��/�)")] private float _maxSpeed = 10f;
    [SerializeField, Tooltip("Acceleration (higher value = faster speed gain) / ��������� (��� ���� ��������, ��� ������� ������)")] private float _acceleration = 10f;

    [SerializeField, Tooltip("Ground Check Trigger / ������� �������� �����")] private GroundCheck _groundCheck;
    [Space]

    [Header("Air Speed Effect")]

    [SerializeField, Tooltip("Deceleration of speed during ascent (when the car is flying up) / ���������� �������� ��� ������� (����� ������ ����� �����)")] private float _airSlowEffect = 5f;
    [SerializeField, Tooltip("Acceleration of speed during descent (when the car is flying down) / ��������� �������� ��� ������ (����� ������ ����� ����)")] private float _airBoostEffect = 12f;
    [Space]

    [Header("Suspension Settings")]
    [SerializeField, Range(3, 12), Tooltip("Stiffness of the front suspension (range 3-12) / ��������� �������� �������� (�������� 3�12)")] private float _frontSuspensionStiffness = 5f;
    [SerializeField, Range(3, 12), Tooltip("Stiffness of the back suspension (range 3-12) / ��������� ������ �������� (�������� 3�12)")] private float _backSuspensionStiffness = 5f;

    private CarController _carController;
    public CarController GetCarController => _carController;
    private void Awake()
    {
        // Car Controller Initialization / ������������� ����������� �������

        _carController = GetComponentInChildren<CarController>();
        _carController.Init(_groundCheck, _mass, _maxSpeed, _acceleration, _airSlowEffect, _airBoostEffect, _frontSuspensionStiffness, _backSuspensionStiffness);
    }
}
