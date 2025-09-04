using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StartZone : MonoBehaviour
{
    private Car _car;

    public void Init(Car car)
    {
        _car = car;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_car == null) return;

        if (collision.GetComponent<CarController>() == _car.GetCarController)
        {
            Debug.Log("Машинка в зоне старта");
            _car.GetCarController.ToggleEngineWork(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_car == null) return;

        if (collision.GetComponent<CarController>() == _car.GetCarController)
        {
            Debug.Log("Машинка покинула зону старта");
            _car.GetCarController.ToggleEngineWork(false);
        }
    }
}
