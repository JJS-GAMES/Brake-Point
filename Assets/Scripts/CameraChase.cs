using UnityEngine;

public class CameraChase2D : MonoBehaviour
{
    [SerializeField] private float _smooth = 5f;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;

    private void FixedUpdate()
    {
        Chasing();
    }

    private void Chasing()
    {
        if (_target == null) return;

        Vector3 offset = new Vector3(_offset.x, _offset.y, transform.position.z);
        Vector3 targetPosition = _target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _smooth * Time.fixedDeltaTime);
    }
}
