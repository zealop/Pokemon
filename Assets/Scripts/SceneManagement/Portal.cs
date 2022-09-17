using System.Collections;
using System.Linq;
using UnityEngine;

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private DestinationIdentifier destinationPortal;
    [SerializeField] private Transform spawnPoint;

    public Transform SpawnPoint => spawnPoint;
    public DestinationIdentifier Destinationportal => destinationPortal;
    private PlayerController player;
    private Fader fader;
    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player;
        StartCoroutine(SwitchScene());
    }

    private void Start()
    {
        fader = FindObjectOfType<Fader>();

    }

    private IEnumerator SwitchScene()
    {
        DontDestroyOnLoad(gameObject);
        GameController.I.PauseGame(true);
        yield return fader.FadeIn(0.5f);



        yield return GameController.I.LoadScene(sceneToLoad);
        //yield return SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);



        var destPortal = FindObjectsOfType<Portal>().First(x => x != this && x.destinationPortal == this.destinationPortal);

        player.character.SetPositionAndSnapToTile(destPortal.SpawnPoint.position);





        yield return fader.FadeOut(0.5f);
        GameController.I.PauseGame(false);
        Destroy(gameObject);
    }
}

public enum DestinationIdentifier
{
    A, B, C, D, E, F
}