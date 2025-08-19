using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour
{
    [SerializeField] private float _speed = 20f;
    [SerializeField] private GroundCheck _groundCheck;
    public float GetSpeed => _speed;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
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
