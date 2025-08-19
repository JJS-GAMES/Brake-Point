using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour
{
    [SerializeField] private float _speed = 20f;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        Vector2 direction = transform.right;
        _rb.MovePosition(_rb.position + direction * _speed * Time.fixedDeltaTime);
    }
}