using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] int sceneToloadIndex;
    [SerializeField] DestinationIdentifier destinationPortal;
    [SerializeField] Transform spawnPoint;

    public Transform SpawnPoint => spawnPoint;
    public DestinationIdentifier Destinationportal => destinationPortal;
    PlayerController player;
    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player;
        StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene()
    {
        DontDestroyOnLoad(gameObject);
        GameController.Instance.PauseGame(true);

        yield return SceneManager.LoadSceneAsync(sceneToloadIndex);

        var destPortal = FindObjectsOfType<Portal>().First(x => x != this && x.destinationPortal == this.destinationPortal);

        player.Character.SetPositionAndSnapToTile(destPortal.SpawnPoint.position);


        GameController.Instance.PauseGame(false);
        Destroy(gameObject);
    }
}

public enum DestinationIdentifier
{
    A, B, C, D, E, F
}