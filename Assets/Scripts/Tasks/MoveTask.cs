using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MoveTask : BaseTask
{
    private Unit unit;
    private Mover mover;
    private Vector3 targetPosition;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    private void Start()
    {
        mover = unit.GetMover();
    }

    private void Update()
    {
        if (!isActive) return;

        if (!mover.IsNavigating()) TaskComplete();
    }

    public override void StartTask(TaskArgs e, Action onTaskComplete)
    {
        base.StartTask(e, onTaskComplete);
        targetPosition = (e as MoveTaskArgs).position;
        unit.GetMover().MoveTo(targetPosition);
    }

    public override void TaskComplete()
    {
        mover.Stop();
        base.TaskComplete();
    }
}
