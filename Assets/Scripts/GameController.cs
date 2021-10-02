using UnityEngine;


public enum GameState
{
    FreeRoam, Battle, Dialog, Cutscene, Pause
}
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState beforePauseState;
    GameState state;

    TrainerController trainer;
    public static GameController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
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
    }
    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
    }
    public void PauseGame(bool pause)
    {
        if(pause)
        {
            beforePauseState = state;
            state = GameState.Pause;
        }
        else
        {
            state = beforePauseState;
        }
    }
    public void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PokemonParty>();
        var wildPokemon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomPokemon();

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

    void EndBattle(bool won)
    {
        //if trainer battle
        if (trainer is object && won)
        {
            trainer.BatteLost();
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
}
