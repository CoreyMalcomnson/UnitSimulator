using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; private set; }

    public event Action<Unit> OnUnitSelected;
    public event Action<Unit> OnUnitDeselected;

    public event Action OnSphereSelectionStarted;
    public event Action OnSphereSelectionCompleted;
    public event Action<Vector3, float> OnSphereSelectionUpdate;
    
    [SerializeField] private LayerMask unitLayerMask;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float multiSelectRadius = 5f;

    private List<Unit> selectedUnitsList;

    private bool sphereSelecting;
    private Vector3 sphereCenter;

    private void Awake()
    {
        Instance = this;
        selectedUnitsList = new List<Unit>();
    }

    private void Start()
    {
        UnitManager.Instance.OnUnitRemoved += UnitManager_OnUnitRemoved;
    }

    private void Update()
    {
        if (HandleUnitSingleToggleSelect()) return;
        HandleSphereSelection();
    }

    private bool HandleUnitSingleToggleSelect()
    {
        if (sphereSelecting) return false;

        if (!Input.GetMouseButtonDown(0)) return false;
        if (!Physics.Raycast(MouseWorld.GetRay(), out RaycastHit hitInfo, float.MaxValue, unitLayerMask)) return false;

        if (!hitInfo.collider.TryGetComponent(out Unit unit)) return false;

        if (IsUnitSelected(unit))
        {
            DeselectUnit(unit);
        }
        else
        {
            SelectUnit(unit);
        }

        return true;
    }

    /*private void HandleUnitMultiSelect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!Input.GetKey(KeyCode.LeftShift)) ClearSelectedUnits();

            if (!Physics.Raycast(MouseWorld.GetRay(), out RaycastHit groundHitInfo, float.MaxValue, groundLayerMask)) return;
            StartMultiSelect(groundHitInfo);
        }

        if (isMultiSelecting)
        {
            if (Physics.Raycast(MouseWorld.GetRay(), out RaycastHit hitInfo, float.MaxValue, groundLayerMask))
            { 
                UpdateMultiSelectArea(hitInfo);
            }

            if (Input.GetMouseButtonUp(0))
            {
                CompleteMultiSelect();
            }
        }

        void CompleteMultiSelect()
        {
            Collider[] unitColliders = Physics.OverlapBox(multiSelectCenter, multiSelectExtents / 2f, Quaternion.identity, unitLayerMask);
            foreach (Collider collider in unitColliders)
            {
                SelectUnit(collider.GetComponent<Unit>());
            }

            isMultiSelecting = false;
            OnMultiSelectingCompleted?.Invoke();
        }

        void UpdateMultiSelectArea(RaycastHit hitInfo)
        {
            multiSelectEnd = hitInfo.point;

            float xDif = multiSelectStart.x - multiSelectEnd.x;
            float yDif = multiSelectStart.y - multiSelectEnd.y;
            float zDif = multiSelectStart.z - multiSelectEnd.z;

            float xMid = multiSelectStart.x - xDif / 2f;
            float yMid = multiSelectStart.y - yDif / 2f;
            float zMid = multiSelectStart.z - zDif / 2f;

            multiSelectExtents = new Vector3(Mathf.Abs(xDif), Mathf.Abs(yDif) + multiSelectVerticalBuffer, Mathf.Abs(zDif));
            multiSelectCenter = new Vector3(xMid, yMid, zMid);

            //OnMultiSelectAreaUpdated?.Invoke(multiSelectCenter, multiSelectExtents);
        }

        void StartMultiSelect(RaycastHit groundHitInfo)
        {
            isMultiSelecting = true;
            multiSelectStart = groundHitInfo.point;
            OnMultiSelectingStarted?.Invoke();
        }

        // Put htings in private functions for organization
    }*/

    private void HandleSphereSelection()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!Input.GetKey(KeyCode.LeftShift)) ClearSelectedUnits();

            if (!Physics.Raycast(MouseWorld.GetRay(), out RaycastHit groundHitInfo, float.MaxValue, groundLayerMask)) return;

            StartSphereSelection();
        }

        if (sphereSelecting)
        {
            UpdateSphereSelection();

            if (Input.GetMouseButtonUp(0))
            {
                CompleteSphereSelection();
            }
        }

        void StartSphereSelection()
        {
            sphereSelecting = true;
            OnSphereSelectionStarted?.Invoke();
        }

        void UpdateSphereSelection()
        {
            if (Physics.Raycast(MouseWorld.GetRay(), out RaycastHit hitInfo, float.MaxValue, groundLayerMask))
            {
                sphereCenter = hitInfo.point;
                OnSphereSelectionUpdate?.Invoke(sphereCenter, multiSelectRadius);

                Collider[] unitColliders = Physics.OverlapSphere(sphereCenter, multiSelectRadius, unitLayerMask);
                foreach (Collider collider in unitColliders)
                {
                    SelectUnit(collider.GetComponent<Unit>());
                }
            }
        }

        void CompleteSphereSelection()
        {
            sphereSelecting = false;
            OnSphereSelectionCompleted?.Invoke();
        }
    }

    private void SelectUnit(Unit unit)
    {
        if (IsUnitSelected(unit)) return;

        selectedUnitsList.Add(unit);
        OnUnitSelected?.Invoke(unit);
    }

    private void DeselectUnit(Unit unit)
    {
        if (!IsUnitSelected(unit)) return;

        selectedUnitsList.Remove(unit);
        OnUnitDeselected?.Invoke(unit);
    }

    private void ClearSelectedUnits()
    {
        while (selectedUnitsList.Count > 0)
        {
            DeselectUnit(selectedUnitsList[0]);
        }
    }

    private void UnitManager_OnUnitRemoved(Unit unit)
    {
        DeselectUnit(unit);
    }

    public bool IsUnitSelected(Unit unit)
    {
        return selectedUnitsList.Contains(unit);
    }

    public bool IsAnyUnitSelected()
    {
        return selectedUnitsList.Count > 0;
    }

    public List<Unit> GetSelectedUnitsList()
    {
        return selectedUnitsList;
    }

    public bool IsMultiSelecting()
    {
        return sphereSelecting;
    }
}
