using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DeathZone : MonoBehaviour
{
    private BoxCollider2D _boxCollider;
    private Car _car;
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
            _uiManager?.ToggleDefeatUI(true);
        }
    }
}
