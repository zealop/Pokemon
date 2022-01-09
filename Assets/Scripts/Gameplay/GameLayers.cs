using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] private LayerMask solidObjectsLayer;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private LayerMask grassLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask fovLayer;
    [SerializeField] private LayerMask portalLayer;

    public LayerMask SolidLayer => solidObjectsLayer;
    public LayerMask InteractableLayer => interactableLayer;
    public LayerMask GrassLayer => grassLayer;
    public LayerMask PlayerLayer => playerLayer;
    public LayerMask FOVLayer => fovLayer;
    public LayerMask PortalLayer => portalLayer;
    public LayerMask TriggerableLayers => grassLayer | fovLayer | portalLayer;
    public static GameLayers Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
