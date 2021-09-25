using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState
{
    Start, ActionSelection, MoveSelection, PerformMove, Busy, PartyScreen, BattleOver
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;

    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PartyScreen partyScreen;

    public event System.Action<bool> OnBattleOver;

    public BattleDialogBox DialogBox { get => dialogBox; }
    public Queue<string> MessageQueue { get; } = new Queue<string>();
    public MoveQueue moveQueue = new MoveQueue();


    BattleState state;
    int currentAction;
    int currentMove;
    int currentMember;


    PokemonParty playerParty;
    Pokemon wildPokemon;

    public PokemonParty PlayerParty { get => playerParty; }
    public void StartBattle(PokemonParty party, Pokemon wild)
    {
        playerParty = party;
        wildPokemon = wild;

        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup(playerParty.GetHealthyPokemon(), this);


        enemyUnit.Setup(wildPokemon, this);

        partyScreen.Init();

        dialogBox.SetMoveName(playerUnit.Moves);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared.");

        ActionSelection();
    }

    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        dialogBox.SetDialog("Choose an action");

        dialogBox.EnableActionSelector(true);
    }

    void MoveSelection()
    {
        state = BattleState.MoveSelection;

        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(true);
        dialogBox.EnableMoveSelector(true);
    }

    public void BattleOver(bool won)
    {
        state = BattleState.BattleOver;

        //playerParty.Party.ForEach(p => p.OnBattleOver());

        OnBattleOver(won);
    }
    public void HandleUpdate()
    {
        if(state == BattleState.ActionSelection)
        {
            HandleActionSelection();

        }
        else if(state == BattleState.MoveSelection)
        {
            HandleMoveSelection();  
        }
        else if (state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
    }


    public void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;

        partyScreen.SetPartyData(playerParty.Party);

        partyScreen.gameObject.SetActive(true);
    }

    void HandleActionSelection()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
            currentAction++;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            currentAction--;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            currentAction += 2;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            currentAction -= 2;

        currentAction = Mathf.Clamp(currentAction, 0, 3);

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            switch(currentAction)
            { 
                case 0://Fight
                    MoveSelection();
                    break;
                case 1: //Bag
                    break;

                case 2: //Party
                    OpenPartyScreen();
                    break;
                case 3: //Run
                    break;

            }
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            currentMove++;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            currentMove--;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMove += 2;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMove -= 2;

        currentMove = Mathf.Clamp(currentMove, 0, playerUnit.Moves.Count-1);

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);

            var move = playerUnit.Moves[currentMove];

            if(!move.Disabled)
            {
                moveQueue.Enqueue(move, playerUnit, enemyUnit);

                StartCoroutine(RunTurn());
            }

                
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableMoveSelector(false);

            dialogBox.EnableDialogText(true);
            ActionSelection();
        }
    }

    void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            currentMember++;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            currentMember--;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMember += 2;
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMember -= 2;

        currentMember = Mathf.Clamp(currentMember, 0, playerParty.Party.Count - 1);

        partyScreen.UpdateMemberSelection(currentMember);

        if (Input.GetKeyDown(KeyCode.Z))
        {

            if(!playerUnit.CanSwitch())
            {
                partyScreen.SetMessageText($"{playerUnit.Name} can't switch out!");
                return;
            }



            var selectedMember = playerParty.Party[currentMember];
            if(selectedMember.HP <=0 )
            {
                partyScreen.SetMessageText("You can't send out a fainted Pokemon!");
                return;
            }
            if(selectedMember == playerUnit.Pokemon)
            {
                partyScreen.SetMessageText($"This Pokemon is already out!");
                return;
            }

            


            partyScreen.gameObject.SetActive(false);

            state = BattleState.Busy;

            StartCoroutine(SwitchPokemon(selectedMember));
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if(playerUnit.Pokemon.HP > 0)
            {
                partyScreen.gameObject.SetActive(false);
                ActionSelection();
            }
            
        }
    }

    public IEnumerator SwitchPokemon(Pokemon newPokemon)
    {
        bool currentPokemonFainted = true;
        if(playerUnit.Pokemon.HP > 0)
        {
            currentPokemonFainted = false;
            yield return dialogBox.TypeDialog($"Come back {playerUnit.Pokemon.Base.Name}");

            playerUnit.Visual.PlayFaintAnimation();

        }
        yield return new WaitForSeconds(1f);

        playerUnit.Setup(newPokemon, this);

        dialogBox.SetMoveName(newPokemon.Moves);

        yield return dialogBox.TypeDialog($"Go {newPokemon.Base.Name}");

        if (currentPokemonFainted)
            ActionSelection();
        else
        {
            dialogBox.EnableActionSelector(false);
            dialogBox.EnableDialogText(true);
            yield return RunTurn();
        }
            

    }
    IEnumerator RunTurn(bool generateMove = true)
    {
        state = BattleState.PerformMove;


        if(generateMove)
        {
            var move = enemyUnit.GetRandomMove();
            moveQueue.Enqueue(move, enemyUnit, playerUnit);
        }

        yield return moveQueue.Prepare();
        



        while(moveQueue.Count > 0)
        {
            var node = moveQueue.Dequeue();
            if (node.Source.HP <= 0) continue;

            if(node.Move.Disabled)
            {
                yield return dialogBox.TypeDialog("But it failed!");
                yield break;
            }

            yield return node.Source.OnBeforeMove();

            if(node.Source.CanMove)
                yield return RunMove(node);
        }

        yield return playerUnit.OnTurnEnd();
        yield return enemyUnit.OnTurnEnd();


        CheckForBattleOver();

        if (!playerUnit.LockedAction)
            ActionSelection();
        else
        {
            moveQueue.Enqueue(playerUnit.LastUsedMove, playerUnit, enemyUnit);
            yield return RunTurn();
        }
    }

    IEnumerator RunMove(MoveNode node)
    {
        
        var move = node.Move;
        var source = node.Source;
        var target = node.Target;

        yield return source.RunMove(move, target);
        
        yield return ShowMessages();
    }

    public IEnumerator ShowMessages()
    {
        while(MessageQueue.Count > 0)
        {
            yield return dialogBox.TypeDialog(MessageQueue.Dequeue());
        }
    }
    public void CheckForBattleOver()
    {
        if(enemyUnit.HP <= 0)
        {
            BattleOver(true);
        }
        else if(playerUnit.HP <= 0)
        {
            var nextPokemon = playerParty.GetHealthyPokemon();

            if (nextPokemon is object)
                OpenPartyScreen();
            else
                BattleOver(false);
        }
    }
}
