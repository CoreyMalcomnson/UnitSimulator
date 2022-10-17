using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool invert = true;
    [SerializeField] private bool flattenHeight = true;

    private void Update()
    {
        Vector3 directionToCamera = (Camera.main.transform.position-transform.position).normalized;
        if (flattenHeight) directionToCamera.y = 0;

        transform.forward = invert ? -directionToCamera : directionToCamera;
    }
}
