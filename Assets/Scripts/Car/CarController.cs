using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour
{
    [SerializeField] private WheelJoint2D _frontSuspension;
    [SerializeField] private WheelJoint2D _backSuspension;

    private float _speed = 20f;
    private GroundCheck _groundCheck;
    private Rigidbody2D _rb;
    public float GetSpeed => _speed;

    public void Init(GroundCheck groundCheck, float speed, float frontSuspensionStiffness, float backSuspensionStiffness)
    {
        _rb = GetComponent<Rigidbody2D>();

        _speed = speed;
        _groundCheck = groundCheck;

        var frontSuspension = _frontSuspension.suspension;
        frontSuspension.frequency = frontSuspensionStiffness;
        _frontSuspension.suspension = frontSuspension;

        var backSuspension = _backSuspension.suspension;
        backSuspension.frequency = backSuspensionStiffness;
        _backSuspension.suspension = backSuspension;
    }

    private void FixedUpdate()
    {
        if (_groundCheck.IsGround)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector2 direction = new Vector2(transform.right.x, 0f);
        _rb.linearVelocity = direction * _speed + new Vector2(0f, _rb.linearVelocity.y);
    }
}
