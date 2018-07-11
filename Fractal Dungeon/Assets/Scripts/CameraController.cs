using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera camera;

    private Vector3 lastPosition   = Vector3.zero;
    private Vector3 targetPosition = Vector3.zero;

    private int currentIteration;

    private float    lastOrthographicSize;
    private float currentOrthographicSize;
    private float lerpProgress = 1f;

    public float defaultOrthographicSize;
    public float cameraSpeed = 10f;

    private void Awake()
    {
        camera  = GetComponent<Camera>();
        camera.orthographic = true;
        lastPosition        = transform.position;
    }

    private void Start()
    {
        lastPosition         = transform.position;
        lastOrthographicSize = camera.orthographicSize;
        lerpProgress = 1f;
    }

    private void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        if(transform.position != targetPosition)
        {
            lerpProgress += Time.deltaTime * cameraSpeed;

            if (lerpProgress > 1f)
                lerpProgress = 1f;

            Vector3 lerpVector      = Vector3.Lerp(lastPosition, targetPosition, lerpProgress);
            transform.position      = new Vector3(lerpVector.x, lerpVector.y, -10f);
            camera.orthographicSize = Mathf.Lerp(lastOrthographicSize, currentOrthographicSize, lerpProgress);
        }
    }

    public void UpdateTracking(Transform newTarget, int newIteration)
    {
        lerpProgress = 0f;
        lastPosition   = transform.position;
        targetPosition = newTarget.position;

        currentIteration = newIteration;

        lastOrthographicSize    = camera.orthographicSize;
        currentOrthographicSize = defaultOrthographicSize * Mathf.Pow(0.5f, currentIteration);
    }
}