using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneDetails : SerializedMonoBehaviour
{
    [SerializeField] private HashSet<string> connectedScenes = new HashSet<string>();

    public HashSet<string> ConnectedScenes => connectedScenes;

    public string Name => gameObject.scene.name;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log($"Enter {Name}");

            GameController.I.ActiveScenes.Add(Name);

            LoadConenctedScenes();
            UnloadUnconnectedScenes();
        }
    }

    public void LoadConenctedScenes()
    {
        //load connectedd
        foreach (var scene in connectedScenes)
        {
            //if(!SceneManager.GetSceneByName(scene).isLoaded)
            //{
            //    SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            //    GameController.Instance.ActiveScenes.Add(scene);
            //}
            StartCoroutine(GameController.I.LoadScene(scene));
        }
    }

    public void UnloadUnconnectedScenes()
    {
        foreach (var scene in GameController.I.ActiveScenes.ToList())
        {
            if (scene != Name && !connectedScenes.Contains(scene))
            {
                //SceneManager.UnloadSceneAsync(scene);
                //GameController.Instance.ActiveScenes.Remove(scene);
                GameController.I.UnloadScene(scene);
            }
        }
    }
}
