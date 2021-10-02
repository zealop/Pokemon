using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState
{
    Start, ActionSelection, MoveSelection, PerformMove, Busy, PartyScreen, LearnMoveScreen, BattleOver
}

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem Instance { get; private set; }

    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;

    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] LearnMoveScreen learnMoveScreen;

    [SerializeField] Image playerImage;
    [SerializeField] Image trainerImage;

    [SerializeField] GameObject pokeballSprite;

    [SerializeField] ActionSelector actionSelector;
    [SerializeField] MoveSelector moveSelector;

    public event System.Action<bool> OnBattleOver;

    public BattleDialogBox DialogBox => dialogBox;
    public MoveSelector MoveSelector => moveSelector;
    public Queue<string> MessageQueue { get; } = new Queue<string>();
    public MoveQueue moveQueue = new MoveQueue();


    BattleState state;
    int currentAction;
    int currentMove;
    int currentMember;


    PokemonParty playerParty;
    PokemonParty trainerParty;
    Pokemon wildPokemon;

    bool isTrainerBattle = false;
    PlayerController player;
    TrainerController trainer;

    int escapeAttempts;

    private void Awake()
    {
        Instance = this;
    }
    public PokemonParty PlayerParty { get => playerParty; }
    public void StartBattle(PokemonParty party, Pokemon wild)
    {
        this.playerParty = party;
        this.wildPokemon = wild;

        player = playerParty.GetComponent<PlayerController>();

        StartCoroutine(SetupBattle());
    }
    public void StartTrainerBattle(PokemonParty playerParty, PokemonParty trainerParty)
    {
        this.playerParty = playerParty;
        this.trainerParty = trainerParty;

        isTrainerBattle = true;
        player = playerParty.GetComponent<PlayerController>();
        trainer = trainerParty.GetComponent<TrainerController>();

        StartCoroutine(SetupBattle());
    }
    public IEnumerator SetupBattle()
    {
        playerUnit.Visual.Clear();
        enemyUnit.Visual.Clear();

        playerUnit.Setup(playerParty.GetHealthyPokemon());


        if (isTrainerBattle)
        {
            //show trainer and player
            playerUnit.gameObject.SetActive(false);
            enemyUnit.gameObject.SetActive(false);

            playerImage.sprite = player.Sprite;
            trainerImage.sprite = trainer.Sprite;
            playerImage.gameObject.SetActive(true);
            trainerImage.gameObject.SetActive(true);

            yield return dialogBox.TypeDialog($"{trainer.Name} wants to battle!.");

            //send out trainer pokemon
            trainerImage.gameObject.SetActive(false);
            enemyUnit.gameObject.SetActive(true);

            enemyUnit.Setup(trainerParty.GetHealthyPokemon());

            yield return dialogBox.TypeDialog($"{trainer.Name} sent out {enemyUnit.Name}!");

            //send out player pokemon
            playerImage.gameObject.SetActive(false);
            playerUnit.gameObject.SetActive(true);
        }
        else
        {
            escapeAttempts = 0;
            enemyUnit.Setup(wildPokemon);

            yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared.");
        }



        yield return dialogBox.TypeDialog($"Go {playerUnit.Name}!");

        //partyScreen.Init();

        ActionSelection();
    }

    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        dialogBox.SetDialog("Choose an action");
        actionSelector.gameObject.SetActive(true);
    }

    void MoveSelection()
    {
        state = BattleState.MoveSelection;

        actionSelector.gameObject.SetActive(false);
        moveSelector.gameObject.SetActive(true);
        moveSelector.SetMoves(playerUnit.Moves);
        //dialogBox.EnableDialogText(true);
        //dialogBox.EnableMoveSelector(true);
    }

    public void BattleOver(bool won)
    {
        state = BattleState.BattleOver;

        //playerParty.Party.ForEach(p => p.OnBattleOver());

        OnBattleOver(won);
    }
    public void HandleUpdate()
    {
        if (state == BattleState.ActionSelection)
        {
            HandleActionSelection();

        }
        else if (state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
        else if (state == BattleState.LearnMoveScreen)
        {
            learnMoveScreen.HandleUpdate();
        }
        //test button
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(ThrowPokeball());
        }
    }


    public void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;


        partyScreen.gameObject.SetActive(true);
        partyScreen.SetPartyData(playerParty.Party);
    }

    public void OpenLearnMoveScreen()
    {
        state = BattleState.LearnMoveScreen;


        learnMoveScreen.gameObject.SetActive(true);
        learnMoveScreen.SetMovesData(playerUnit);
    }
    public void CloseMoveScreen(MoveBase forgetmove, MoveBase newMove)
    {
        learnMoveScreen.gameObject.SetActive(false);

        MessageQueue.Enqueue($"{playerUnit.Name} forgot {forgetmove.Name} and learned {newMove.Name}!");

        state = BattleState.Busy;
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            currentAction++;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            currentAction--;

        currentAction = (currentAction + 4) % 4;

        actionSelector.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            switch (currentAction)
            {
                case 0://Fight
                    MoveSelection();
                    break;
                case 1: //Party
                    OpenPartyScreen();
                    break;
                case 2: //Bag
                    StartCoroutine(ThrowPokeball());
                    break;
                case 3: //Run
                    StartCoroutine(TryToEscape());
                    break;

            }
        }
    }

    void HandleMoveSelection()
    {
        int count = playerUnit.Moves.Count;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMove++;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMove--;

        currentMove = (currentMove + count) % count;

        moveSelector.UpdateMoveSelection(currentMove);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            moveSelector.gameObject.SetActive(false);
            dialogBox.EnableDialogText(true);

            var move = playerUnit.Moves[currentMove];

            if (!move.Disabled)
            {
                moveQueue.Enqueue(move, playerUnit, enemyUnit);

                StartCoroutine(RunTurn());
            }


        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            moveSelector.gameObject.SetActive(false);

            dialogBox.EnableDialogText(true);
            ActionSelection();
        }
    }

    void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMember++;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMember--;

        currentMember = (currentMember + playerParty.Count) % playerParty.Count;

        partyScreen.UpdateMemberSelection(currentMember);

        if (Input.GetKeyDown(KeyCode.Z))
        {

            if (!playerUnit.CanSwitch())
            {
                partyScreen.SetMessageText($"{playerUnit.Name} can't switch out!");
                return;
            }
            var selectedMember = playerParty.Party[currentMember];
            if (selectedMember.HP <= 0)
            {
                partyScreen.SetMessageText("You can't send out a fainted Pokemon!");
                return;
            }
            if (selectedMember == playerUnit.Pokemon)
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
            if (playerUnit.Pokemon.HP > 0)
            {
                partyScreen.gameObject.SetActive(false);
                ActionSelection();
            }

        }
    }

    public IEnumerator SwitchPokemon(Pokemon newPokemon)
    {
        bool currentPokemonFainted = true;
        if (playerUnit.Pokemon.HP > 0)
        {
            currentPokemonFainted = false;
            yield return dialogBox.TypeDialog($"Come back {playerUnit.Pokemon.Base.Name}");

            playerUnit.Visual.PlayFaintAnimation();

        }
        yield return new WaitForSeconds(1f);

        playerUnit.Setup(newPokemon);

        moveSelector.SetMoves(newPokemon.Moves);

        yield return dialogBox.TypeDialog($"Go {newPokemon.Base.Name}");

        if (currentPokemonFainted)
            ActionSelection();
        else
        {
            actionSelector.gameObject.SetActive(false);
            dialogBox.EnableDialogText(true);
            yield return RunTurn();
        }
    }
    public IEnumerator SendNextTrainerPokemon(Pokemon nextPokemon)
    {
        state = BattleState.Busy;
        enemyUnit.Setup(nextPokemon);
        yield return dialogBox.TypeDialog($"{trainer.Name} sent out {enemyUnit.Name}!");
        ActionSelection();

    }
    IEnumerator RunTurn(bool generateMove = true)
    {
        state = BattleState.PerformMove;


        if (generateMove)
        {
            var move = enemyUnit.GetRandomMove();
            moveQueue.Enqueue(move, enemyUnit, playerUnit);
        }

        yield return moveQueue.Prepare();




        while (moveQueue.Count > 0)
        {
            var node = moveQueue.Dequeue();
            if (node.Source.HP <= 0) continue;

            if (node.Move.Disabled)
            {
                yield return dialogBox.TypeDialog("But it failed!");
                yield break;
            }

            yield return node.Source.OnBeforeMove();

            if (node.Source.CanMove)
                yield return RunMove(node);
        }

        if (playerUnit.HP > 0) yield return playerUnit.OnTurnEnd();
        if (enemyUnit.HP > 0) yield return enemyUnit.OnTurnEnd();

        yield return CheckForBattleOver();

        if (state != BattleState.PerformMove)
        {
            yield break;
        }


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
    }

    public IEnumerator ShowMessages()
    {
        while (MessageQueue.Count > 0)
        {
            yield return dialogBox.TypeDialog(MessageQueue.Dequeue());
        }
    }
    public IEnumerator CheckForBattleOver()
    {
        state = BattleState.Busy;
        if (enemyUnit.HP <= 0)
        {
            //gain exp
            yield return playerUnit.GainEXP(enemyUnit.Pokemon, isTrainerBattle);
            yield return new WaitForSeconds(0.5f);

            yield return new WaitUntil(() => state == BattleState.Busy);

            yield return ShowMessages();

            if (isTrainerBattle)
            {
                var nextPokemon = trainerParty.GetHealthyPokemon();
                if (nextPokemon is object)
                {
                    yield return SendNextTrainerPokemon(nextPokemon);
                }
                else
                {
                    BattleOver(true);
                }
            }
            else
            {
                BattleOver(true);
            }

        }
        else if (playerUnit.HP <= 0)
        {
            var nextPokemon = playerParty.GetHealthyPokemon();

            if (nextPokemon is object)
            {

                OpenPartyScreen();
            }

            else
                BattleOver(false);
        }
    }

    IEnumerator ThrowPokeball()
    {
        state = BattleState.Busy;
        actionSelector.gameObject.SetActive(false);

        if (isTrainerBattle)
        {
            yield return dialogBox.TypeDialog("You can't steal a trainer Pokemon!");
            yield break;
        }

        yield return dialogBox.TypeDialog($"{player.Name} used Pokeball!");

        var pokeballObject = Instantiate(pokeballSprite, playerUnit.transform.position - new Vector3(2, 0), Quaternion.identity);
        var pokeball = pokeballObject.GetComponent<SpriteRenderer>();

        //Animations
        //Throwing ball
        yield return pokeball.transform.DOJump(enemyUnit.transform.position + new Vector3(0, 2), 2f, 1, 1f).WaitForCompletion();
        yield return enemyUnit.Visual.PlayCaptureAnimation();
        yield return pokeball.transform.DOMoveY(enemyUnit.transform.position.y - 1.3f, 0.5f).WaitForCompletion();
        //shake pokball
        int shakeCount = TryToCatchPokemon(enemyUnit.Pokemon);


        for (int i = 0; i < Mathf.Max(shakeCount, 3); i++)
        {
            yield return new WaitForSeconds(0.5f);
            yield return pokeball.transform.DOPunchRotation(new Vector3(0, 0, 10f), 0.8f).WaitForCompletion();
        }

        if (shakeCount == 4)
        {
            //pokemon caught
            yield return dialogBox.TypeDialog($"{enemyUnit.Name} was caught!");
            yield return pokeball.DOFade(0, 1.5f).WaitForCompletion();

            playerParty.AddPokemon(enemyUnit.Pokemon);
            yield return dialogBox.TypeDialog($"{enemyUnit.Name} has been added to your party!");

            BattleOver(true);
        }
        else
        {
            //pokemon not caught
            yield return new WaitForSeconds(1f);
            pokeball.DOFade(0, 0.2f);
            yield return enemyUnit.Visual.PlayBreakoutAnimation();

            if (shakeCount < 2)
                yield return dialogBox.TypeDialog($"{enemyUnit.Name} broke free!");
            else
                yield return dialogBox.TypeDialog($"It was so close!");
        }
        Destroy(pokeball);

        yield return RunTurn();
    }

    int TryToCatchPokemon(Pokemon pokemon)
    {
        int catchRate = pokemon.Base.CatchRate;
        float statusBonus = StatusCondition.CatchBonus(pokemon.Status);
        float a = (3 * pokemon.MaxHP - 2 * pokemon.HP) * catchRate * statusBonus / (3 * pokemon.MaxHP);

        //auto caught
        if (a >= 255)
            return 4;

        float b = 1048560 / Mathf.Pow(16711680 / a, 0.25f);
        int shakeCount = 0;

        while (shakeCount < 4)
        {
            int random = Random.Range(0, 65536);
            Debug.Log(random);
            if (random >= b)
                break;

            shakeCount++;
        }
        return shakeCount;
    }

    IEnumerator TryToEscape()
    {
        state = BattleState.Busy;

        if (isTrainerBattle)
        {
            yield return dialogBox.TypeDialog("You can't run from a trainer battle!");
            yield return RunTurn();
        }
        else
        {
            int playerSpeed = playerUnit.Pokemon.Speed;
            int enemySpeed = enemyUnit.Pokemon.Speed;

            escapeAttempts++;

            int oddEscape = (playerSpeed * 128 / enemySpeed + 30 * escapeAttempts) % 256;
            if (enemySpeed < playerSpeed || Random.Range(0, 256) < oddEscape)
            {
                yield return dialogBox.TypeDialog("Ran away safely!");
                BattleOver(true);
            }
            else
            {
                yield return dialogBox.TypeDialog("Can't escape!");
                yield return RunTurn();
            }
        }

    }


}
