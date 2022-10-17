using System.Collections;
using UnityEngine;

public class HarvestableEffects : MonoBehaviour
{
    [SerializeField] protected GameObject visual;

    [SerializeField] private ParticleSystem harvestVfx;
    [SerializeField] private AudioClip harvestSfxClip;
    [SerializeField] private AudioClip disableSfxClip;

    private Harvestable harvestable;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        harvestable = GetComponent<Harvestable>();
    }

    private void Start()
    {
        harvestable.OnHarvest += Harvestable_OnHarvest;
        harvestable.OnHarvestableEnabled += Harvestable_OnHarvestableEnabled;
        harvestable.OnHarvestableDisabled += Harvestable_OnHarvestableDisabled;
    }

    private void Harvestable_OnHarvestableDisabled()
    {
        visual.SetActive(false);
        audioSource.PlayOneShot(disableSfxClip);
    }

    private void Harvestable_OnHarvestableEnabled()
    {
        visual.SetActive(true);
    }

    private void Harvestable_OnHarvest()
    {
        harvestVfx.Play();
        audioSource.PlayOneShot(harvestSfxClip);
    }
}