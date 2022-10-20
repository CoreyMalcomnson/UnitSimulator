using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleRandomizer : MonoBehaviour
{
    [SerializeField] private Vector2 minMaxRandom;

    private void Start()
    {
        transform.localScale = Vector3.one * Random.Range(minMaxRandom.x, minMaxRandom.y);
    }
}
