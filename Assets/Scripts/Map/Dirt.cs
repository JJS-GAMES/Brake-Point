using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Dirt : MonoBehaviour
{
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

        float mudTraction = car.GetComponentInParent<Car>().CarSettings.MudTraction;

        float factor = Mathf.Clamp01(mudTraction / 100f);

        rb.linearVelocity *= factor;
    }
}
