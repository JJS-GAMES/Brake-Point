using UnityEngine;
using UnityEngine.EventSystems;

public class AccelerationPedal : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private CarController _carController;

    public void Init(CarController carController)
    {
        if (carController != null) _carController = carController;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _carController?.ToggleWorkingEngine(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _carController?.ToggleWorkingEngine(false);
    }
}