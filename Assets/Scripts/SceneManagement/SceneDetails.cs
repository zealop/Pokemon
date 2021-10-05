using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour
{
    [SerializeField] List<SceneDetails> connectedScenes;

    public List<SceneDetails> ConnectedScenes => connectedScenes;
    public bool IsLoaded { get; private set; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log($"Enter {gameObject.name}");

            LoadScene();

            GameController.Instance.SetCurrentScene(this);

            LoadConenctedScenes();
            UnloadUnconnectedScenes();
        }
    }

    public void LoadScene()
    {
        if (!IsLoaded)
        {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            IsLoaded = true;
        }
    }

    public void UnloadScene()
    {
        if(IsLoaded)
        {
            SceneManager.UnloadSceneAsync(gameObject.name);
            IsLoaded = false;
        }
    }

    private void LoadConenctedScenes()
    {
        //load connectedd
        foreach (var scene in connectedScenes)
        {
            scene.LoadScene();
        }
    }

    public void UnloadUnconnectedScenes()
    {
        var previousScene = GameController.Instance.PreviousScene;
        if (previousScene is object)
        {
            foreach (var scene in previousScene.connectedScenes)
            {
                if(!connectedScenes.Contains(scene) && scene != this)
                {
                    scene.UnloadScene();
                }
            }
        }
    }
}
