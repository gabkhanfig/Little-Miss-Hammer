using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector2 offset;
    [SerializeField]
    private float damping;
    [SerializeField]
    private float zOffset = -10;

    private Vector2 velocity;

    void FixedUpdate() {
        Vector2 movePosition = (Vector2)target.position + offset;
        Vector2 newPosition = Vector2.SmoothDamp(transform.position, movePosition, ref velocity, damping);
        transform.position = new Vector3(newPosition.x, newPosition.y, zOffset);
    }
}
