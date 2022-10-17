using System.Collections;
using UnityEngine;

public class UnitAnimationEvents : MonoBehaviour
{
    [SerializeField] private BagProjectile bagProjectilePrefab;
    [SerializeField] private Transform bagProjectileSpawn;

    public void SpawnBagProjectile()
    {
        BagProjectile bagProjectile = Instantiate(bagProjectilePrefab, bagProjectileSpawn.position, bagProjectileSpawn.rotation);
        bagProjectile.Setup(Spaceship.Instance.GetPosition());
    }
}