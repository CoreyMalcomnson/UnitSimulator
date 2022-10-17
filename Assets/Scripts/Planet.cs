using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public static Planet Instance;

    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float radius = 25f;

    private List<NavMeshLink> navMeshLinkList;

    private void Awake()
    {
        Instance = this;

        navMeshLinkList = new List<NavMeshLink>(FindObjectsOfType<NavMeshLink>());
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public float GetRadius()
    {
        return radius;
    }

    public RaycastHit GetRandomPointOnPlanet()
    {
        Vector3 randomDirection = Random.onUnitSphere;
        Vector3 randomPoint = Planet.Instance.GetPosition() + (randomDirection * Planet.Instance.GetRadius()) + randomDirection;
        Physics.Raycast(randomPoint, -randomDirection, out RaycastHit hitInfo, 2f, groundLayerMask);

        return hitInfo;
    }

    public List<NavMeshLink> GetNavMeshLinkList()
    {
        return navMeshLinkList;
    }
}
