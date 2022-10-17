using System.Collections;
using UnityEngine;

public class BagProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float stoppingDistance = .5f;

    private Vector3 targetPosition;

    private void Update()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) <= stoppingDistance)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}