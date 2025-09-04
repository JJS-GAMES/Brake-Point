using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car Settings / Настройки машины")]

    [SerializeField] private CarSettings _carSettings;

    [Header("Camera Settings / Настройки камеры")]

    [SerializeField] private CameraChase2D _cameraChase2D;
    [SerializeField] private Transform _target;
    public Transform Target => _target;

    [SerializeField, Tooltip("Ground Check Trigger / Триггер проверки земли")]
    private GroundCheck _groundCheck;

    private CarController _carController;
    private CarParticleController _carParticleController;
    public CarController GetCarController => _carController;
    private void Awake() // Временно в Update для эскперементов, в будущем ОБЯЗАТЕЛЬНО вернуть в Awake!
    {
        // Physics Settings Initialization
        // Инициализация настроек физики

        if (_carSettings.CarPhysicsMaterial != null) _carSettings.CarPhysicsMaterial.friction = _carSettings.Friction;

        // Car Controller Initialization
        // Инициализация контроллера машинки

        _carController = GetComponentInChildren<CarController>();
        _carParticleController = _carController.GetComponent<CarParticleController>();
        _carController.Init(_groundCheck, _carSettings.IsWorkingEngine, _carSettings.Mass, _carSettings.MaxSpeed, _carSettings.Acceleration, _carSettings.AirSlowEffect, _carSettings.AirBoostEffect, _carSettings.FrontSuspensionStiffness, _carSettings.BackSuspensionStiffness);

        // Car Particle Controller Initialization
        // Инициализация контроллера парртиклов машинки

        _carParticleController.Init(_carController, _groundCheck, _carSettings.DecaySpeed, _carSettings.RestoreSpeed, _carSettings.MinSpeedToEmit);
    }

    private void FixedUpdate()
    {
        if(_cameraChase2D != null && _target != null) _cameraChase2D.Chasing(_target, _carSettings.Offset, _carSettings.Smooth);
    }
}
