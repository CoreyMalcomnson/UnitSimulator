using System.Collections;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    [SerializeField] private LayerMask validLayers;

    private static MouseWorld instance;

    private void Awake()
    {
        instance = this;
    }

    public static Ray GetRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}