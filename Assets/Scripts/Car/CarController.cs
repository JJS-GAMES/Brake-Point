using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour
{
    [Header("Suspensions / Подвески")]
    [Space]

    [Space, SerializeField, Tooltip("Front suspension component / Компонент передней подвески")] private WheelJoint2D _frontSuspensionWheelJoint;
    [SerializeField, Tooltip("Back suspension component / Компонент задней подвески")] private WheelJoint2D _backSuspensionWheelJoint;

    private float _mass = 10;

    private float _engineMaxSpeed = 10f;
    private float _coastMaxSpeed = 10f;
    private float _acceleration = 10f;

    private float _brakeForce = 2f;

    private float _airTorque = 20f;
    private float _airSlowEffect = 5f;
    private float _airBoostEffect = 12f;

    private bool _isWorkingEngine;
    private bool _isBraking;
    private GroundCheck _groundCheck;
    private Rigidbody2D _rb;
    public Rigidbody2D GetRb => _rb;
    public bool GetIsWorkingEngine => _isWorkingEngine;
    public bool GetIsBraking => _isBraking;

    public void Init(GroundCheck groundCheck, float mass, float engineMaxSpeed, float coastMaxSpeed, float acceleration, float brakeForce, float airTorque, float airSlowEffect, float airBoostEffect, float frontSuspensionStiffness, float backSuspensionStiffness)
    {
        _rb = GetComponent<Rigidbody2D>();

        // Main Settings
        // Основные настройки

        _mass = mass;
        _rb.mass = _mass;

        _engineMaxSpeed = engineMaxSpeed;
        _coastMaxSpeed = coastMaxSpeed;
        _acceleration = acceleration;
        _groundCheck = groundCheck;

        _brakeForce = brakeForce;

        // Air Speed
        // Скорость в воздухе

        _airTorque = airTorque;
        _airSlowEffect = airSlowEffect;
        _airBoostEffect = airBoostEffect;

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

        float tilt = transform.up.y;
        Vector2 newVelocity = _rb.linearVelocity;

        float airDrag = 2f;
        if (Mathf.Abs(newVelocity.x) > 0.01f)
            newVelocity.x -= Mathf.Sign(newVelocity.x) * airDrag * Time.fixedDeltaTime;

        if (tilt > 0f)
            newVelocity.x += -tilt * _airSlowEffect * Time.fixedDeltaTime;
        else if (tilt < 0f)
            newVelocity.x += -tilt * _airBoostEffect * Time.fixedDeltaTime;

        _rb.linearVelocity = newVelocity;
    }



    private void Move()
    {
        if (!_groundCheck.IsGround) return;

        Vector2 velocity = _rb.linearVelocity;

        if (_isWorkingEngine && !_isBraking) // If the engine is running, push the car forward. / Если двигатель работает, толкаем машинку вперед.
        {
            float targetX = transform.right.x * _engineMaxSpeed;

            if (Mathf.Abs(velocity.x) < Mathf.Abs(targetX))
            {
                velocity.x += Mathf.Sign(targetX) * _acceleration * Time.fixedDeltaTime;
            }

            velocity.x = Mathf.Clamp(velocity.x, -_engineMaxSpeed, _engineMaxSpeed);
        }
        else if (_isBraking) // If we press the brake, we smoothly stop the car.. / Если мы жмем тормоз, плавно останавливаем машинку.
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0f, _acceleration * _brakeForce * Time.fixedDeltaTime);

            if (Mathf.Abs(velocity.x) < 0.05f)
                velocity.x = 0f;
        }
        else // If the engine is not running, the car is traveling by inertia. / Если двигатель не работает, машинка едет по инерции.
        {
            float max = _coastMaxSpeed;
            velocity.x = Mathf.MoveTowards(velocity.x, Mathf.Clamp(velocity.x, -max, max), _acceleration * Time.fixedDeltaTime);
        }

        _rb.linearVelocity = velocity;
    }

    public void ToggleWorkingEngine(bool toggle)
    {
        _isWorkingEngine = toggle;
    }
    public void ToggleBraking(bool toggle)
    {
        _isBraking = toggle;
    }
}
