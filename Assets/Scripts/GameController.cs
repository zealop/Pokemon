using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle;
using Pokemons;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    FreeRoam,
    Battle,
    Dialog,
    Cutscene,
    Pause,
    Menu
}

public class GameController : SerializedMonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BattleManager battleSystem;
    [SerializeField] private Camera worldCamera;
    [SerializeField] private HashSet<string> activeScenes = new HashSet<string>();

    [SerializeField] private Pokemon testPokemon;

    public HashSet<string> ActiveScenes => activeScenes;

    private GameState beforePauseState;
    private GameState state;

    private TrainerController trainer;

    private MenuController menuController;
    public static GameController I { get; private set; }

    private void Awake()
    {
        I = this;
        menuController = GetComponent<MenuController>();
    }

    private void Start()
    {
        battleSystem.OnBattleEnd += EndBattle;

        DialogManager.Instance.OnShowDialog += () => state = GameState.Dialog;
        DialogManager.Instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
                state = GameState.FreeRoam;
        };

        menuController.onBack += () => state = GameState.FreeRoam;
        menuController.onMenuSelected += OnMenuSelected;
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.FreeRoam:
            {
                StartWildBattle(new Pokemon(testPokemon.Base, testPokemon.Level));

                playerController.HandleUpdate();
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    state = GameState.Menu;
                    menuController.OpenMenu();
                }

                break;
            }
            case GameState.Battle:
                battleSystem.HandleUpdate();
                break;
            case GameState.Dialog:
                DialogManager.Instance.HandleUpdate();
                break;
            case GameState.Menu:
                menuController.HandleUpdate();
                break;
            case GameState.Cutscene:
                break;
            case GameState.Pause:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            beforePauseState = state;
            state = GameState.Pause;
        }
        else
        {
            state = beforePauseState;
        }
    }

    public void StartWildBattle(Pokemon wildPokemon)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        battleSystem.StartWildBattle(wildPokemon);
    }

    public void StartTrainerBattle(TrainerController trainerController)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        trainer = trainerController;
        
        battleSystem.StartTrainerBattle(trainerController);
    }

    private void EndBattle(bool won)
    {
        //if trainer battle
        if (trainer is object && won)
        {
            trainer.BattleLost();
            trainer = null;
        }

        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    public void OnEnterTrainerFOV(TrainerController trainer)
    {
        state = GameState.Cutscene;
        StartCoroutine(trainer.TriggerTrainerBattle(playerController));
    }

    public IEnumerator LoadScene(string sceneName)
    {
        if (!ActiveScenes.Contains(sceneName))
        {
            ActiveScenes.Add(sceneName);
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            var scene = SceneManager.GetSceneByName(sceneName);
            var savable = scene.GetRootGameObjects().Select(o => o.GetComponent<SavableEntity>())
                .Where(o => o is object).ToList();

            SavingSystem.i.RestoreEntityStates(savable);
        }
    }

    public void UnloadScene(string sceneName)
    {
        if (ActiveScenes.Contains(sceneName))
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            var savable = scene.GetRootGameObjects().Select(o => o.GetComponent<SavableEntity>())
                .Where(o => o is object).ToList();

            SavingSystem.i.CaptureEntityStates(savable);

            ActiveScenes.Remove(sceneName);
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    private void OnMenuSelected(int selected)
    {
        switch (selected)
        {
            case 0: //Party
                break;
            case 1: //Bag
                break;
            case 2: //Save
                SavingSystem.i.Save("saveSlot1");
                break;
            case 3: //Load
                SavingSystem.i.Load("saveSlot1");
                break;
        }

        state = GameState.FreeRoam;
    }
}