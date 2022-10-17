using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Worker : MonoBehaviour
{
    private List<BaseTask> allTasks;
    private bool isWorking;
    private BaseTask currentTask;

    private void Awake()
    {
        allTasks = new List<BaseTask>(GetComponents<BaseTask>());
    }

    public void StartTask<T>(TaskArgs e) where T : BaseTask
    {
        BaseTask newTask = GetTask<T>();
        if (newTask == null) return;

        currentTask?.CancelTask();
        currentTask = newTask;

        isWorking = true;
        currentTask.StartTask(e, OnTaskComplete);

        void OnTaskComplete()
        {
            isWorking = false;
            currentTask = null; // I could rework this to make it restart the task not sure how itd work with certain tasks
        }
    }

    public T GetTask<T>() where T : BaseTask
    {
        return allTasks.FirstOrDefault(task => task is T) as T;
    }

    public bool IsWorking()
    {
        return isWorking;
    }
}
