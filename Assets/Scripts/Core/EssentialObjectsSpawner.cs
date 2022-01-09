using UnityEngine;

public class EssentialObjectsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject essentialObjectsPrefab;

    private void Awake()
    {
        var existingObjects = FindObjectsOfType<EssentialObjects>();
        if (existingObjects.Length == 0)
        {
            //spawn at center of grid
            var spawnPos = new Vector3(0, 0, 0);
            var grid = FindObjectOfType<Grid>();
            if (grid is object)
            {
                spawnPos = grid.transform.position;
            }


            Instantiate(essentialObjectsPrefab, spawnPos, Quaternion.identity);
        }
    }
}