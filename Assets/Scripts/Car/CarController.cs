using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour
{
    [Header("Suspensions")]
    [Space]

    [SerializeField, Tooltip("Front suspension component / Компонент передней подвески")] private WheelJoint2D _frontSuspension;
    [SerializeField, Tooltip("Back suspension component / Компонент задней подвески")] private WheelJoint2D _backSuspension;

    private float _airSlowEffect = 5f;
    private float _airBoostEffect = 12f;

    private float _maxSpeed;
    private float _acceleration;
    private GroundCheck _groundCheck;
    private Rigidbody2D _rb;
    public Rigidbody2D GetRb => _rb;

    public void Init(GroundCheck groundCheck, float maxSpeed, float acceleration, float airSlowEffect, float airBoostEffect, float frontSuspensionStiffness, float backSuspensionStiffness)
    {
        _rb = GetComponent<Rigidbody2D>();

        _maxSpeed = maxSpeed;
        _acceleration = acceleration;
        _groundCheck = groundCheck;

        // Air Speed / Скорость в воздухе

        _airSlowEffect = airSlowEffect;
        _airBoostEffect = airBoostEffect;

        // Suspension Settings / Настройки подвески

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
        else
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

        float targetX = transform.right.x * _maxSpeed;

        if (Mathf.Abs(velocity.x) < Mathf.Abs(targetX))
        {
            velocity.x += Mathf.Sign(targetX) * _acceleration * Time.fixedDeltaTime;
        }

        velocity.x = Mathf.Clamp(velocity.x, -_maxSpeed, _maxSpeed);

        _rb.linearVelocity = velocity;
    }


}
