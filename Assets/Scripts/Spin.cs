using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed = 1f;

    private void Update()
    {
        transform.eulerAngles = transform.eulerAngles + direction * Time.deltaTime * speed;
    }
}
