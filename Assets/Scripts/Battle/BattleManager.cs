using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Move;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Battle
{
    public enum BattleState
    {
        Start,
        ActionSelection,
        MoveSelection,
        PerformMove,
        Busy,
        PartyScreen,
        LearnMoveScreen,
        BattleOver
    }

    public class BattleManager : MonoBehaviour
    {
        public static BattleManager I { get; private set; }
        
        [SerializeField] private BattleVisual playerSprite;
        [SerializeField] private BattleVisual enemySprite;
        
        [SerializeField] private BattleHUD playerHud;
        [SerializeField] private BattleHUD enemyHud;
        
        private BattleUnit playerUnit = new BattleUnit();
        private BattleUnit enemyUnit = new BattleUnit();

        [SerializeField] private DialogBox dialogBox;
        [SerializeField] private PartyScreen partyScreen;
        [SerializeField] private LearnMoveScreen learnMoveScreen;

        [SerializeField] private GameObject pokeballSprite;

        [SerializeField] private ActionSelector actionSelector;
        [SerializeField] private MoveSelector moveSelector;

        public event Action<bool> OnBattleEnd;

        public Queue<IEnumerator> AnimationQueue { get; } = new Queue<IEnumerator>();
        public BattleUnit PlayerUnit => playerUnit;
        public DialogBox DialogBox => dialogBox;
        public MoveSelector MoveSelector => moveSelector;

        public bool IsTrainerBattle { get; private set; }

        private MoveQueue MoveQueue { get; } = new MoveQueue();

        private BattleState state;

        public PokemonParty PlayerParty { get; private set; }
        public PokemonParty TrainerParty { get; private set; }
        private Pokemon WildPokemon { get; set; }

        private PlayerController player;
        private TrainerController trainer;

        private int escapeAttempts;

        private bool isBattleOver;
        private void Awake()
        {
            I = this;
        }

        private void Start()
        {
            actionSelector.OnSelectAction = SelectAction;

            moveSelector.OnSelectMove = SelectMove;
            moveSelector.OnBack = CloseMoveSelector;
            moveSelector.OnBack += OpenActionSelector;
        }

        public void HandleUpdate()
        {
            switch (state)
            {
                case BattleState.ActionSelection:
                    actionSelector.HandleUpdate();
                    break;
                case BattleState.MoveSelection:
                    moveSelector.HandleUpdate();
                    break;
                case BattleState.PartyScreen:
                    partyScreen.HandleUpdate();
                    break;
                case BattleState.LearnMoveScreen:
                    learnMoveScreen.HandleUpdate();
                    break;
                case BattleState.Start:
                    break;
                case BattleState.PerformMove:
                    break;
                case BattleState.Busy:
                    break;
                case BattleState.BattleOver:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //test button
            if (Input.GetKeyDown(KeyCode.T))
            {
                StartCoroutine(ThrowPokeball());
            }
        }

        private IEnumerator EndBattleSequence(bool won)
        {
            state = BattleState.BattleOver;
            yield return PlayAnimation();

            OnBattleEnd?.Invoke(won);
        }
        
        public void StartWildBattle(PokemonParty playerParty, Pokemon wildPokemon)
        {
            PlayerParty = playerParty;
            WildPokemon = wildPokemon;

            IsTrainerBattle = false;
            player = PlayerParty.GetComponent<PlayerController>();

            StartCoroutine(SetupBattle());
        }

        public void StartTrainerBattle(PokemonParty playerParty, PokemonParty trainerParty)
        {
            PlayerParty = playerParty;
            TrainerParty = trainerParty;

            IsTrainerBattle = true;
            player = playerParty.GetComponent<PlayerController>();
            trainer = trainerParty.GetComponent<TrainerController>();

            StartCoroutine(SetupBattle());
        }

        private void ResetBattleState()
        {
            isBattleOver = false;
            escapeAttempts = 0;
            
            playerSprite.gameObject.SetActive(false);
            enemySprite.gameObject.SetActive(false);
            playerHud.gameObject.SetActive(false);
            enemyHud.gameObject.SetActive(false);
            dialogBox.gameObject.SetActive(true);
        }
        
        private IEnumerator SetupBattle()
        {
            ResetBattleState();
            
            playerUnit.Setup(PlayerParty.GetHealthyPokemon());
            playerSprite.Setup(playerUnit);
            playerHud.Setup(playerUnit);
            
            if (IsTrainerBattle)
            {
                enemyUnit.Setup(TrainerParty.GetHealthyPokemon());
                enemySprite.Setup(enemyUnit);
                enemyHud.Setup(enemyUnit);
                
                yield return dialogBox.TypeDialog($"{trainer.Name} wants to battle!");
                yield return dialogBox.TypeDialog($"{trainer.Name} sent out {enemyUnit.Name}!");
                yield return enemySprite.PlayEnterAnimation();
            }
            else
            {
                enemyUnit.Setup(WildPokemon);
                enemySprite.Setup(enemyUnit);
                enemyHud.Setup(enemyUnit);
                
                yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared.");
                yield return enemySprite.PlayEnterAnimation();
            }
            
            yield return dialogBox.TypeDialog($"Go {playerUnit.Name}!");
            yield return playerSprite.PlayEnterAnimation();
            
            OpenActionSelector();
        }

        public void Log(string message)
        {
            AnimationQueue.Enqueue(DialogBox.TypeDialog(message));
        }

        public void Log(IEnumerable<string> messages)
        {
            AnimationQueue.Enqueue(DialogBox.TypeDialog(messages));
        }
        
        private void SelectAction(int currentAction)
        {
            switch (currentAction)
            {
                case 0: //Fight
                    OpenMoveSelector();
                    break;
                case 1: //Party
                    OpenPartyScreenManual();
                    break;
                case 2: //Bag
                    StartCoroutine(ThrowPokeball());
                    break;
                case 3: //Run
                    StartCoroutine(TryToEscape());
                    break;
            }
        }

        private void SelectMove(int index)
        {
            var move = playerUnit.Moves[index];

            if (!move.IsDisabled)
            {
                MoveQueue.Enqueue(move, playerUnit, GetMoveTarget(move, playerUnit, enemyUnit));

                CloseMoveSelector();
                StartCoroutine(RunTurn());
            }
        }

        private void SelectMember(Pokemon pokemon)
        {
            ClosePartyScreen();
            StartCoroutine(SwitchPokemon(pokemon));
        }

        private void SelectMemberForced(Pokemon pokemon)
        {
            ClosePartyScreen();
            StartCoroutine(ReplacePokemon(pokemon));
        }

        private IEnumerator ReplacePokemon(Pokemon pokemon)
        {
            state = BattleState.Busy;

            playerUnit.Setup(pokemon);
            yield return dialogBox.TypeDialog($"Go {pokemon.Base.Name}");

            OpenActionSelector();
        }

        private IEnumerator SwitchPokemon(Pokemon pokemon)
        {
            state = BattleState.Busy;

            yield return dialogBox.TypeDialog($"Come back {playerUnit.Pokemon.Base.Name}");
            yield return playerSprite.PlayFaintAnimation();
            playerUnit.Setup(pokemon);
            yield return dialogBox.TypeDialog($"Go {pokemon.Base.Name}");

            yield return RunTurn();
        }

        private IEnumerator SendNextTrainerPokemon(Pokemon nextPokemon)
        {
            state = BattleState.Busy;
            enemyUnit.Setup(nextPokemon);
            yield return dialogBox.TypeDialog($"{trainer.Name} sent out {enemyUnit.Name}!");
            OpenActionSelector();
        }

        public void SwitchTrainerPokemon(Pokemon nextPokemon, BattleUnit unit)
        {
            MoveQueue.Cancel(unit);
            unit.Setup(nextPokemon);
        }

        private static BattleUnit GetMoveTarget(Move.Move move, BattleUnit self, BattleUnit foe)
        {
            return move.Base.Target == MoveTarget.Foe ? foe : self;
        }

        private IEnumerator RunTurn()
        {
            state = BattleState.PerformMove;

            var move = enemyUnit.GetRandomMove();
            MoveQueue.Enqueue(move, enemyUnit, GetMoveTarget(move, enemyUnit, playerUnit));

            while (MoveQueue.Count > 0)
            {
                var node = MoveQueue.Dequeue();
                RunMove(node);
            }

            if (isBattleOver) yield return EndBattleSequence(true);
            
            OnTurnEnd();
            yield return PlayAnimation();

            yield return CheckForBattleOver();
        }
        
        private IEnumerator PlayAnimation()
        {
            while (AnimationQueue.Count > 0)
            {
                var enumerator = AnimationQueue.Dequeue();
                yield return enumerator;
            }
        }

        private void OnTurnEnd()
        {
            if (playerUnit.Hp > 0) playerUnit.Modifier.OnTurnEnd();
            if (enemyUnit.Hp > 0) enemyUnit.Modifier.OnTurnEnd();
        }

        private static void RunMove(MoveNode node)
        {
            var move = node.Move;
            var source = node.Source;
            var target = node.Target;

            if (!source.IsFainted)
            {
                source.UseMove(move, target);
            }
        }

        private IEnumerator CheckForBattleOver()
        {
            state = BattleState.Busy;
            if (enemyUnit.Pokemon is null || enemyUnit.Hp <= 0)
            {
                //gain exp
                yield return playerUnit.GainEXP(enemyUnit.Pokemon, IsTrainerBattle);
                yield return new WaitForSeconds(0.5f);

                yield return new WaitUntil(() => state == BattleState.Busy);

                if (IsTrainerBattle)
                {
                    var nextPokemon = TrainerParty.GetHealthyPokemon();
                    if (nextPokemon is object)
                    {
                        yield return SendNextTrainerPokemon(nextPokemon);
                    }
                    else
                    {
                        yield return EndBattleSequence(true);
                    }
                }
                else
                {
                    yield return EndBattleSequence(true);
                }
            }
            else if (playerUnit.Hp <= 0)
            {
                var nextPokemon = PlayerParty.GetHealthyPokemon();

                if (nextPokemon is object)
                {
                    OpenPartyScreenForced();
                }
                else
                {
                    yield return EndBattleSequence(false);
                }
            }
            else
            {
                if (!playerUnit.LockedAction)
                {
                    OpenActionSelector();
                }
                else
                {
                    var move = playerUnit.Moves.FirstOrDefault();
                    // MoveQueue.Enqueue(playerUnit.LastUsedMove, playerUnit, enemyUnit);
                    yield return RunTurn();
                }
            }
        }

        private IEnumerator ThrowPokeball()
        {
            state = BattleState.Busy;
            CloseActionSelector();

            if (IsTrainerBattle)
            {
                yield return dialogBox.TypeDialog("You can't steal a trainer Pokemon!");
                yield break;
            }

            yield return dialogBox.TypeDialog($"{player.Name} used Pokeball!");

            // var pokeballObject = Instantiate(pokeballSprite, playerUnit.transform.position - new Vector3(2, 0),
            //     Quaternion.identity);
            // var pokeball = pokeballObject.GetComponent<SpriteRenderer>();
            //
            // //Animations
            // //Throwing ball
            // var position = enemyUnit.transform.position;
            // yield return pokeball.transform.DOJump(position + new Vector3(0, 2), 2f, 1, 1f)
            //     .WaitForCompletion();
            // yield return enemyUnit.Visual.PlayCaptureAnimation();
            // yield return pokeball.transform.DOMoveY(position.y - 1.3f, 0.5f).WaitForCompletion();
            // //shake pokeball
            // int shakeCount = TryToCatchPokemon(enemyUnit.Pokemon);
            // Debug.Log(shakeCount);
            //
            // for (int i = 0; i < Mathf.Min(shakeCount, 3); i++)
            // {
            //     yield return new WaitForSeconds(0.5f);
            //     yield return pokeball.transform.DOPunchRotation(new Vector3(0, 0, 10f), 0.8f).WaitForCompletion();
            // }
            //
            // if (shakeCount == 4)
            // {
            //     //pokemon caught
            //     yield return dialogBox.TypeDialog($"{enemyUnit.Name} was caught!");
            //     yield return pokeball.DOFade(0, 1.5f).WaitForCompletion();
            //
            //     PlayerParty.AddPokemon(enemyUnit.Pokemon);
            //     yield return dialogBox.TypeDialog($"{enemyUnit.Name} has been added to your party!");
            //
            //     yield return  EndBattleSequence(true);
            // }
            // else
            // {
            //     //pokemon not caught
            //     yield return new WaitForSeconds(1f);
            //     pokeball.DOFade(0, 0.2f);
            //     yield return enemyUnit.Visual.PlayBreakoutAnimation();
            //
            //     if (shakeCount < 2)
            //         yield return dialogBox.TypeDialog($"{enemyUnit.Name} broke free!");
            //     else
            //         yield return dialogBox.TypeDialog($"It was so close!");
            // }
            //
            // Destroy(pokeball);

            yield return RunTurn();
        }

        private static int TryToCatchPokemon(Pokemon pokemon)
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
                if (random >= b)
                    break;

                shakeCount++;
            }

            return shakeCount;
        }

        private IEnumerator TryToEscape()
        {
            state = BattleState.Busy;

            if (IsTrainerBattle)
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
                    yield return EndBattleSequence(true);
                }
                else
                {
                    yield return dialogBox.TypeDialog("Can't escape!");
                    yield return RunTurn();
                }
            }
        }

        private void OpenActionSelector()
        {
            state = BattleState.ActionSelection;
            dialogBox.SetDialog("Choose an action");
            actionSelector.gameObject.SetActive(true);
        }

        private void CloseActionSelector()
        {
            actionSelector.gameObject.SetActive(false);
        }

        private void OpenMoveSelector()
        {
            state = BattleState.MoveSelection;
            CloseActionSelector();
            moveSelector.gameObject.SetActive(true);
            moveSelector.SetMoves(playerUnit.Moves);
        }

        private void CloseMoveSelector()
        {
            moveSelector.gameObject.SetActive(false);
        }

        /// <summary>
        /// Open Party Screen manually by "Party" button from Action Selection
        /// </summary>
        private void OpenPartyScreenManual()
        {
            partyScreen.OnSelectMember = SelectMember;
            partyScreen.OnBack = () =>
            {
                ClosePartyScreen();
                OpenActionSelector();
            };

            OpenPartyScreen();
        }

        /// <summary>
        /// Open Party Screen forcibly when player pokemon fainted at turn end
        /// </summary>
        private void OpenPartyScreenForced()
        {
            partyScreen.OnSelectMember = SelectMemberForced;
            partyScreen.OnBack = null;

            OpenPartyScreen();
        }

        private void OpenPartyScreen()
        {
            state = BattleState.PartyScreen;
            partyScreen.gameObject.SetActive(true);
            partyScreen.Init();
        }

        private void ClosePartyScreen()
        {
            partyScreen.gameObject.SetActive(false);
        }

        public void OpenLearnMoveScreen()
        {
            state = BattleState.LearnMoveScreen;
            learnMoveScreen.gameObject.SetActive(true);
            learnMoveScreen.SetMovesData(playerUnit);
        }

        public IEnumerator CloseLearnMoveScreen(MoveBase forgetMove, MoveBase newMove)
        {
            learnMoveScreen.gameObject.SetActive(false);

            yield return
                dialogBox.TypeDialog($"{playerUnit.Name} forgot {forgetMove.Name} and learned {newMove.Name}!");

            state = BattleState.Busy;
        }

        private readonly Dictionary<Pokemon, int> paydays = new Dictionary<Pokemon, int>();

        public void Payday(Pokemon pokemon)
        {
            paydays.TryGetValue(pokemon, out int count);
            paydays[pokemon] = ++count;
        }
        /// <summary>
        /// Forcibly end battle (Whirlwind, Roar,...)
        /// </summary>
        public void ForcedEndBattle()
        {
            MoveQueue.Clear();
            isBattleOver = true;
        }
    }
}