using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] private bool isPlayerUnit;
    public Pokemon Pokemon { get; private set; }
    public bool IsPlayerUnit => isPlayerUnit;
    public string Name => Pokemon.Name;
    public int Level => Pokemon.Level;
    public List<Move> Moves { get; private set; }

    public StatusCondition Status
    {
        get => Pokemon.Status;
        private set => Pokemon.Status = value;
    }

    public BattleVisual Visual { get; private set; }

    public bool LockedAction;
    public bool LockedMove;
    public bool EndTurn;

    public System.Func<MoveBase, BattleUnit, bool> Invulnerability = (m, s) => false;
    public System.Func<bool> CanSwitch = () => true;

    public StatStage StatStage { get; set; }
    public Dictionary<VolatileID, VolatileCondition> Volatiles;

    private Dictionary<BattleUnit, MoveBase> MirrorMoves;
    public int Weight => Pokemon.Base.Weight;
    public int Height => Pokemon.Base.Height;

    private int _attack;
    private int _defense;
    private int _spAttack;
    private int _spDefense;
    private int _speed;
    public int Attack => Mathf.FloorToInt(_attack * StatStage[BoostableStat.Attack]);

    public int Defense => Mathf.FloorToInt(_defense * StatStage[BoostableStat.Defense]);

    public int SpAttack => Mathf.FloorToInt(_spAttack * StatStage[BoostableStat.SpAttack]);

    public int SpDefense => Mathf.FloorToInt(_spDefense * StatStage[BoostableStat.SpDefense]);

    public int Speed => Mathf.FloorToInt(_speed * StatStage[BoostableStat.Speed] * SpeedMod());
    public int HP => Pokemon.HP;
    public int MaxHP => Pokemon.MaxHP;

    public float Accuracy => StatStage[BoostableStat.Accuracy];
    public float Evasion => StatStage[BoostableStat.Evasion];

    public List<PokemonType> Types { get; private set; }
    public int CritStage => StatStage.CritStage;
    public bool IsFainted => HP <= 0;
    private static BattleManager BattleManager => BattleManager.Instance;
    private static Queue<IEnumerator> AnimationQueue => BattleManager.AnimationQueue;
    private static BattleDialogBox DialogBox => BattleManager.DialogBox;

    private void Awake()
    {
        Visual = GetComponent<BattleVisual>();
    }

    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;
        Types = new List<PokemonType> {pokemon.Base.Type1, pokemon.Base.Type2};

        Moves = new List<Move>(Pokemon.Moves);

        StatStage = new StatStage();

        Volatiles = new Dictionary<VolatileID, VolatileCondition>();
        MirrorMoves = new Dictionary<BattleUnit, MoveBase>();

        Visual.Setup();

        Transform(Pokemon);
    }

    public void Transform(Pokemon pokemon)
    {
        _attack = pokemon.Attack;
        _defense = pokemon.Defense;
        _spAttack = pokemon.SpAttack;
        _spDefense = pokemon.SpDefense;
        _speed = pokemon.Speed;
        Visual.Transform(pokemon.Base);
    }

    public Move LastUsedMove;

    public void UseMove(Move move, BattleUnit target)
    {
        LastUsedMove = move;

        AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Name} used {move.Name}!"));
        AnimationQueue.Enqueue(Visual.PlayAttackAnimation());

        move.Base.Run(this, target);
    }

    public void TakeDamage(DamageDetail damage)
    {
        Pokemon.UpdateHP(HP - damage.Value);

        AnimationQueue.Enqueue(Visual.PlayHitAnimation());
        AnimationQueue.Enqueue(Visual.HUD.UpdateHP());
        AnimationQueue.Enqueue(DialogBox.TypeDialog(damage.Messages));

        CheckFaint();
    }

    public void TakeDamage(int damage, string message)
    {
        Pokemon.UpdateHP(HP - damage);

        AnimationQueue.Enqueue(Visual.PlayHitAnimation());
        AnimationQueue.Enqueue(Visual.HUD.UpdateHP());
        AnimationQueue.Enqueue(DialogBox.TypeDialog(message));
    }

    private void CheckFaint()
    {
        if (IsFainted)
        {
            AnimationQueue.Enqueue(Visual.PlayFaintAnimation());
            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Name} fainted!"));
        }
    }

    public void SetStatusCondition(StatusID id)
    {
        if (Status is object)
        {
            AnimationQueue.Enqueue(DialogBox.TypeDialog($"It doesn't affect {Name}"));
        }
        else
        {
            Status = StatusCondition.Create(id, this);
            Visual.HUD.SetStatusImage();
            Status.OnStart();
        }
    }

    public void RemoveStatusCondition()
    {
        Status.OnEnd();
        Status = null;
        Visual.HUD.SetStatusImage();
    }

    public IEnumerator AddVolatileCondition(VolatileCondition condition)
    {
        condition.Unit = this;
        Volatiles[condition.ID] = condition;
        yield return condition.OnStart();
    }

    public IEnumerator RemoveVolatileCondition(VolatileID ID)
    {
        yield return Volatiles[ID].OnEnd();
        Volatiles.Remove(ID);
    }

    public void OnMiss()
    {
        AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Name}'s attack missed"));
    }

    public int LastHitDamage;

    public List<System.Action<MoveBase, BattleUnit, int>> OnHitList =
        new List<System.Action<MoveBase, BattleUnit, int>>();

    public void OnHit(MoveBase move, BattleUnit source, int damage)
    {
        OnHitList.ForEach(a => a(move, source, damage));

        LastHitDamage = damage;
    }


    public List<System.Func<MoveBase, BattleUnit, float>> AttackerModList =
        new List<System.Func<MoveBase, BattleUnit, float>>();

    public float AttackerMod(MoveBase move, BattleUnit target)
    {
        return AttackerModList.ToList().Aggregate(1f, (current, mod) => current * mod(move, target));
    }

    public List<System.Func<MoveBase, BattleUnit, float>> DefenderModList =
        new List<System.Func<MoveBase, BattleUnit, float>>();

    public float DefenderMod(MoveBase move, BattleUnit source)
    {
        return DefenderModList.ToList().Aggregate(1f, (current, mod) => current * mod(move, source));
    }

    public List<System.Func<float>> SpeedModList = new List<System.Func<float>>();

    public float SpeedMod()
    {
        return SpeedModList.ToList().Aggregate(1f, (current, mod) => current * mod());
    }

    //true means can pass the move
    public bool CanMove = true;

    public List<System.Action> OnBeforeMoveList = new List<System.Action>();

    public void OnBeforeMove()
    {
        OnBeforeMoveList.ForEach(a => a());
    }

    public List<System.Func<MoveBase, BattleUnit, bool>> ByPassAccuracyCheckList =
        new List<System.Func<MoveBase, BattleUnit, bool>>();

    //true if pass (hit)
    public bool ByPassAccuracyCheck(MoveBase move, BattleUnit source)
    {
        bool result = false;
        foreach (var e in ByPassAccuracyCheckList)
        {
            result = e(move, source);
        }

        return result;
    }

    public List<System.Action> OnTurnEndList = new List<System.Action>();

    public void OnTurnEnd()
    {
        OnTurnEndList.ForEach(a => a());
    }

    public IEnumerator ApplyStatBoost(Dictionary<BoostableStat, int> boosts)
    {
        foreach (var kvp in boosts)
        {
            var stat = kvp.Key;
            int boost = kvp.Value;
            string message = "changed";

            switch (StatStage.Value[stat])
            {
                case 6:
                    message = "can't go any higher";
                    break;
                case -6:
                    message = "can't go any lower";
                    break;
                default:
                    message = boost switch
                    {
                        -3 => "severely fell",
                        -2 => "harshly fell",
                        -1 => "fell",
                        1 => "rose",
                        2 => "rose sharply",
                        3 => "rose drastically",
                        _ => message
                    };
                    break;
            }
            StatStage.Value[stat] = Mathf.Clamp(StatStage.Value[stat] + boost, -6, 6);

            yield return BattleManager.Instance.DialogBox.TypeDialog($"{Name}'s {stat} {message}!");
        }
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }

    public IEnumerator GainEXP(Pokemon defeatedPokemon, bool isTrainerBattle)
    {
        int exp = defeatedPokemon.ExpReward;
        float trainerBonus = isTrainerBattle ? 1.5f : 1f;
        int expGain = Mathf.FloorToInt(exp * trainerBonus / 7);

        Pokemon.EXP += expGain;

        yield return BattleManager.Instance.DialogBox.TypeDialog($"{Name} gained {expGain} experience points!");
        yield return Visual.HUD.UpdateEXP();
    }

    public IEnumerator LevelUp()
    {
        Pokemon.LevelUp();

        yield return BattleManager.Instance.DialogBox.TypeDialog($"{Name} grew to level {Level}!");

        yield return CheckForNewMoves();
    }

    private IEnumerator CheckForNewMoves()
    {
        var newMoves = Pokemon.GetMovesToLearnOnLevelUp();

        foreach (var move in newMoves)
        {
            var newMove = new Move(move);
            Pokemon.Moves.Add(newMove);
            Moves.Add(newMove);
            if (Pokemon.Moves.Count < 4)
            {
                yield return BattleManager.Instance.DialogBox.TypeDialog($"{Name} learned {move.Name}!");
            }
            else
            {
                yield return BattleManager.Instance.DialogBox.TypeDialog($"{Name} wants to learn {move.Name}!");
                BattleManager.Instance.OpenLearnMoveScreen();
            }
        }
    }
}