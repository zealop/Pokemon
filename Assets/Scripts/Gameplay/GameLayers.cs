using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask solidObjectsLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask grassLayer;

    public LayerMask SolidLayer { get => solidObjectsLayer; }
    public LayerMask InteractableLayer { get => interactableLayer; }
    public LayerMask GrassLayer { get => grassLayer; }

    public static GameLayers i { get; private set; }

    private void Awake()
    {
        i = this;
    }
}
