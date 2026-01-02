using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxFactor = 0.5f; // 0 = no move, 1 = full camera speed. Use <1 for depth
    private Transform cameraTransform;
    private Vector3 lastCameraPos;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPos = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - lastCameraPos;
        transform.position += delta * parallaxFactor;
        lastCameraPos = cameraTransform.position;
    }
}