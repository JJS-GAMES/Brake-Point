using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StartZone : MonoBehaviour
{
    private Car _car;
    private BoxCollider2D _boxCollider;
    public void Init(Car car)
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.isTrigger = true;

        _car = car;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_car == null) return;

        if (collision.GetComponent<CarController>() == _car.GetCarController)
        {
            Debug.Log("������� � ���� ������");
            _car.GetCarController.ToggleEngineWork(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_car == null) return;

        if (collision.GetComponent<CarController>() == _car.GetCarController)
        {
            Debug.Log("������� �������� ���� ������");
            _car.GetCarController.ToggleEngineWork(false);
        }
    }
}
