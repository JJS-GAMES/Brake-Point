using UnityEngine;

public class CameraChase2D : MonoBehaviour
{
    public void Chasing(Transform target, Vector3 offset, float smooth)
    {
        if (target == null) return;

        Vector3 getOffset = new Vector3(offset.x, offset.y, transform.position.z);
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smooth * Time.fixedDeltaTime);
    }
}
