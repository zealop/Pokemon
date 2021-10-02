using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask solidObjectsLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask grassLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask fovLayer;
    [SerializeField] LayerMask portalLayer;

    public LayerMask SolidLayer { get => solidObjectsLayer; }
    public LayerMask InteractableLayer { get => interactableLayer; }
    public LayerMask GrassLayer { get => grassLayer; }
    public LayerMask PlayerLayer { get => playerLayer; }
    public LayerMask FOVLayer { get => fovLayer; }
    public LayerMask PortalLayer { get => portalLayer; }
    public LayerMask TriggerableLayers { get => grassLayer | fovLayer | portalLayer; }
    public static GameLayers Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
