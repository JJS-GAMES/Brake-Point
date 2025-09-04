using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FinishZone : MonoBehaviour
{
    private Car _car;
    private BoxCollider2D _boxCollider;
    private UIManager _uiManager;

    public void Init(Car car, UIManager uiManager)
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.isTrigger = true;

        _car = car;
        _uiManager = uiManager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_car == null) return;

        if (collision.GetComponent<CarController>() == _car.GetCarController)
        {
            Debug.Log("Машинка в зоне финиша");

            _uiManager?.ToggleFinishUI(true);
        }
    }
}
