using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Parallax : MonoBehaviour
{
    [SerializeField] private float _parallaxFactor = 0.5f;

    private Transform _cameraTransform;
    private float _startCameraX;

    void Start()
    {
        _cameraTransform = Camera.main.transform;
        _startCameraX = _cameraTransform.position.x;
    }

    void LateUpdate()
    {
        float deltaX = _cameraTransform.position.x - _startCameraX;
    }

}
