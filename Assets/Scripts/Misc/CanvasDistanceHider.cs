using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDistanceHider : MonoBehaviour
{
    [SerializeField] private float hideDistance = 5f;
    [SerializeField] private bool ignoreHeight = true;

    private Camera mainCamera;
    private Canvas canvas;

    private void Awake()
    {
        mainCamera = Camera.main;
        canvas = GetComponent<Canvas>();    
    }

    private void Update()
    {
        Vector3 target = mainCamera.transform.position;
        if (ignoreHeight) target.y = transform.position.y;

        canvas.enabled = Vector3.Distance(transform.position, target) < hideDistance;
    }
}
