using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class HarvestableManager : MonoBehaviour
{
    private const int FIND_VALID_POINT_ATTEMPTS = 50;

    public static HarvestableManager Instance;

    [Header("Spawn Settings")]
    [SerializeField] private List<Harvestable> harvestablePrefabs;
    [SerializeField] private int spawnCount = 50;
    [SerializeField] private float stockpileClearRadius = 15f;
    [SerializeField] private float linkClearRadius = 5f;

    private Dictionary<HarvestableType, List<Harvestable>> harvestableDictionary;

    private void Awake()
    {
        Instance = this;
        InitializeHarvestableDictionary();
    }

    private void Start()
    {
        SpawnHarvestables();
    }

    private void InitializeHarvestableDictionary()
    {
        harvestableDictionary = new Dictionary<HarvestableType, List<Harvestable>>();

        // Create list for each type
        foreach (HarvestableType harvestableType in System.Enum.GetValues(typeof(HarvestableType)))
        {
            harvestableDictionary[harvestableType] = new List<Harvestable>();
        }
    }

    private void SpawnHarvestables()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Harvestable harvestablePrefab = harvestablePrefabs[Random.Range(0, harvestablePrefabs.Count)];

            if (!TryGetValidSpawnPoint(out RaycastHit hitInfo)) break;

            Harvestable harvestable = Instantiate(harvestablePrefab, transform);
            harvestable.transform.position = hitInfo.point - hitInfo.normal * .1f;
            harvestable.transform.up = hitInfo.normal;
        }
    }

    private bool TryGetValidSpawnPoint(out RaycastHit hitInfo)
    {
        Planet planet = Planet.Instance;
        Vector3 stockpilePosition = Stockpile.Instance.GetPosition();
        List<NavMeshLink> navMeshLinkList = planet.GetNavMeshLinkList();

        hitInfo = default;

        //Try to get a valid point FIND_VALID_POINT_ATTEMPTS times
        for (int i = 0; i < FIND_VALID_POINT_ATTEMPTS; i++)
        {
            hitInfo = planet.GetRandomPointOnPlanet();
            if (!IsValidHarvestablePoint(hitInfo, stockpilePosition, navMeshLinkList)) continue;

            return true;
        }
        
        return false;
    }

    private bool IsValidHarvestablePoint(RaycastHit hitInfo, Vector3 stockpilePosition, List<NavMeshLink> navMeshLinkList)
    {
        if (Vector3.Distance(hitInfo.point, stockpilePosition) < stockpileClearRadius) return false;

        // Check if it's near navMeshLink
        bool navMeshLinkInRange = false;
        foreach (NavMeshLink navMeshLink in navMeshLinkList)
        {
            navMeshLinkInRange = (Vector3.Distance(hitInfo.point, navMeshLink.transform.position) < linkClearRadius);
            if (navMeshLinkInRange) break;
        }

        if (navMeshLinkInRange) return false;

        return true;
    }

    public void AddHarvestable(Harvestable harvestable)
    {
        HarvestableType harvestableType = harvestable.GetHarvestableType();
        harvestableDictionary[harvestableType].Add(harvestable);
    }

    public Harvestable FindClosestHarvestable(Vector3 currentPosition, HarvestableType harvestableType) // This could be bad
    {
        Harvestable closestHarvestable = null;
        float smallestDistance = float.MaxValue;

        foreach (Harvestable harvestable in harvestableDictionary[harvestableType])
        {
            if (harvestable.IsDead()) continue;

            float distance = (currentPosition - harvestable.GetPosition()).magnitude;
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                closestHarvestable = harvestable;
            }
        }

        return closestHarvestable;
    }

    
}
