using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTask : MonoBehaviour
{
    protected TaskArgs taskArgs;
    protected Action onTaskComplete;
    protected bool isActive;

    public virtual void StartTask(TaskArgs taskArgs, Action onTaskComplete)
    {
        this.taskArgs = taskArgs;
        this.onTaskComplete = onTaskComplete;
        isActive = true;
    }

    public virtual void TaskComplete()
    {
        isActive = false;
        onTaskComplete();
    }

    public virtual void CancelTask()
    {
        TaskComplete();
    }
}
