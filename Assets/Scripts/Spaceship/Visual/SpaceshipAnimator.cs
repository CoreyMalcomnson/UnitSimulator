using System;
using System.Collections;
using UnityEngine;

public class SpaceshipAnimator : MonoBehaviour
{
    [SerializeField] private GameObject visual;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Spaceship.Instance.OnStateChanged += Spaceship_OnStateChanged;
    }

    private void OnDestroy()
    {
        Spaceship.Instance.OnStateChanged -= Spaceship_OnStateChanged;
    }

    private void Spaceship_OnStateChanged()
    {
        switch(Spaceship.Instance.GetState())
        {
            case Spaceship.State.Docked:
                animator.SetTrigger("Docking");
                break;
            case Spaceship.State.OffPlanet:
                animator.SetTrigger("TakeOff");
                break;
        }
    }

    public void ShowShip()
    {
        visual.SetActive(true);
    }

    public void HideShip()
    {
        visual.SetActive(false);
    }
}