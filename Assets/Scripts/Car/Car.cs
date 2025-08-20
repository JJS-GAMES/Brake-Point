using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField] private float _speed;
    [SerializeField] private GroundCheck _groundCheck;

    [Space]

    [Header("Suspension Settings")]
    [SerializeField][Range(3, 12)] private float _frontSuspensionStiffness = 5f;
    [SerializeField][Range(3, 12)] private float _backSuspensionStiffness = 5f;

    private CarController _carController;
    public CarController GetCarController => _carController;
    private void Awake()
    {
        _carController = GetComponentInChildren<CarController>();
        _carController.Init(_groundCheck, _speed, _frontSuspensionStiffness, _backSuspensionStiffness);
    }
}
