using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public Transform target;
    public Collider areaLimit;

    public float distance = 10.0f;
    public float height = 5.0f;
    public float damping = 2.0f;
    public float zoomSpeed = 2.0f;
    public float minDistance = 5.0f;
    public float maxDistance = 15.0f;

    private float currentDistance;
    private Vector3 offset;

    // ī�޶� ���� ���� ����
    public Vector3 TargetDirection
    {
        get { return target.forward; }
    }

    void Start()
    {
        offset = new Vector3(0f, height, -distance);
        currentDistance = distance;
    }

    void LateUpdate()
    {
        // ���콺 �ٷ� ī�޶� ����/�ܾƿ�
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentDistance -= scroll * zoomSpeed;

        // ī�޶� �Ÿ� ����
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        offset = new Vector3(0f, height, -currentDistance);

        Vector3 desiredPosition = target.position + TargetDirection * offset.z + target.up * offset.y;
        Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);

        // ī�޶� �̵� ���� ����
        if (areaLimit != null)
        {
            Vector3 clampedPosition = areaLimit.ClosestPoint(position);
            position = clampedPosition;
        }

        transform.position = position;
        transform.LookAt(target);
    }
}
