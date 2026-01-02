using UnityEngine;

public class CameraController : MonoBehaviour
{
    public arrowShooter arrowShooter;  // Назначь в Inspector!
    public float followSpeed = 5f;
    public float zoomSpeed = 2f;
    private float normalSize = 5f;
    private float maxZoomSize = 2.5f;

    private Transform arrowToFollow;
    private float initialDist;
    private bool following = false;

    void Update()
    {
        if (following && arrowToFollow != null)
        {
            Vector3 targetPos = new Vector3(arrowToFollow.position.x, arrowToFollow.position.y, -10f);
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);

            float distToTarget = Vector2.Distance(arrowToFollow.position, arrowShooter.targetPosition);
            float zoomLerp = Mathf.Clamp01(distToTarget / initialDist);
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(maxZoomSize, normalSize, zoomLerp);
        }
    }

    public void StartFollowing(Transform arrow)
    {
        arrowToFollow = arrow;
        following = true;
        initialDist = Vector2.Distance(arrow.position, arrowShooter.targetPosition);
    }

    public void ResetCamera()
    {
        following = false;
        arrowToFollow = null;
        GetComponent<Camera>().orthographicSize = normalSize;
        transform.position = new Vector3(0, 0, -10f);
    }
}