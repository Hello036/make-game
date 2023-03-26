using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform objectToFollow;
    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - objectToFollow.position;
    }

    private void LateUpdate()
    {
        transform.position = objectToFollow.position + offset;
    }
}
