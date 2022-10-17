using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UnitTaskManager : MonoBehaviour
{
    [SerializeField] private LayerMask taskableLayerMask;
    [SerializeField] private LayerMask groundLayerMask;

    private void Update()
    {
        if (HandleUnitTaskables()) return;
        if (HandleUnitMoveTask()) return;
    }

    private bool HandleUnitTaskables()
    {
        if (!Input.GetMouseButtonDown(1)) return false;
        if (!UnitSelectionManager.Instance.IsAnyUnitSelected()) return false;
        if (!Physics.Raycast(MouseWorld.GetRay(), out RaycastHit hitInfo, float.MaxValue, taskableLayerMask)) return false;
        if (!hitInfo.collider.TryGetComponent(out ITaskable taskable)) return false;

        TaskType taskType = taskable.GetTaskType();

        switch (taskType)
        {
            case TaskType.Harvest:
                PerformTaskOnSelectedUnits<HarvestTask>(new HarvestTaskArgs
                {
                    harvestable = hitInfo.collider.GetComponent<Harvestable>()
                });
                break;
            case TaskType.Retrieve:
                PerformTaskOnSelectedUnits<RetrieveTask>(new RetrieveTaskArgs
                {
                    retrieveable = hitInfo.collider.GetComponent<Retrievable>()
                });
                break;
            case TaskType.Sell:
                PerformTaskOnSelectedUnits<SellTask>(TaskArgs.Empty);
                break;
        }

        return true;
    }

    private bool HandleUnitMoveTask()
    {
        if (!Input.GetMouseButtonDown(1)) return false;
        if (!UnitSelectionManager.Instance.IsAnyUnitSelected()) return false;
        if (!Physics.Raycast(MouseWorld.GetRay(), out RaycastHit hitInfo, float.MaxValue, groundLayerMask)) return false;

        PerformTaskOnSelectedUnits<MoveTask>(new MoveTaskArgs
        {
            position = hitInfo.point
        });

        return true;
    }

    private async void PerformTaskOnSelectedUnits<T>(TaskArgs e) where T : BaseTask
    {
        foreach (Unit unit in UnitSelectionManager.Instance.GetSelectedUnitsList())
        {
            unit.GetWorker().StartTask<T>(e);
            await Task.Yield();
        }
    }
}
