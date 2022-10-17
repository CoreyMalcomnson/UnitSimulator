using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSphereSelectVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    private void Start()
    {
        UnitSelectionManager.Instance.OnSphereSelectionStarted += UnitSelectionManager_OnSphereSelectionStarted;
        UnitSelectionManager.Instance.OnSphereSelectionCompleted += UnitSelectionManager_OnSphereSelectionCompleted;
        UnitSelectionManager.Instance.OnSphereSelectionUpdate += UnitSelectionManager_OnSphereSelectionUpdated;
    }

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.OnSphereSelectionStarted -= UnitSelectionManager_OnSphereSelectionStarted;
        UnitSelectionManager.Instance.OnSphereSelectionCompleted -= UnitSelectionManager_OnSphereSelectionCompleted;
        UnitSelectionManager.Instance.OnSphereSelectionUpdate -= UnitSelectionManager_OnSphereSelectionUpdated;
    }

    private void UnitSelectionManager_OnSphereSelectionStarted()
    {
        meshRenderer.enabled = true;
    }

    private void UnitSelectionManager_OnSphereSelectionCompleted()
    {
        meshRenderer.enabled = false;
    }

    private void UnitSelectionManager_OnSphereSelectionUpdated(Vector3 center, float radius)
    {
        transform.position = center;
        transform.localScale = Vector3.one * radius * 2f; // Sphere size is double units
        transform.up = (Camera.main.transform.position-transform.position).normalized;
    }
}
