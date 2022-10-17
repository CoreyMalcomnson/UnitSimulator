using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystemVisual : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Image barImage;

    private void Start()
    {
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        UpdateHealthBar();
    }

    private void OnDestroy()
    {
        healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        barImage.fillAmount = healthSystem.GetHealthNormalized();
    }
}
