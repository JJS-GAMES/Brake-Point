using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Dirt : MonoBehaviour
{
    [SerializeField, Range(0f, 100f), Tooltip("Percentage of car speed deceleration / Процент замедления скорости машины")]
    private float _speedReductionPercent = 1f;

    private BoxCollider2D _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.enabled = true;
        _boxCollider.isTrigger = true;
    }

    private void OnTriggerStay2D (Collider2D other)
    {
        var car = other.gameObject.GetComponent<CarController>();
        if (car == null) return;

        Rigidbody2D rb = car.GetRb;
        if (rb == null) return;

        float factor = Mathf.Clamp01(1f - _speedReductionPercent / 100f);
        rb.linearVelocity *= factor;
    }
}
