using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour
{
    [Header("Suspensions / Подвески")]
    [Space]

    [Space, SerializeField, Tooltip("Front suspension component / Компонент передней подвески")] private WheelJoint2D _frontSuspensionWheelJoint;
    [SerializeField, Tooltip("Back suspension component / Компонент задней подвески")] private WheelJoint2D _backSuspensionWheelJoint;

    private float _mass = 10f;

    private float _motorMaxSpeed = 10f;

    private float _flipCheckInterval = 0.2f;
    private float _speedThreshold = 0.1f;
    private float _upDotThreshold = -0.8f;
    private float _flipTimer;

    private float _brakeForce = 2f;

    private float _airTorque = 20f;

    private bool _frontWheel = false;
    private bool _backWheel = true;

    private bool _isWorkingEngine;
    private bool _isBraking;

    private bool _engineFromKeyboard;
    private bool _engineFromUI;

    private bool _brakeFromKeyboard;
    private bool _brakeFromUI;

    private GroundCheck _groundCheck;
    private Rigidbody2D _rb;

    public Rigidbody2D GetRb => _rb;
    public bool IsWorkingEngine => _isWorkingEngine;
    public bool IsBraking => _isBraking;

    public event Action<float> OnSpeedChanged;
    public event Action OnCarDefeated;

    public void Init(GroundCheck groundCheck, bool frontWheel, bool backWheel, float mass, float motorMaxSpeed, float flipCheckInterval, float speedThreshold, float upDotThreshold, float brakeForce, float airTorque, float frontSuspensionStiffness, float backSuspensionStiffness)
    {
        _rb = GetComponent<Rigidbody2D>();

        // Main Settings
        // Основные настройки

        _frontWheel = frontWheel;
        _backWheel = backWheel;

        _mass = mass;
        _rb.mass = _mass;

        _motorMaxSpeed = motorMaxSpeed;
        _groundCheck = groundCheck;

        _flipCheckInterval = flipCheckInterval;
        _speedThreshold = speedThreshold;
        _upDotThreshold = upDotThreshold;

        _brakeForce = brakeForce;

        // Air Speed
        // Скорость в воздухе

        _airTorque = airTorque;

        // Suspension Settings
        // Настройки подвески

        var frontSuspensionSettings = _frontSuspensionWheelJoint.suspension;
        frontSuspensionSettings.frequency = frontSuspensionStiffness;
        _frontSuspensionWheelJoint.suspension = frontSuspensionSettings;
        _frontSuspensionWheelJoint.anchor = _frontSuspensionWheelJoint.connectedBody.transform.parent.localPosition;

        var backSuspensionSettings = _backSuspensionWheelJoint.suspension;
        backSuspensionSettings.frequency = backSuspensionStiffness;
        _backSuspensionWheelJoint.suspension = backSuspensionSettings;
        _backSuspensionWheelJoint.anchor = _backSuspensionWheelJoint.connectedBody.transform.parent.localPosition;

        _rb.centerOfMass = new Vector2(0, -0.2f);
    }
    private void Update()
    {
        SetEngineFromKeyboard(Input.GetKey(KeyCode.RightArrow));
        SetBrakeFromKeyboard(Input.GetKey(KeyCode.LeftArrow));

        _isWorkingEngine = _engineFromKeyboard || _engineFromUI;
        _isBraking = _brakeFromKeyboard || _brakeFromUI;

        CheckForFlipAndStop();
    }
    private void FixedUpdate()
    {
        if(_groundCheck.IsGround )
        {
            Move();
        }    
        else if (!_groundCheck.IsGround)
        {
            ApplyAirSpeedEffect();
        }
    }

    private void ApplyAirSpeedEffect()
    {
        _rb.angularVelocity = Mathf.MoveTowards(_rb.angularVelocity, 0f, _rb.angularDamping * Time.fixedDeltaTime);

        if (_isWorkingEngine && !_isBraking) // If the engine is running, gently turn the machine forward. / Если двигатель работает, плавно поворачиваем машинку вперед.
        {
            _rb.AddTorque(-_airTorque * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        else if (_isBraking) // If we press the brake, we can smoothly turn the car back. / Если мы жмем тормоз, плавно поворачиваем машинку назад.
        {
            _rb.AddTorque(_airTorque * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }

    private void CheckForFlipAndStop()
    {
        _flipTimer += Time.deltaTime;
        if (_flipTimer < _flipCheckInterval) return;
        _flipTimer = 0f;

        bool isStopped = _rb.linearVelocity.magnitude / 3.6f < _speedThreshold;

        bool isUpsideDown = Vector2.Dot(transform.up, Vector2.up) < _upDotThreshold;

        if (isStopped && isUpsideDown)
        {
            OnCarDefeated?.Invoke();
        }
    }

    private void Move()
    {
        float currentSpeedKmh = _rb.linearVelocity.magnitude * 3.6f;
        OnSpeedChanged?.Invoke(currentSpeedKmh);

        if (!_groundCheck.IsGround) return;

        var motorFront = _frontSuspensionWheelJoint.motor;
        var motorBack = _backSuspensionWheelJoint.motor;

        _frontSuspensionWheelJoint.useMotor = false;
        _backSuspensionWheelJoint.useMotor = false;

        if (_isWorkingEngine && !_isBraking && currentSpeedKmh < _motorMaxSpeed) // If the engine is running, push the car forward. / Если двигатель работает, толкаем машинку вперед.
        {
            if (_frontWheel)
            {
                _frontSuspensionWheelJoint.useMotor = true;

                motorFront.motorSpeed = -_motorMaxSpeed * 100f;
                motorFront.maxMotorTorque = 500f;

                _frontSuspensionWheelJoint.motor = motorFront;
            }
            if (_backWheel)
            {
                _backSuspensionWheelJoint.useMotor = true;

                motorBack.motorSpeed = -_motorMaxSpeed * 100f;
                motorBack.maxMotorTorque = 500f;

                _backSuspensionWheelJoint.motor = motorBack;
            }
        }
        else if (_isBraking) // If we press the brake, we smoothly stop the car. / Если мы жмем тормоз, плавно останавливаем машинку.
        {
            if (_frontWheel)
            {
                _frontSuspensionWheelJoint.useMotor = true;

                motorFront.motorSpeed = 0;
                motorFront.maxMotorTorque = _brakeForce * 100f;

                _frontSuspensionWheelJoint.motor = motorFront;
            }
            if (_backWheel)
            {
                _backSuspensionWheelJoint.useMotor = true;

                motorBack.motorSpeed = 0;
                motorBack.maxMotorTorque = _brakeForce * 100f;

                _backSuspensionWheelJoint.motor = motorBack;
            }
        }
        else
        {
            _frontSuspensionWheelJoint.useMotor = false;
            _backSuspensionWheelJoint.useMotor = false;
        }
    }

    public void ToggleWorkingEngine(bool toggle) => _isWorkingEngine = toggle;
    public void ToggleBraking(bool toggle) => _isBraking = toggle;
    public void SetEngineFromKeyboard(bool state) => _engineFromKeyboard = state;
    public void SetEngineFromUI(bool state) => _engineFromUI = state;
    public void SetBrakeFromKeyboard(bool state) => _brakeFromKeyboard = state;
    public void SetBrakeFromUI(bool state) => _brakeFromUI = state;
}
