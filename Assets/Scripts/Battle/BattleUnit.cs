using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayerUnit;
    public Pokemon Pokemon { get; set; }

    public bool IsPlayerUnit { get => isPlayerUnit; }
    public string Name { get => Pokemon.Name; }
    public int Level { get => Pokemon.Level; }

    public List<Move> Moves { get; set; }
    public StatusCondition Status { get => Pokemon.Status; set => Pokemon.Status = value; }


    //public BattleSystem System { get; private set; }
    public BattleVisual Visual { get; private set; }

    public bool LockedAction;
    public bool LockedMove;
    public bool EndTurn;

    public System.Func<MoveBase, BattleUnit, bool> Invulnerability = (m, s) => false;
    public System.Func<bool> CanSwitch = () => true;

    public StatStage StatStage { get; set; }
    public Dictionary<VolatileID, VolatileCondition> Volatiles;

    public Dictionary<BattleUnit, MoveBase> MirrorMoves;
    public int Weight { get => Pokemon.Base.Weight; }
    public int Height { get => Pokemon.Base.Height; }

    int _attack;
    int _defense;
    int _spAttack;
    int _spDefense;
    int _speed;
    public int Attack
    {
        get => Mathf.FloorToInt(_attack * StatStage[BoostableStat.Attack]);
    }
    public int Defense
    {
        get => Mathf.FloorToInt(_defense * StatStage[BoostableStat.Defense]);
    }
    public int SpAttack
    {
        get => Mathf.FloorToInt(_spAttack * StatStage[BoostableStat.SpAttack]);
    }
    public int SpDefense
    {
        get => Mathf.FloorToInt(_spDefense * StatStage[BoostableStat.SpDefense]);
    }
    public int Speed
    {
        get => Mathf.FloorToInt(_speed * StatStage[BoostableStat.Speed] * SpeedMod());
    }
    public int HP { get => Pokemon.HP; }
    public int MaxHP { get => Pokemon.MaxHP; }

    public float Accuracy { get => StatStage[BoostableStat.Accuracy]; }
    public float Evasion { get => StatStage[BoostableStat.Evasion]; }

    public List<PokemonType> Types { get; set; }
    public int CritStage { get => StatStage.CritStage; }

    private void Awake()
    {
        Visual = GetComponent<BattleVisual>();
    }

    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;
        Types = new List<PokemonType>() { pokemon.Base.Type1, pokemon.Base.Type2 };

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
    public IEnumerator RunMove(Move move, BattleUnit target)
    {
        LastUsedMove = move;

        yield return move.Base.Run(this, target);
    }

    public IEnumerator TakeDamage(int damage)
    {
        Pokemon.UpdateHP(damage);
        yield return Visual.HUD.UpdateHP();
        yield return BattleSystem.Instance.ShowMessages();
        if (HP <= 0)
        {
            Visual.PlayFaintAnimation();
            yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Name} fainted!");
        }
    }

    public IEnumerator SetStatusCondition(StatusID id)
    {

        if (Status is object)
        {
            yield return BattleSystem.Instance.DialogBox.TypeDialog($"It doesn't affect {Name}");
            yield break;
        }


        Status = StatusCondition.Create(id);

        Status.Unit = this;
        Visual.HUD.SetStatusImage();

        yield return Status.OnStart();
    }
    public IEnumerator RemoveStatusCondition()
    {
        yield return Status.OnEnd();

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


    public int LastHitDamage;
    public List<System.Func<MoveBase, BattleUnit, int, IEnumerator>> OnHitList = new List<System.Func<MoveBase, BattleUnit, int, IEnumerator>>();
    public IEnumerator OnHit(MoveBase move, BattleUnit source, int damage)
    {
        foreach (var e in OnHitList.ToList())
        {
            yield return e(move, source, damage);
        }
        LastHitDamage = damage;
    }


    public List<System.Func<MoveBase, BattleUnit, float>> AttackerModList = new List<System.Func<MoveBase, BattleUnit, float>>();
    public float AttackerMod(MoveBase move, BattleUnit target)
    {
        float result = 1f;

        foreach (var mod in AttackerModList.ToList())
        {
            result *= mod(move, target);
        }

        return result;
    }

    public List<System.Func<MoveBase, BattleUnit, float>> DefenderModList = new List<System.Func<MoveBase, BattleUnit, float>>();
    public float DefenderMod(MoveBase move, BattleUnit source)
    {
        float result = 1f;

        foreach (var mod in DefenderModList.ToList())
        {
            result *= mod(move, source);
        }

        return result;
    }

    public List<System.Func<float>> SpeedModList = new List<System.Func<float>>();
    public float SpeedMod()
    {
        float result = 1f;

        foreach (var mod in SpeedModList.ToList())
        {
            result *= mod();
        }

        return result;
    }

    //true means can pass the move
    public bool CanMove = true;

    public List<System.Func<IEnumerator>> OnBeforeMoveList = new List<System.Func<IEnumerator>>();

    public IEnumerator OnBeforeMove()
    {
        foreach (var e in OnBeforeMoveList.ToList())
        {
            yield return e();
        }
    }

    public List<System.Func<MoveBase, BattleUnit, bool>> ByPassAccuracyCheckList = new List<System.Func<MoveBase, BattleUnit, bool>>();
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

    public List<System.Func<IEnumerator>> OnTurnEndList = new List<System.Func<IEnumerator>>();
    public IEnumerator OnTurnEnd()
    {
        foreach (var e in OnTurnEndList.ToList())
        {
            yield return e();
        }
        CanMove = true;
    }

    public IEnumerator ApplyStatBoost(Dictionary<BoostableStat, int> boosts)
    {
        foreach (var kvp in boosts)
        {
            var stat = kvp.Key;
            var boost = kvp.Value;
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
                    switch (boost)
                    {
                        case -3:
                            message = "severely fell";
                            break;
                        case -2:
                            message = "harshly fell";
                            break;
                        case -1:
                            message = "fell";
                            break;
                        case 1:
                            message = "rose";
                            break;
                        case 2:
                            message = "rose sharply";
                            break;
                        case 3:
                            message = "rose drastically";
                            break;

                    }
                    break;
            }

            StatStage.Value[stat] = Mathf.Clamp(StatStage.Value[stat] + boost, -6, 6);

            yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Name}'s {stat} {message}!");
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
       
        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Name} gained {expGain} experience points!");
        yield return Visual.HUD.UpdateEXP();
    }

    public IEnumerator LevelUp()
    {
        Pokemon.LevelUp();

        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{Name} grew to {Level}!");
    }
}

