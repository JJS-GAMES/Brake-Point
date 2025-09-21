using UnityEngine;
using UnityEngine.EventSystems;

public class BrakePedal : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private CarController _carController;

    public void Init(CarController carController)
    {
        if (carController != null) _carController = carController;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _carController?.SetBrakeFromUI(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _carController?.SetBrakeFromUI(false);
    }
}
