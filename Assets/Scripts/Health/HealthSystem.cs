using System;
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnDeath;

    [SerializeField] private int health = 1;

    private int maxHealth;
    private bool isDead;

    private void Awake()
    {
        maxHealth = health;
    }

    public void Damage(int amount)
    {
        if (isDead) return;

        health = Mathf.Clamp(health - amount, 0, maxHealth);

        if (health == 0)
        {
            isDead = true;
            OnDeath?.Invoke(this, EventArgs.Empty);
        }

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(int amount)
    {
        health += amount;

        health = Mathf.Clamp(health + amount, 0, maxHealth);

        if (health > 0)
        {
            isDead = false;
        }

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void HealMax()
    {
        Heal(maxHealth);
    }

    public bool IsDead()
    {
        return isDead;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthNormalized()
    {
        return (float)health / maxHealth;
    }
}
