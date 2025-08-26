using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _parallaxFactor = 1f;
    private Transform _cameraTransform;
    private Vector3 _previousCameraPosition;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _previousCameraPosition = _cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 delta = _cameraTransform.position - _previousCameraPosition;
        transform.position += new Vector3(delta.x * _parallaxFactor, 0, 0);
        _previousCameraPosition = _cameraTransform.position;
    }
}
