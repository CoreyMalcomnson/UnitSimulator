using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToGround : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float alignmentSmoothing = 2f;
    [SerializeField] private float castDistance = 2f;

    private Quaternion targetRotation;

    private void LateUpdate()
    {
        TryUpdateTargetRotation();
        UpdateRotation();
    }

    private bool TryGetHitInfo(out RaycastHit hitInfo)
    {
        return Physics.Raycast(transform.position + transform.up, -transform.up, out hitInfo, castDistance, groundLayerMask);
    }

    private void TryUpdateTargetRotation()
    {
        if (TryGetHitInfo(out RaycastHit hitInfo))
        {
            targetRotation = Quaternion.FromToRotation(transform.up, hitInfo.normal) * transform.parent.rotation; // Multiply by parent to maintian forward faceing
        }
    }

    private void UpdateRotation()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * alignmentSmoothing);
    }
}
