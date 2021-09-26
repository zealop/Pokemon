using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    FreeRoam, Battle, Dialog, Cutscene
}
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;
    private void Awake()
    {
        //ConditionDB.Init();
    }
    private void Start()
    {
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
        playerController.OnEnterTrainerView += StartTrainerBattle;


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

    void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PokemonParty>();
        var wildPokemon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomPokemon();

        battleSystem.StartBattle(playerParty, wildPokemon);
    }

    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    void StartTrainerBattle(Collider2D trainerCollider)
    {
        var trainer = trainerCollider.GetComponentInParent<TrainerController>();
        if (trainer is object)
        {
            state = GameState.Cutscene;
            StartCoroutine(trainer.TriggerTrainerBattle(playerController));
        }
    }
}
