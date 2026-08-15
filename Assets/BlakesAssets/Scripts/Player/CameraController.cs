using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform FollowTarget, LookTarget;
    public float FollowSpeed = 10f;
    private void LateUpdate()
    {
        Vector3 targetPos = FollowTarget.position;
        transform.position = Vector3.Lerp(transform.position, targetPos, FollowSpeed * Time.deltaTime);
        transform.LookAt(LookTarget);
    }
}
