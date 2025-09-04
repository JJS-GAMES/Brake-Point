using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour
{
    [Header("Suspensions / Подвески")]
    [Space]

    [SerializeField, Tooltip("Front suspension component / Компонент передней подвески")] private WheelJoint2D _frontSuspension;
    [SerializeField, Tooltip("Back suspension component / Компонент задней подвески")] private WheelJoint2D _backSuspension;

    private float _airSlowEffect = 5f;
    private float _airBoostEffect = 12f;

    private float _mass = 10;
    private float _engineMaxSpeed = 10;
    private float _coastMaxSpeed = 10;
    private float _acceleration = 10;

    private bool _isWorkingEngine = true;

    private GroundCheck _groundCheck;
    private Rigidbody2D _rb;
    public Rigidbody2D GetRb => _rb;

    public void Init(GroundCheck groundCheck, bool isWorkingEngine, float mass, float engineMaxSpeed, float coastMaxSpeed, float acceleration, float airSlowEffect, float airBoostEffect, float frontSuspensionStiffness, float backSuspensionStiffness)
    {
        _rb = GetComponent<Rigidbody2D>();

        // Main Settings
        // Основные настройки

        _isWorkingEngine = isWorkingEngine;

        _mass = mass;
        _rb.mass = _mass;
        _engineMaxSpeed = engineMaxSpeed;
        _coastMaxSpeed = coastMaxSpeed;
        _acceleration = acceleration;
        _groundCheck = groundCheck;

        // Air Speed
        // Скорость в воздухе

        _airSlowEffect = airSlowEffect;
        _airBoostEffect = airBoostEffect;

        // Suspension Settings
        // Настройки подвески

        var frontSuspensionSettings = _frontSuspension.suspension;
        frontSuspensionSettings.frequency = frontSuspensionStiffness;
        _frontSuspension.suspension = frontSuspensionSettings;

        var backSuspensionSettings = _backSuspension.suspension;
        backSuspensionSettings.frequency = backSuspensionStiffness;
        _backSuspension.suspension = backSuspensionSettings;
    }

    private void FixedUpdate()
    {
        if (_groundCheck.IsGround)
        {
            Move();
        }
        else if(!_groundCheck.IsGround)
        {
            ApplyAirSpeedEffect();
        }
    }

    private void ApplyAirSpeedEffect()
    {
        float tilt = transform.up.y;

        Vector2 newVelocity = _rb.linearVelocity;

        float airDrag = 2f;
        if (Mathf.Abs(newVelocity.x) > 0.01f)
        {
            newVelocity.x -= Mathf.Sign(newVelocity.x) * airDrag * Time.fixedDeltaTime;
        }

        if (tilt > 0f)
            newVelocity.x += -tilt * _airSlowEffect * Time.fixedDeltaTime;
        else if (tilt < 0f)
            newVelocity.x += -tilt * _airBoostEffect * Time.fixedDeltaTime;

        _rb.linearVelocity = newVelocity;
    }


    private void Move()
    {
        Vector2 velocity = _rb.linearVelocity;

        if (_isWorkingEngine) // If the engine is running, push the car forward. / Если двигатель работает, толкаем машинку вперед
        {
            float targetX = transform.right.x * _engineMaxSpeed;

            if (Mathf.Abs(velocity.x) < Mathf.Abs(targetX))
            {
                velocity.x += Mathf.Sign(targetX) * _acceleration * Time.fixedDeltaTime;
            }

            velocity.x = Mathf.Clamp(velocity.x, -_engineMaxSpeed, _engineMaxSpeed);
        }
        else // If the engine is not running, the car is traveling by inertia. / Если двигатель не работает, машинка едет по инерции.
        {
            float max = _coastMaxSpeed;
            velocity.x = Mathf.MoveTowards(velocity.x, Mathf.Clamp(velocity.x, -max, max), _acceleration * Time.fixedDeltaTime);
        }

        _rb.linearVelocity = velocity;
    }

    public void ToggleEngineWork(bool toggle)
    {
        _isWorkingEngine = toggle;
    }
}
