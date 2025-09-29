using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car Settings / ��������� ������")]

    [SerializeField] private CarSettings _carSettings;

    [Header("Camera Settings / ��������� ������")]

    [SerializeField] private CameraChase2D _cameraChase2D;
    [SerializeField] private Transform _target;
    public Transform Target => _target;

    [SerializeField, Tooltip("Ground Check Trigger / ������� �������� �����")]
    private GroundCheck _groundCheck;

    private CarController _carController;
    private CarParticleController _carParticleController;
    private CarEngineSoundController _carEngineSoundController;
    public GroundCheck GetGroundCheck => _groundCheck;
    public CarController GetCarController => _carController;
    public CarSettings GetCarSettings => _carSettings;
    public void Init(Camera camera)
    {
        // Physics Settings Initialization
        // ������������� �������� ������

        if (_carSettings.CarPhysicsMaterial != null) _carSettings.CarPhysicsMaterial.friction = _carSettings.Friction;

        // Car Controller Initialization
        // ������������� ����������� �������

        _carController = GetComponentInChildren<CarController>();
        _carController?.Init(_groundCheck, _carSettings.Mass, _carSettings.EngineMaxSpeed, _carSettings.CoastMaxSpeed, _carSettings.Acceleration, _carSettings.FlipCheckInterval, _carSettings.SpeedThreshold, _carSettings.UpDotThreshold, _carSettings.BrakeForce, _carSettings.AirTorque, _carSettings.AirSlowEffect, _carSettings.AirBoostEffect, _carSettings.FrontSuspensionStiffness, _carSettings.BackSuspensionStiffness);

        _cameraChase2D = camera.GetComponent<CameraChase2D>();  

        // Engine Sound Controller Initialization
        // ������������� ����������� ����� ���������

        _carEngineSoundController = _carController.GetComponent<CarEngineSoundController>();
        _carEngineSoundController?.Init(this, _carSettings.EngineSoundClip, _carSettings.MinPitch, _carSettings.GroundMaxPitch, _carSettings.AirMaxPitch, _carSettings.GroundPitchSmooth, _carSettings.AirPitchSmooth);

        // Car Particle Controller Initialization
        // ������������� ����������� ���������� �������

        _carParticleController = _carController?.GetComponent<CarParticleController>();
        _carParticleController?.Init(_carController, _groundCheck, _carSettings.DecaySpeed, _carSettings.RestoreSpeed, _carSettings.MinSpeedToEmit);
    }

    private void FixedUpdate()
    {
        if(_cameraChase2D != null && _target != null) _cameraChase2D.Chasing(_target, _carSettings.Offset, _carSettings.Smooth);
    }
}
