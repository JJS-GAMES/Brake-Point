using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof (Rigidbody2D), typeof(Collider2D))]
public class Barrier : MonoBehaviour
{
    [SerializeField, Range(0f, 100f), Tooltip("Percentage of car speed deceleration / Процент замедления скорости машины")]
    private float _speedReductionPercent = 20f;
    [SerializeField, Range(0f, 10f), Tooltip("Time in seconds before the barrier is disabled after collision / Время в секундах до отключения барьера после столкновения.")]
    private float _deactivationDelay = 5f;
    [SerializeField, Range(1f, 20f), Tooltip("Barrier mass (kg) / Масса барьера (кг)")]
    private float _mass = 1f;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.enabled = true;
        _boxCollider.isTrigger = false;

        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.simulated = true;
        _rigidbody.mass = _mass;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var car = collision.gameObject.GetComponent<CarController>();
        if (car == null) return;

        Rigidbody2D rb = car.GetRb;
        if (rb == null) return;

        float factor = Mathf.Clamp01(1f - _speedReductionPercent / 100f);
        rb.linearVelocity *= factor;

        StartCoroutine(DisableBarrierAfterDelay(_deactivationDelay));
    }

    private IEnumerator DisableBarrierAfterDelay(float delay)
    { 
        _boxCollider.enabled = false;
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }    
}
