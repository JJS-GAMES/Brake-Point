using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Barrier : MonoBehaviour
{
    [Header("Percentage of car speed deceleration / Процент замедления скорости машины")]
    [Range(0f, 100f)]
    [SerializeField] private float _speedReductionPercent = 20f;

    private BoxCollider2D _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var car = other.GetComponent<CarController>();
        if (car == null) return;

        Rigidbody2D rb = car.GetRb;
        if (rb == null) return;

        float factor = Mathf.Clamp01(1f - _speedReductionPercent / 100f);
        rb.linearVelocity *= factor;

        gameObject.SetActive(false);
    }
}
