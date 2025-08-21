using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour
{
    [Header("Suspensions")]
    [Space]

    [SerializeField, Tooltip("Front suspension component / ��������� �������� ��������")] private WheelJoint2D _frontSuspension;
    [SerializeField, Tooltip("Back suspension component / ��������� ������ ��������")] private WheelJoint2D _backSuspension;

    private float _airSlowEffect = 5f;
    private float _airBoostEffect = 12f;

    private float _speed = 20f;
    private GroundCheck _groundCheck;
    private Rigidbody2D _rb;
    public Rigidbody2D GetRb => _rb;

    public void Init(GroundCheck groundCheck, float speed, float airSlowEffect, float airBoostEffect, float frontSuspensionStiffness, float backSuspensionStiffness)
    {
        _rb = GetComponent<Rigidbody2D>();

        _speed = speed;
        _groundCheck = groundCheck;

        // AirSpeed / �������� � �������

        _airSlowEffect = airSlowEffect;
        _airBoostEffect = airBoostEffect;

        // Suspension Settings / ��������� ��������

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
        Vector2 direction = new Vector2(transform.right.x, 0f);
        _rb.linearVelocity = direction * _speed + new Vector2(0f, _rb.linearVelocity.y);
    }

}
