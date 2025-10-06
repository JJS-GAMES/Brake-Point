using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car Settings / Настройки машины")]

    [SerializeField] private CarSettings _carSettings;

    [Header("Camera Settings / Настройки камеры")]

    private CameraChase2D _cameraChase2D;
    [SerializeField] private Transform _target;
    public Transform Target => _target;

    [SerializeField, Tooltip("Ground Check Trigger / Триггер проверки земли")]
    private GroundCheck _groundCheck;

    private CarController _carController;
    private CarParticleController _carParticleController;
    private CarEngineSoundController _carEngineSoundController;
    public GroundCheck GroundCheck => _groundCheck;
    public CarController CarController => _carController;
    public CarSettings CarSettings => _carSettings;
    public void Init(Camera camera)
    {
        // Physics Settings Initialization
        // Инициализация настроек физики

        if (_carSettings.CarPhysicsMaterial != null) _carSettings.CarPhysicsMaterial.friction = _carSettings.Friction;

        // Car Controller Initialization
        // Инициализация контроллера машинки

        _carController = GetComponentInChildren<CarController>();
        _carController?.Init
            (
            _groundCheck,
            _carSettings.FrontWheel,
            _carSettings.BackWheel,
            _carSettings.Mass,
            _carSettings.MotorMaximumSpeed,
            _carSettings.MaximumMotorTorque,
            _carSettings.FlipCheckInterval,
            _carSettings.SpeedThreshold,
            _carSettings.UpDotThreshold,
            _carSettings.BreakForce,
            _carSettings.AirTorque,
            _carSettings.FrontSuspensionStiffness,
            _carSettings.BackSuspensionStiffness,
            _carSettings.FrontWheelTraction,
            _carSettings.BackWheelTraction
            );

        _cameraChase2D = camera.GetComponent<CameraChase2D>();  

        // Engine Sound Controller Initialization
        // Инициализация контроллера звука двигателя

        _carEngineSoundController = _carController.GetComponent<CarEngineSoundController>();
        _carEngineSoundController?.Init
            (
            this, 
            _carSettings.EngineSoundClip, 
            _carSettings.MinPitch, 
            _carSettings.GroundMaxPitch, 
            _carSettings.AirMaxPitch, 
            _carSettings.GroundPitchSmooth, 
            _carSettings.AirPitchSmooth
            );

        // Car Particle Controller Initialization
        // Инициализация контроллера парртиклов машинки

        _carParticleController = _carController?.GetComponent<CarParticleController>();
        _carParticleController?.Init
            (
            _carController,
            _groundCheck,
            _carSettings.FrontWheel,
            _carSettings.BackWheel,
            _carSettings.DecaySpeed,
            _carSettings.RestoreSpeed,
            _carSettings.MinSpeedToEmit
            );
    }

    private void FixedUpdate()
    {
        if(_cameraChase2D != null && _target != null) _cameraChase2D.Chasing(_target, _carSettings.Offset, _carSettings.Smooth);
    }
}
