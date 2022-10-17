using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public static Spaceship Instance;

    public event Action OnStateChanged;

    [SerializeField] private float dockedTime = 10;
    [SerializeField] private float preparingTime = 5;
    [SerializeField] private float offplanetTime = 5;

    public enum State
    {
        Docked,
        PreparingToTakeOff,
        OffPlanet
    }

    private ResourceInventory resourceInventory;
    private State state;
    private float timer;

    private void Awake()
    {
        Instance = this;

        resourceInventory = GetComponent<ResourceInventory>();
        SwitchState(State.Docked);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        switch (state)
        {

            case State.Docked:
                if (timer <= 0)
                {
                    SwitchState(State.PreparingToTakeOff);
                }
                break;
            case State.PreparingToTakeOff:
                if (timer <= 0)
                {
                    SwitchState(State.OffPlanet);
                }
                break;
            case State.OffPlanet:
                if (timer <= 0)
                {
                    SwitchState(State.Docked);
                }
                break;
        }
    }

    private void SwitchState(State newState)
    {
        // Exit
        switch (state)
        {
            case State.Docked:
                break;
            case State.PreparingToTakeOff:
                break;
            case State.OffPlanet:
                int resourceValue = resourceInventory.GetTotalResourceAmount();
                CurrencyManager.Instance.AddCurrencyAmount(CurrencyType.Credit, resourceValue);
                resourceInventory.ClearReasourceInventory();
                break;
        }

        state = newState;

        //Enter
        switch (state)
        {
            case State.Docked:
                timer = dockedTime;
                break;
            case State.PreparingToTakeOff:
                timer = preparingTime;
                break;
            case State.OffPlanet:
                timer = offplanetTime;
                break;
        }

        OnStateChanged?.Invoke();
    }

    public ResourceInventory GetResourceInventory()
    {
        return resourceInventory;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public State GetState()
    {
        return state;
    }
}
