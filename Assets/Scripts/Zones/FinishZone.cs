using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FinishZone : MonoBehaviour
{
    private Car _car;
    private BoxCollider2D _boxCollider;

    public void Init(Car car)
    {
        _boxCollider = GetComponent<BoxCollider2D>();

        _car = car;
    }
}
