using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] string sceneToLoad;
    [SerializeField] DestinationIdentifier destinationPortal;
    [SerializeField] Transform spawnPoint;
    [SerializeField] bool goOutdoor;
    public Transform SpawnPoint => spawnPoint;
    public DestinationIdentifier Destinationportal => destinationPortal;
    PlayerController player;
    Fader fader;
    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player;
        StartCoroutine(SwitchScene());
    }

    private void Start()
    {
        fader = FindObjectOfType<Fader>();

    }

    IEnumerator SwitchScene()
    {
        DontDestroyOnLoad(gameObject);
        GameController.Instance.PauseGame(true);
        yield return fader.FadeIn(0.5f);

       
        

        yield return SceneManager.LoadSceneAsync(sceneToLoad);
        if(goOutdoor)
        {
            SceneManager.LoadSceneAsync("GamePlay");
        }
        else
        {
            GameController.Instance.SetCurrentScene(null);
        }


        var destPortal = FindObjectsOfType<Portal>().First(x => x != this && x.destinationPortal == this.destinationPortal);

        player.Character.SetPositionAndSnapToTile(destPortal.SpawnPoint.position);





        yield return fader.FadeOut(0.5f);
        GameController.Instance.PauseGame(false);
        Destroy(gameObject);
    }
}

public enum DestinationIdentifier
{
    A, B, C, D, E, F
}