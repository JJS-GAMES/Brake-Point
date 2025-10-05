using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FinishZone : MonoBehaviour
{
    private BoxCollider2D _boxCollider;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<CarController>(out CarController carController))
        {
            carController.enabled = false;

            UIManager.Instance?.ToggleFinishUI(true);
        }
    }

}
