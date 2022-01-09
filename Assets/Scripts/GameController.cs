using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    FreeRoam, Battle, Dialog, Cutscene, Pause, Menu
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
    public static GameController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        menuController = GetComponent<MenuController>();
    }
    private void Start()
    {
        battleSystem.OnBattleOver += EndBattle;

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
        if (state == GameState.FreeRoam)
        {
            StartWildBattle(new Pokemon(testPokemon.Base, testPokemon.Level));

            playerController.HandleUpdate();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                state = GameState.Menu;
                menuController.OpenMenu();
            }



        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Menu)
        {
            menuController.HandleUpdate();
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

        var playerParty = playerController.GetComponent<PokemonParty>();
        battleSystem.StartBattle(playerParty, wildPokemon);
    }
    public void StartTrainerBattle(TrainerController trainerController)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        this.trainer = trainerController;

        var playerParty = playerController.GetComponent<PokemonParty>();
        var trainerParty = trainerController.GetComponent<PokemonParty>();

        battleSystem.StartTrainerBattle(playerParty, trainerParty);
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
            var savables = scene.GetRootGameObjects().Select(o => o.GetComponent<SavableEntity>()).OfType<SavableEntity>().ToList();

            SavingSystem.i.RestoreEntityStates(savables);
        }
    }
    public void UnloadScene(string sceneName)
    {
        if (ActiveScenes.Contains(sceneName))
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            var savables = scene.GetRootGameObjects().Select(o => o.GetComponent<SavableEntity>()).OfType<SavableEntity>().ToList();

            SavingSystem.i.CaptureEntityStates(savables);

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
            default:
                break;
        }
        state = GameState.FreeRoam;
    }
}


