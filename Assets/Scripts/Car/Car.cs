using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car Settings / Настройки машины")]

    [SerializeField] private CarSettings _carSettings;

    [SerializeField, Tooltip("Ground Check Trigger / Триггер проверки земли")]
    public GroundCheck _groundCheck;

    private CarController _carController;
    private CarParticleController _carParticleController;
    public CarController GetCarController => _carController;
    private void Awake()
    {
        // Physics Settings Initialization / Инициализация настроек физики

        if (_carSettings.CarPhysicsMaterial != null) _carSettings.CarPhysicsMaterial.friction = _carSettings.Friction;

        // Car Controller Initialization / Инициализация контроллера машинки

        _carController = GetComponentInChildren<CarController>();
        _carParticleController = _carController.GetComponent<CarParticleController>();
        _carController.Init(_groundCheck, _carSettings.Mass, _carSettings.MaxSpeed, _carSettings.Acceleration, _carSettings.AirSlowEffect, _carSettings.AirBoostEffect, _carSettings.FrontSuspensionStiffness, _carSettings.BackSuspensionStiffness);

        // Car Particle Controller Initialization / Инициализация контроллера парртиклов машинки

        _carParticleController.Init(_carController, _groundCheck, _carSettings.DecaySpeed, _carSettings.RestoreSpeed, _carSettings.MinSpeedToEmit);
    }
}
