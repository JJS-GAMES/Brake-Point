using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FinishZone : MonoBehaviour
{
    [SerializeField, Tooltip("If the car's speed is equal to or below this value, then a successful finish is counted. / Если скорость машинки равна или ниже этого значения, то засчитывается успешный финиш")]
    private float _finishSpeedThreshold = 2f;
    
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_car == null) return;

        if (collision.GetComponent<CarController>() == _car.GetCarController)
        {
            if (Mathf.RoundToInt(_car.GetCarController.GetRb.linearVelocity.magnitude) <= _finishSpeedThreshold)
            {
                _uiManager?.ToggleFinishUI(true);
            }
        }
    }

}
